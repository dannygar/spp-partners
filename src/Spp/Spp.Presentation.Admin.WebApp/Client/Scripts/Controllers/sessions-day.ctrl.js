/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
// -----------------------------------------------------------------------
// <copyright file="session-day.ctrl.js" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

(function () {
    'use strict';

    angular
        .module('app.controllers')
        .controller('SessionsDayCtrl', SessionsDayCtrl);

    SessionsDayCtrl.$inject = ['$rootScope', 'sessions', 'date', 'wellness', '$state', '$scope', 'config',
        'ResourceService', 'ValidationService'];

    function SessionsDayCtrl($rootScope, sessions, date, wellness, $state, $scope, config, ResourceService, ValidationService) {
        console.log("SessionsDayCtrl");

        var cannotSwitchWellnessMessage = 'Wellness has already started or finished. It cannot be modified.';

        var recalculateWellnessEvent = "recalculate-wellness";
        var showSpinnerEvent = 'showSpinner';
        var hideSpinnerEvent = 'hideSpinner';

        var itemToDelete;

        var vm = this;

        vm.sessions = sessions || [];

        vm.recalculateWellnessEvent = recalculateWellnessEvent;
        vm.morningSchedule = [];
        vm.afternoonSchedule = [];
        vm.selectedDate = new Date(date);
        vm.wellness = wellness[0];
        vm.wellnessTurned = vm.wellness;
        vm.isWellnessReminderDisabled = false;
        vm.wellnessReminderTime = config.wellnessReminder.defaultTime;

        vm.sessionContextMenuOptions = prepareSessionContextMenuOptions;
        vm.removeSession = removeSession;
        vm.openOnEdit = openOnEdit;
        vm.openWeekView = openWeekView;
        vm.turnWellness = turnWellness;
        vm.setWellnessAlert = setWellnessAlert;
        vm.getCurrentDate = getCurrentDate;
        vm.nextDay = nextDay;
        vm.previousDay = previousDay;

        //Initial values
        prepareSessionsView(sessions || []);
        vm.wellnessReminderTime = calculateWellnessReminderTime();

        $scope.filterSessionsByPlayer = function (val) {
            if (val === "") {
                return prepareSessionsView(vm.sessions || []);
            }
            return filterSessions($scope.players[val]);
        }

        return vm;

        function nextDay() {
            vm.selectedDate.setDate(vm.selectedDate.getDate() + 1);
            updateView(vm.selectedDate, showSpinnerEvent, hideSpinnerEvent, setDateBack);

            function setDateBack() {
                vm.selectedDate.setDate(vm.selectedDate.getDate() - 1);
            }
        }

        function previousDay() {
            vm.selectedDate.setDate(vm.selectedDate.getDate() - 1);
            updateView(vm.selectedDate, showSpinnerEvent, hideSpinnerEvent, setDateBack);

            function setDateBack() {
                vm.selectedDate.setDate(vm.selectedDate.getDate() + 1);
            }
        }

        function openOnEdit(item) {
            $state.go('app.session-detail', {id: item.id, session: item});
        }

        function openWeekView() {
            $state.go('app.sessions-week');
        }

        function prepareSessionsView(sessions, refreshPlayersScope = false) {
            if(!refreshPlayersScope){$scope.players = playersInDisplayedSessions(sessions);}
            var schedule = createScheduleView(sessions);

            vm.morningSchedule = schedule.filter(beforeNoon);
            vm.afternoonSchedule = schedule.filter(afterNoon);

            function beforeNoon(item) {
                return Time.compare(item.timeObj, Time.noon) == -1;
            }

            function afterNoon(item) {
                return Time.compare(item.timeObj, Time.noon) > -1;
            }
        }

        function createScheduleView(sessions) {
            var startTimeForView = Time.parseTimeString(config.startSessionTime).toMilliseconds();
            var endTimeForView = Time.parseTimeString(config.endSessionTime).toMilliseconds();

            var currentTime = 0;
            var dayEndTime = 24 * 60 * 60 * 1000;
            var defaultIntervalDuration = config.sessionView.intervalDuration * 60 * 1000;

            var schedule = [];
            var intervalExtraTime = 0;
            var lastSession = null;

            while (currentTime < dayEndTime) {
                var time = new Time(currentTime);

                var timeInterval = {
                    time: time.toString(),
                    timeObj: time,
                    duration: config.sessionView.intervalDuration - time.minutes + intervalExtraTime,
                    sessions: []
                };

                if (time.hours == Time.noon.hours && timeInterval.duration < config.sessionView.intervalDuration) {
                    var session = angular.copy(lastSession);

                    session.height = Math.abs(intervalExtraTime);
                    lastSession.height = lastSession.height - Math.abs(intervalExtraTime);
                    timeInterval.sessions.push(session);
                }

                intervalExtraTime = createSessionsForTimeInterval(timeInterval, sessions);

                if (timeInterval.sessions.length > 0) {
                    lastSession = timeInterval.sessions[timeInterval.sessions.length - 1];
                }

                schedule.push(timeInterval);
                currentTime = currentTime + defaultIntervalDuration;
            }

            schedule = schedule.filter(isValidInterval);

            return schedule;

            function isValidInterval(item) {
                var intervalTime = item.timeObj.toMilliseconds();
                var thereIsScheduledSessions = item.sessions.some(function (session) {
                    return session.id !== config.idForNewItems;
                });

                return thereIsScheduledSessions
                    || (intervalTime >= startTimeForView && intervalTime <= endTimeForView)
                    || item.duration < config.sessionView.intervalDuration;
            }
        }

        function createSessionsForTimeInterval(timeInterval, sessions) {
            var output = timeInterval.sessions;

            var intervalEndTime = new Time(timeInterval.timeObj);
            intervalEndTime.addMinutes(config.sessionView.intervalDuration);

            var freeIntervalTime = timeInterval.duration;

            var currentTime = new Time(timeInterval.timeObj);
            currentTime.addMinutes(config.sessionView.intervalDuration - freeIntervalTime);

            var sessionsInInterval = sessions
                .filter(filterByHour)
                .sort(compareByTime);

            for (var i = 0; i < sessionsInInterval.length; i++) {
                if (currentTime.minutes < sessionsInInterval[i].time.minutes) {
                    var durationTime = new Time(Time.timeDifference(sessionsInInterval[i].time, currentTime));
                    var fakeSessionDuration = (durationTime.hours * 60) + durationTime.minutes;

                    output.push(createFakeSession(currentTime, fakeSessionDuration));
                }

                var session = angular.copy(sessionsInInterval[i]);

                session.time = sessionsInInterval[i].time.toString();
                session.height = 30; //default duration of 30

                output.push(session);

                currentTime = new Time(sessionsInInterval[i].time);
                currentTime.addMinutes(30); //default duration of 30

                var subtraction = new Time(Time.timeDifference(intervalEndTime, currentTime));
                freeIntervalTime = (subtraction.hours * 60) + subtraction.minutes;
            }

            while (freeIntervalTime > 0) {
                var prevFreeIntervalTime = freeIntervalTime;

                freeIntervalTime = freeIntervalTime == config.sessionView.intervalDuration
                    ? freeIntervalTime / 2
                    : 0;

                var sessionDuration = prevFreeIntervalTime - freeIntervalTime;

                output.push(createFakeSession(currentTime, sessionDuration));
                currentTime.addMinutes(sessionDuration);
            }

            timeInterval.sessions = output;

            return freeIntervalTime;

            function filterByHour(item) {
                item.time = Time.getDateTime(item.scheduled);
                return timeInterval.timeObj.hours == item.time.hours;
            }
        }

        function createFakeSession(time, duration) {
            return {
                id: config.idForNewItems,
                scheduled: vm.selectedDate,
                time: time.toString(),
                height: duration
            }
        }

        function prepareSessionContextMenuOptions(item) {
            if (item.id === config.idForNewItems) {
                return [];
            }

            return [
                ["Delete", function () {
                    $('#deleteModal').modal('show');
                    itemToDelete = item;
                }]
            ];
        }

        function removeSession(modal) {
            var message = ValidationService.validateSessionRemoval(itemToDelete);
            if (message) {
                $rootScope.$emit('showPopoverInDeleteModal', {
                    content: message,
                    time: config.errorPopoverTime
                });
                return;
            }

            $rootScope.$emit('showSpinnerInDeleteModal');

            ResourceService.deleteSession(itemToDelete.id)
                .then(successCallback, errorCallback);

            function successCallback() {
                $rootScope.$emit('hideSpinnerInDeleteModal');
                modal.modal('hide');

                updateView(vm.selectedDate, showSpinnerEvent, hideSpinnerEvent);
            }

            function errorCallback() {
                $rootScope.$emit('hideSpinnerInDeleteModal');
                $rootScope.$emit('showPopoverInDeleteModal', {
                    content: config.defaultErrorMessage,
                    time: config.errorPopoverTime
                });
            }
        }

        function calculateWellnessReminderTime() {
            var scheduledSessions = vm.sessions.filter(isRealSession);

            if (scheduledSessions.length > 0) {
                scheduledSessions.sort(compareByTime);

                var earliestSession = scheduledSessions[0];

                var dateTime = new Date(earliestSession.StartDateTime);
                dateTime.setMinutes(dateTime.getMinutes() - config.wellnessReminder.remindBeforeFirstSession);

                vm.isWellnessReminderDisabled = true;

                return Time.getDateTime(dateTime);
            }

            vm.isWellnessReminderDisabled = !vm.wellnessTurned || new Date().onTheSameDay(vm.selectedDate);

            return vm.wellness
                ? Time.getDateTime(vm.wellness.AlertDateTime, true)
                : config.wellnessReminder.defaultTime;

            function isRealSession(item) {
                return item.id !== config.idForNewItems;
            }
        }

        function updateView(date, showSpinnerEvent, hideSpinnerEvent, additionalErrorCallback) {
            $rootScope.$emit(showSpinnerEvent);
            ResourceService.getAllSessions(date)
                .then(updateViewSuccessCallback, errorCallback);

            function updateViewSuccessCallback(result) {
                    $rootScope.$emit(hideSpinnerEvent);
                    vm.sessions = result || [];

                    prepareSessionsView(result || []);

                    $scope.$broadcast(recalculateWellnessEvent, wellness);
            }

            function errorCallback() {
                additionalErrorCallback && additionalErrorCallback();

                $rootScope.$emit(hideSpinnerEvent);
                $rootScope.showAlert();
            }
        }

        function turnWellness() {
            var data = {
                "date": new Date(vm.selectedDate).toDateString(),
                "enable": vm.wellnessTurned,
                "timeZoneOffset": Date.getLocalTimezoneOffset()
            };

            if (!checkWellnessAlertDate(data.date)) {
                vm.wellnessTurned = !vm.wellnessTurned;
                $rootScope.showAlert(cannotSwitchWellnessMessage);
                return;
            }

            ResourceService.switchWellness(data)
                .then(switchWellnessSuccessCallback, switchWellnessErrorCallback);

            function switchWellnessSuccessCallback(result) {
                vm.wellness = result;
                calculateWellnessReminderTime();
            }

            function switchWellnessErrorCallback() {
                vm.wellnessTurned = !vm.wellnessTurned;
                $rootScope.showAlert();
            }
        }

        function setWellnessAlert() {
            if (vm.isWellnessReminderDisabled || !vm.wellness
                || !checkWellnessAlertDate(vm.wellness.StartDateTime)) {
                vm.isWellnessReminderDisabled = true;
                return;
            }

            var alertTime = Time.parseTimeString(vm.wellnessReminderTime);
            var alertDateTime = new Date(vm.selectedDate);

            alertDateTime.setHours(alertTime.hours, alertTime.minutes, 0, 0);

            var data = {
                AlertTime: Time.getDateTime(alertDateTime).to24FormatString()
            };

            ResourceService.updateQuestionnaire(vm.wellness.Id, data)
                .then(null, errorCallback);

            function errorCallback() {
                $rootScope.showAlert();
            }
        }

        function checkWellnessAlertDate(startDateTimeString) {
            var today = new Date();
            var wellnessStartDateTime = new Date(startDateTimeString);
            wellnessStartDateTime.setHours(wellnessStartDateTime.getHours() - Date.getLocalTimezoneOffsetAsInteger());

            return today.getTime() < wellnessStartDateTime.getTime();
        }

        function getCurrentDate() {
            var dayOfWeek = vm.selectedDate.getDayName();
            var month = vm.selectedDate.getMonth() + 1; // starts with 0
            var day = vm.selectedDate.getDate();
            var year = vm.selectedDate.getFullYear();

            return dayOfWeek + ", " + month + "/" + day + "/" + year;
        }

        function compareByTime(a, b) {
            return Time.compare(a.time, b.time);
        }

        function playersInDisplayedSessions(sessions) {
            if (sessions.length === 0) {
                return [];
            }
            var playersInSession = [];

            sessions.forEach(function (session) {
                var players = session.users;
                if (players.length === 0) {
                    return;
                }
                var i;
                for (i = 0; i < players.length; i++) {
                    if (isPlayerInArray(players[i], playersInSession)) {
                        var j;
                        for (j = 0; j < playersInSession.length; j++) {
                            if (players[i].id === playersInSession[j].id) {
                                playersInSession[j].sessionIds.push(session.id);

                            }
                        }
                    } else {
                        playersInSession.push(players[i]);
                        players[i].sessionIds = [];
                        players[i].sessionIds.push(session.id);
                    }
                }
            });

            return playersInSession;
        }

        function filterSessions(player) {
            var sessionsFiltered = [];
            var playerSessions = player.sessionIds;
            vm.sessions.forEach(function(session) {
                var i;
                for (i = 0; i < playerSessions.length; i++) {
                    if (session.id === player.sessionIds[i]) {
                        sessionsFiltered.push(session);
                    }
                }
            });
            prepareSessionsView(sessionsFiltered, true);
        }

        function isPlayerInArray(player, playersInSession) {
            for (var i = 0; i < playersInSession.length; i++) {
                if (playersInSession[i].id === player.id) {
                    return true; 
                } else {
                    continue;
                }
            }
            return false;
        }
    }

})();
