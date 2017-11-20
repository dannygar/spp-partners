/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿// -----------------------------------------------------------------------
// <copyright file="timezone-picker.drv.js" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

(function () {
    'use strict';

    angular
        .module('app.directives')
        .directive('timezonePicker', timezonePicker);

    timezonePicker.$inject = ['$timeout'];

    function timezonePicker($timeout) {
        var directive = {
            restrict: 'E',
            scope: {
                value: '=',
                disabled: '='
            },
            link: link,
            templateUrl: '/Client/Scripts/Directives/Views/timezone-picker.html'
        };
        return directive;

        function link(scope, element, attrs) {
            var tzp = element.find('.timezone-picker');

            scope.timezoneOptions = getTimezoneOptions();

            if (scope.value && scope.value.name) {
                var selectedTimeZone = getTimeZoneByName(scope.value.name);

                if (selectedTimeZone) {
                    selectedTimeZone.selected = true;
                }

            } else {
                var clientTimeZone = tryToGuessClientTimeZone();

                if (clientTimeZone) {
                    clientTimeZone.selected = true;
                } else {
                    getTimeZoneByName('US/Pacific').selected = true; // PST time by default
                }
            }

            tzp.on('change', setValue);
            $timeout(setValue, 0);

            function setValue() {
                scope.value = getTimeZoneByName(tzp.val());
                scope.$apply();
            }

            function getTimezoneOptions() {
                var zones = [];

                moment.tz.names().forEach(addTimezone);

                return zones.sort(sortByOffset);

                function addTimezone(zoneName) {
                    var prefix = moment.tz(new Date(), zoneName).format('Z zz');
                    var offset = moment.tz.zone(zoneName).offset(new Date());

                    zones.push({
                        title: '({0}) {1}'.format(prefix, zoneName),
                        offset: -(offset / 60),
                        name: zoneName
                    });
                }

                function sortByOffset(item1, item2) {
                    if (item1.offset < item2.offset) {
                        return -1;
                    }
                    return item1.offset > item2.offset;
                }
            }

            function tryToGuessClientTimeZone() {
                var clientTimeZoneName = moment.tz.guess();
                return getTimeZoneByName(clientTimeZoneName);
            }

            function getTimeZoneByName(name) {
                var searchTimeZoneName = applyLinking(name);

                return scope.timezoneOptions.filter(filterByTimeZoneName)[0];

                function filterByTimeZoneName(zone) {
                    return zone.name == searchTimeZoneName;
                }

                function applyLinking(strName) {
                    switch(strName) {
                        case 'America/Los_Angeles':
                            return 'US/Pacific';
                        default:
                            return strName;
                    }
                }
            }
        }
    }

})();