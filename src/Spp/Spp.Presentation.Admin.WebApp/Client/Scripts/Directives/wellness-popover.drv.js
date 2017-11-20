/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿// -----------------------------------------------------------------------
// <copyright file="wellness-popover.drv.js" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

(function () {
    'use strict';

    angular
        .module('app.directives')
        .directive('wellnessPopover', wellnessPopover);

    wellnessPopover.$inject = ['$rootScope', 'UtilsService', 'ResourceService', 'config'];

    function wellnessPopover($rootScope, UtilsService, ResourceService, config) {
        var directive = {
            restrict: 'E',
            scope: {
                day: '='
            },
            link: {
                pre: preLink,
                post: postLink
            },
            templateUrl: '/Client/Scripts/Directives/Views/wellness-popover.html'
        };
        return directive;

        function preLink(scope, element, attrs) {
            var cannotSwitchWellnessMessage = 'Wellness has already started or finished. It cannot be modified.';

            scope.wellnessTurned = !!scope.day.wellness;
            scope.turnWellness = turnWellness;

            scope.isWellnessReminderDisabled = false;
            scope.wellnessReminderTime = calculateWellnessReminderTime();

            scope.setWellnessAlert = setWellnessAlert;

            function turnWellness() {
                var data = {
                    "date": scope.day.date.toDateString(),
                    "enable": scope.wellnessTurned,
                    "timeZoneOffset": Date.getLocalTimezoneOffset()
                };

                if (!checkWellnessAlertDate(data.date)) {
                    scope.wellnessTurned = !scope.wellnessTurned;
                    showAlertAndHidePopovers(cannotSwitchWellnessMessage);
                    return;
                }

                ResourceService.switchWellness(data)
                    .then(switchWellnessSuccessCallback, switchWellnessErrorCallback);

                function switchWellnessSuccessCallback(result) {
                    scope.day.wellness = result;
                    calculateWellnessReminderTime();
                }

                function switchWellnessErrorCallback() {
                    scope.wellnessTurned = !scope.wellnessTurned;
                    showAlertAndHidePopovers();
                }
            }

            function setWellnessAlert() {
                if (scope.isWellnessReminderDisabled || !scope.day.wellness
                    || !checkWellnessAlertDate(scope.day.wellness.StartDateTime)) {
                    scope.isWellnessReminderDisabled = true;
                    return;
                }

                var alertTime = Time.parseTimeString(scope.wellnessReminderTime);
                var alertDateTime = new Date(scope.day.date);

                alertDateTime.setHours(alertTime.hours, alertTime.minutes, 0, 0);

                ResourceService.updateQuestionnaire(scope.day.wellness.Id,
                    {
                        AlertTime: Time.getDateTime(alertDateTime).to24FormatString()
                    }
                ).then(null, updateQuestionnaireErrorCallback);

                function updateQuestionnaireErrorCallback() {
                    showAlertAndHidePopovers();
                }
            }

            function calculateWellnessReminderTime () {
                var scheduledSessions = scope.day.scheduledSessions;

                if (scheduledSessions.length > 0) {
                    scheduledSessions.sort(compareByTime);

                    var earliestSession = scheduledSessions[0];

                    var dateTime = new Date(earliestSession.StartDateTime);
                    dateTime.setMinutes(dateTime.getMinutes() - config.wellnessReminder.remindBeforeFirstSession);

                    // Disable manual changing value of time-picker
                    scope.isWellnessReminderDisabled = true;

                    return Time.getDateTime(dateTime);
                }

                // Enable manual changing value of time-picker
                scope.isWellnessReminderDisabled = !scope.wellnessTurned
                    || !checkWellnessAlertDate(scope.day.wellness.StartDateTime)
                    || new Date().onTheSameDay(scope.day.date);

                return scope.day.wellness
                    ? Time.getDateTime(scope.day.wellness.AlertDateTime, true)
                    : config.wellnessReminder.defaultTime;

                function compareByTime(a, b) {
                    return Time.compare(a.time, b.time);
                }
            }

            function checkWellnessAlertDate(startDateTimeString) {
                var today = new Date();
                var wellnessStartDateTime = new Date(startDateTimeString);
                wellnessStartDateTime.setHours(wellnessStartDateTime.getHours() - Date.getLocalTimezoneOffsetAsInteger());

                return today.getTime() < wellnessStartDateTime.getTime();
            }

            function showAlertAndHidePopovers(message) {
                $rootScope.showAlert(message);

                var popover = $('.wellness-popover');
                popover.addClass('hidden');
            }
        }

        function postLink(scope, element, attrs) {
            var isHidden = true;
            var popover = element.find('.wellness-popover');

            scope.triggerPopover = triggerPopover;

            element.on('click', scope.triggerPopover);
            popover.on('click', UtilsService.stopEventPropagation);

            function triggerPopover() {
                popover.toggleClass('hidden');
                isHidden = !isHidden;
            }
        }
    }

})();