/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
// -----------------------------------------------------------------------
// <copyright file="session-week.ctrl.js" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

(function () {
    'use strict';

    angular
        .module('app.controllers')
        .controller('SessionsWeekCtrl', SessionsWeekCtrl);

    SessionsWeekCtrl.$inject = ['$rootScope', 'sessions', 'selectedPeriod', 'wellness', '$state', 'config', 'ResourceService', 'UtilsService', 'ValidationService', '$scope'];

    function SessionsWeekCtrl($rootScope, sessions, selectedPeriod, wellness, $state, config, ResourceService, UtilsService, ValidationService, $scope) {
        console.log("SessionsWeekCtrl");

        var mainDayNumber = 6; // last day of week
        var changeMonthStartEvent = "changeMonthStart";
        var changeMonthEndEvent = "changeMonthEnd";
        var changeWeekStartEvent = "changeWeekStart";
        var changeWeekEndEvent = "changeWeekEnd";

        var itemToDelete;

        var vm = this;
        vm.sessions = sessions || [];
        vm.selectedPeriod = selectedPeriod;
        vm.selectedDate = vm.selectedPeriod[mainDayNumber].date;

        vm.openOnEdit = openOnEdit;
        vm.openDayView = openDayView;
        vm.sessionContextMenuOptions = prepareSessionContextMenuOptions;
        vm.removeSession = removeSession;
        vm.getCurrentDate = getCurrentDate;
        vm.nextMonth = nextMonth;
        vm.previousMonth = previousMonth;
        vm.nextWeek = nextWeek;
        vm.previousWeek = previousWeek;
        vm.filterSessions = filterSessions;

        //Initial values
        prepareWeekScheduleView(sessions || [], selectedPeriod[mainDayNumber].date, wellness);

        $scope.filterSessionsByPlayer = function (val) {
            if (val === "") {
                return prepareWeekScheduleView(vm.sessions || [], vm.selectedPeriod[mainDayNumber].date, wellness);
            }
            return filterSessions($scope.players[val]);
        }

        return vm;

        function nextMonth() {
            var targetDate = new Date(vm.selectedDate);
            targetDate.setMonth(targetDate.getMonth() + 1, 1);

            updateView(targetDate, changeMonthStartEvent, changeMonthEndEvent);
        }

        function previousMonth() {
            var targetDate = new Date(vm.selectedDate);
            targetDate.setMonth(targetDate.getMonth() - 1, 1);

            updateView(targetDate, changeMonthStartEvent, changeMonthEndEvent);
        }

        function nextWeek() {
            var nextWeekDate = vm.selectedPeriod[vm.selectedPeriod.length - 1].date.nextDate();
            updateView(nextWeekDate, changeWeekStartEvent, changeWeekEndEvent);
        }

        function previousWeek() {
            var previousWeekDate = vm.selectedPeriod[0].date.previousDate();
            updateView(previousWeekDate, changeWeekStartEvent, changeWeekEndEvent);
        }

        function openOnEdit(item) {
            $state.go('app.session-detail', {id: item.id, session: item});
        }

        function openDayView() {
            $state.go('app.sessions-day');
        }

        function prepareWeekScheduleView(sessions, date, wellnessArray, refreshPlayersScope = false) {
            if(!refreshPlayersScope){$scope.players = playersInDisplayedSessions(sessions);}
            vm.selectedPeriod = UtilsService.getWeekForDate(date);
            vm.selectedDate = vm.selectedPeriod[mainDayNumber].date;

            vm.selectedPeriod.scheduledTime = calculateScheduledTimeForPeriod(sessions);
            vm.selectedPeriod.forEach(prepareDay);

            function prepareDay(day) {
                day.scheduledSessions = sessions.filter(onTheSameDay);

                var selectedPeriodLastIndex = vm.selectedPeriod.scheduledTime.length - 1;
                day.sessions = createScheduleView(day, getScheduledTimeValue(0), getScheduledTimeValue(selectedPeriodLastIndex));

                day.wellness = wellnessArray.filter(onTheSameDayIgnoringTimezone)[0];

                function onTheSameDay(item) {
                    return day.date.onTheSameDay(item.scheduled);
                }

                function onTheSameDayIgnoringTimezone(item) {
                    var wellnessStartDateTime = new Date(item.StartDateTime);
                    wellnessStartDateTime.setHours(wellnessStartDateTime.getHours() - Date.getLocalTimezoneOffsetAsInteger());

                    return day.date.onTheSameDay(wellnessStartDateTime);
                }

                function getScheduledTimeValue(index) {
                    return vm.selectedPeriod.scheduledTime[index].value;
                }
            }
        }

        function createScheduleView(day, startTime, endTime) {
            var startTimeForView = startTime.toMilliseconds();
            var endTimeForView = endTime.toMilliseconds();

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

                intervalExtraTime = createSessionsForTimeInterval(timeInterval, day);

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

        function createSessionsForTimeInterval(timeInterval, day) {
            var output = timeInterval.sessions;

            var intervalEndTime = new Time(timeInterval.timeObj);
            intervalEndTime.addMinutes(config.sessionView.intervalDuration);

            var freeIntervalTime = timeInterval.duration;

            var currentTime = new Time(timeInterval.timeObj);
            currentTime.addMinutes(config.sessionView.intervalDuration - freeIntervalTime);

            var sessionsInInterval = day.scheduledSessions
                .filter(filterByHour)
                .sort(compareByTime);

            for (var i = 0; i < sessionsInInterval.length; i++) {
                if (currentTime.minutes < sessionsInInterval[i].time.minutes) {
                    var durationTime = new Time(Time.timeDifference(sessionsInInterval[i].time, currentTime));
                    var fakeSessionDuration = (durationTime.hours * 60) + durationTime.minutes;

                    output.push(createFakeSession(day, currentTime, fakeSessionDuration));
                }

                var session = angular.copy(sessionsInInterval[i]);

                session.time = sessionsInInterval[i].time.toString();
                session.height = 30; // default duration

                output.push(session);

                currentTime = new Time(sessionsInInterval[i].time);
                currentTime.addMinutes(30); // default duration

                var subtraction = new Time(Time.timeDifference(intervalEndTime, currentTime));
                freeIntervalTime = (subtraction.hours * 60) + subtraction.minutes;
            }

            while (freeIntervalTime > 0) {
                var prevFreeIntervalTime = freeIntervalTime;
                freeIntervalTime = freeIntervalTime == config.sessionView.intervalDuration
                    ? freeIntervalTime / 2
                    : 0;

                var sessionDuration = prevFreeIntervalTime - freeIntervalTime;

                output.push(createFakeSession(day, currentTime, sessionDuration));
                currentTime.addMinutes(sessionDuration);
            }

            timeInterval.sessions = output;

            return freeIntervalTime;

            function filterByHour(item) {
                item.time = Time.getDateTime(item.scheduled);
                return timeInterval.timeObj.hours == item.time.hours;
            }

            function compareByTime(a, b) {
                return Time.compare(a.time, b.time);
            }
        }

        function createFakeSession(day, time, duration) {
            return {
                id: config.idForNewItems,
                scheduled: day.date,
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

                updateView(vm.selectedDate, changeWeekStartEvent, changeWeekEndEvent);
            }

            function errorCallback() {
                $rootScope.$emit('hideSpinnerInDeleteModal');
                $rootScope.$emit('showPopoverInDeleteModal', {
                    content: config.defaultErrorMessage,
                    time: config.errorPopoverTime
                });
            }
        }

        function calculateScheduledTimeForPeriod(sessions) {
            var startTimeForView = Time.parseTimeString(config.startSessionTime);
            var endTimeForView = Time.parseTimeString(config.endSessionTime);
            var output = [];

            sessions.forEach(function (session) {
                var time = Time.getDateTime(session.scheduled);
                var endTime = new Time(time).addMinutes(30); // default duration

                if (Time.compare(time, startTimeForView) === -1) {
                    startTimeForView = new Time(time);
                }

                if (Time.compare(endTime, endTimeForView) === 1) {
                    endTimeForView = new Time(endTime);
                }
            });

            var i = startTimeForView.hours;
            for (i; i <= endTimeForView.hours; i++) {
                var scheduledTime = new Time(i, 0);

                output.push({
                    value: scheduledTime,
                    name: scheduledTime.toString()
                });
            }

            output = output.sort(compareByValue);

            return output;

            function compareByValue(a, b) {
                return Time.compare(a.value, b.value);
            }
        }

        function updateView(date, startSpinnerEvent, endSpinnerEvent) {
            $rootScope.$emit(startSpinnerEvent);

            var requestPeriod = UtilsService.getWeekForDate(date);

            ResourceService.getAllSessions(requestPeriod[0].date, requestPeriod[requestPeriod.length - 1].date.nextDate())
                .then(updateViewSuccessCallback, errorCallback);

            function updateViewSuccessCallback(result) {
                $rootScope.$emit(endSpinnerEvent);
                vm.sessions = result || [];
                prepareWeekScheduleView(result || [], date, wellness);
            }

            function errorCallback() {
                $rootScope.$emit(endSpinnerEvent);
                $rootScope.showAlert();
            }
        }

        function getCurrentDate() {
            return vm.selectedDate.getMonthName() + ", " + vm.selectedDate.getFullYear();
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
            vm.sessions.forEach(function (session) {
                var i;
                for (i = 0; i < playerSessions.length; i++) {
                    if (session.id === player.sessionIds[i]) {
                        sessionsFiltered.push(session);
                    }
                }
            });
            prepareWeekScheduleView(sessionsFiltered, vm.selectedPeriod[mainDayNumber].date, wellness, true);
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