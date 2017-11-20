/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
// -----------------------------------------------------------------------
// <copyright file="session-detail.ctrl.js" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

(function () {
    'use strict';

    angular
        .module('app.controllers')
        .controller('SessionDetailCtrl', SessionDetailCtrl);

    SessionDetailCtrl.$inject = ['$rootScope', 'session', 'options', '$state', 'config', 'ResourceService',
        'UtilsService', 'ValidationService', 'team', 'sessionType', 'drills', 'practices', 'locations'];

    function SessionDetailCtrl($rootScope, session, options, $state, config, ResourceService, UtilsService, ValidationService, team, sessionType, drills, practices, locations) {
        console.log("SessionDetailCtrl");
        var vm = this;

        team = JSON.parse(localStorage.getItem("currentTeam"));

        options.Players = initializePlayers(team.users);

        vm.step1 = initStep1();
        vm.step2 = initStep2();
        vm.step3 = initStep3();

        vm.session = initializeSession(session);
        vm.isSessionAlreadyStarted = checkIfSessionAlreadyStarted(session);

        vm.openStep = openStep;
        vm.getDrills = getDrills;
        vm.displayValues = displayValues;
        vm.drillSelected = null;
        vm.save = save;
        vm.originalPracticeId = 0;

        return vm;

        function initStep1() {
            return {
                sessionTypeOptions: sessionType,
                practiceOptions: practices,
                drillsOptions: drills,
                sessionLocationOptions: locations,
                timezone: {
                    offset: session && session.id !== config.idForNewItems
                        ? Date.getLocalTimezoneOffsetAsInteger()
                        : -(moment.tz.zone(moment.tz.guess()).offset(new Date()) / 60)
                }
            };
        }

        function getDrills() {
            var drillsByPractice = [];
            for (var i = 0; i < drills.length; i++) {
                if (drills[i].practiceId === vm.session.typeId)
                    drillsByPractice.push(drills[i].drill);
            }
            return drillsByPractice;
        }

        function displayValues() {
            vm.drillSelected = drills[vm.session.subTypeId];
        }

        function initStep2() {
            return {
                players: options.Players
            };
        }

        function initStep3() {
            return {
                startDate: calculateStartDate,
                type: calculateTypeName,
                subType: calculateSubTypeName,
                location: calculateLocationName
            };
        }

        function openStep(stepId) {
            $('#headingStep' + stepId).click();
        }        

        function save() {           
            var sessionToSubmit = vm.isSessionAlreadyStarted
                ? filterOutDisabledFields(vm.session)
                : prepareSessionToSave(vm.session);

            var validationMessage = vm.isSessionAlreadyStarted
                ? ValidationService.validatePastSession(sessionToSubmit)
                : ValidationService.validateFutureSession(sessionToSubmit);

            if (validationMessage) {
                $rootScope.showAlert(validationMessage);
                return;
            }

            $rootScope.$emit('showSpinner');

            var time = Time.parseTimeString(vm.session.time);
            var localOffsetInHours = Date.getLocalTimezoneOffsetAsInteger();

            sessionToSubmit.startDateTime.setHours(time.hours + localOffsetInHours);

            var newSession = {
                sessionType: vm.session.sessionType,
                scheduled: sessionToSubmit.startDateTime,
                location: {
                    name: "Gym",
                    address: null,
                    type: 0,
                    id: sessionToSubmit.location
                },
                users: sessionToSubmit.players,
                id: session.id || 0
            };

            var promise = session && session.id !== config.idForNewItems
                ? ResourceService.updateSession(newSession).then(updateSessionSuccess, updateSessionError)
                : ResourceService.createSession(newSession).then(createSessionSuccess, createSessionError);

            function createSessionSuccess(sessionId) {              

                var addSessionSession = {
                    sessionType: vm.session.sessionType,
                    scheduled: sessionToSubmit.startDateTime,
                    location: {
                        name: "Gym",
                        address: null,
                        type: 0,
                        id: 1
                    },
                    users: sessionToSubmit.players,
                    id: sessionId
                }
                ResourceService.addUsersToSession(addSessionSession).then(addSessionUsersSuccess);

                function addSessionUsersSuccess() {
                    ResourceService.getPractice(vm.session.typeId).then(getPraticeSessionSuccess);                    
                    function getPraticeSessionSuccess(practice) {
                        practice.sessionId = sessionId;
                        ResourceService.updateSessionPractice(practice).then(updatePraticeSessionSuccess);

                        function updatePraticeSessionSuccess() {
                            promise.then(successHandler, errorHandler);
                        }
                    }
                }
            }

            function createSessionError() {
                
            }

            function updateSessionSuccess() {
                if (vm.originalPracticeId != vm.session.typeId) {
                    ResourceService.getPractice(vm.session.typeId).then(getPraticeSessionSuccess);
                    function getPraticeSessionSuccess(practice) {
                        practice.sessionId = session.id;
                        ResourceService.updateSessionPractice(practice).then(updatePraticeSessionSuccess);
                        function updatePraticeSessionSuccess() {
                            promise.then(successHandler, errorHandler);
                        }
                    }
                }
                else {
                    promise.then(successHandler, errorHandler);
                }
            }

            function updateSessionError() {
            }

        }

        function successHandler() {
            $state.go('app.sessions-day', {date: null});
        }

        function errorHandler(response) {
            $rootScope.$emit('hideSpinner');

            var message = response.status === 400
                ? 'One of fields has been filled with unacceptable value. Please check the values and try again.'
                : isOverlapIssue(response.errors);

            $rootScope.showAlert(message);

            function isOverlapIssue(errors) {
                return errors && errors.some(hasIntersectText)
                    ? 'Session cannot overlap with another scheduled session. Please change start time or duration.'
                    : null;
            }

            function hasIntersectText(msg) {
                return msg.indexOf('intersect in time') + 1;
            }
        }

        function calculateDate(dateHandleFunction) {            
            var date = new Date(vm.session.date);
            var time = Time.parseTimeString(vm.session.time);
            var localOffsetInHours = Date.getLocalTimezoneOffsetAsInteger();

            dateHandleFunction(date, time, localOffsetInHours);

            return date;
        }

        function prepareStartDateTimeBeforeSend() {
            return calculateDate(dateHandleFunction);

            function dateHandleFunction(date, time, localOffsetInHours) {
                date.setHours(time.hours + localOffsetInHours - parseInt(vm.step1.timezone.offset));
                date.setMinutes(time.minutes, 0, 0);
            }
        }

        function calculateStartDate() {
            return calculateDate(dateHandleFunction).toString();

            function dateHandleFunction(date, time, localOffsetInHours) {
                date.setHours(time.hours - localOffsetInHours + parseInt(vm.step1.timezone.offset), time.minutes, 0, 0);
            }
        }

        function calculateTypeName() {
            var type = practices.filter(function (item) {
                return item.id === vm.session.typeId;
            })[0];

            return type ? type.name : '';
        }

        function calculateSubTypeName() {
            var subType = drills.filter(function (item) {
                return item.drill.id === vm.session.subTypeId;
            })[0];

            return subType ? subType.drill.name : '';
        }

        function calculateLocationName(){
            var location = locations.filter(function (item) {
                return item.id === vm.session.location;
            })[0];

            return location ? location.name : '';
        }

        function calculateDayTypeName() {
            var dayType = options.SessionDayTypes.filter(function (item) {
                return item.Id === vm.session.dayTypeId;
            })[0];

            return dayType ? dayType.Name : '';
        }

        function calculateAlertAfterMinutes(session) {
            var questionnaire = session.Questionnaires[0];
            var alertTime = new Date(questionnaire.AlertDateTime);
            var startTime = new Date(questionnaire.StartDateTime);

            startTime.setMinutes(startTime.getMinutes() + questionnaire.DurationMinutes);

            return (alertTime - startTime) / 1000 / 60;
        }

        function initializeSession(session) {
            var result = {
                id: config.idForNewItems,
                durationMinutes: 30,
                coachRating: 7.5,
                startDateTime: null,
                sessionType: vm.step1.sessionTypeOptions[0].id,
                typeId: vm.step1.practiceOptions[0].id,
                subTypeId: null,
                location: vm.step1.sessionLocationOptions[0].id,
                alertAfterMinutes: 20,
                playerIds: [],
                randomizeQuestionOrder: false,
                isActive: true,
                time: '9.00 AM',
                date: new Date(),
                players: []
            };                


            if (session) {                                
                if (session.scheduled) {
                    result.date = new Date(session.scheduled);
                    result.time = session.time
                        ? Time.parseTimeString(session.time).toString()
                        : Time.getDateTime(session.scheduled).toString();
                }
                var getPracticeBySession = ResourceService.getPracticeBySessionId(session.id);

                //Assign Practice Data
                vm.originalPracticeId = parseInt(getPracticeBySession.id);

                if (vm.originalPracticeId > 0) {
                    result.typeId = vm.originalPracticeId;
                }
                if (session.sessionType) {
                    result.sessionType = parseInt(session.sessionType);
                }

                if (session.location && session.location.id) {
                    result.location = session.location.id;
                }

                if (session.SubType && session.SubType.Name
                    && UtilsService.hasCollectionId(options.SessionSubTypes, session.SubType.Id)) {
                    result.subTypeId = session.SubType.Id;
                }

                if (session.Squads && session.Squads.length > 0) {
                    result.squadIds = [];

                    session.Squads.forEach(function (item) {
                        result.squadIds.push(item.Id);
                    });
                }
                session.PlayerIds = [];
                if (session.users && session.users.length > 0) {
                    session.users.forEach(function (player) {
                        session.PlayerIds.push(player.id);
                    });
                }
                if (session.PlayerIds && session.PlayerIds.length > 0) {
                    result.playerIds = [];

                    session.PlayerIds.forEach(function (id) {
                        var player = options.Players.filter(filterById)[0];

                        player && result.playerIds.push(id);

                        function filterById(player) {
                            return player.id === id;
                        }
                    });

                    result.players = options.Players.filter(function (item) {
                        return result.playerIds.indexOf(item.id) + 1;
                    });
                }

                if (session.Questionnaires && session.Questionnaires.length > 0) {
                    result.alertAfterMinutes = calculateAlertAfterMinutes(session);
                }

                result.randomizeQuestionOrder = session.RandomizeQuestionOrder || result.randomizeQuestionOrder;
                result.id = session.id || result.id;
                result.durationMinutes = session.DurationMinutes || result.durationMinutes;
                result.coachRating = session.CoachRating || result.coachRating;

                // Remove already added players from available to select
                vm.step2.players = options.Players.filter(function (item) {
                    return result.players.indexOf(item) === -1;
                });
            }

            return result;
        }

        function initializePlayers(players) {
            var result = players || [];

            result = result.filter(activePlayerFilter);

            result.forEach(function (item) {
                item.PathToPhoto = item.pathtoPhoto || config.defaultUserPhoto;
            });

            return result;

            function activePlayerFilter(player) {
                return player.isActive;
            }
        }

        function addAllPlayersSquad(squads, players) {
            var playerRoleID = 2; // Player Role always has ID = 2.
            var activePlayers = players.filter(isActivePlayer);

            var playerIds = activePlayers.map(function (item) {
                return item.Id;
            });

            squads.unshift({
                Id: -1,
                Name: 'All Players',
                PlayerIds: playerIds
            });

            function isActivePlayer(item) {
                var playerRole = item.Roles.filter(function (role) {
                    return role.Id === playerRoleID;
                })[0];

                return item.Active && playerRole;
            }
        }

        function checkIfSessionAlreadyStarted(session) {
            if (!session) {
                return false;
            }

            var isNotNewSession = session.id !== config.idForNewItems;
            var isStarted = new Date(session.StartDateTime) < new Date();

            return isNotNewSession && isStarted;
        }

        function filterOutDisabledFields(session) {
            return {
                id: session.id,
                durationMinutes: session.durationMinutes,
                coachRating: session.coachRating
            };
        }

        function prepareSessionToSave(session) {
            var output = angular.copy(session);

            output.startDateTime = prepareStartDateTimeBeforeSend();            
            return output;
        }
    }

})();
