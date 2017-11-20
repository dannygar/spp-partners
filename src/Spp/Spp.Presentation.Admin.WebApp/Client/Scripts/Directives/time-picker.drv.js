/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿// -----------------------------------------------------------------------
// <copyright file="time-picker.drv.js" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

(function () {
    'use strict';

    angular
        .module('app.directives')
        .directive('timePicker', timePicker);

    timePicker.$inject = ['$timeout', 'DebounceService'];

    function timePicker($timeout, DebounceService) {
        var directive = {
            restrict: 'E',
            scope: {
                value: '=',
                disabled: '=',
                onChangeEvent: '=',
                onChange: '=',
                extraArgs: '=',
                debounceTime: '='
            },
            link: link,
            templateUrl: '/Client/Scripts/Directives/Views/time-picker.html'
        };
        return directive;

        function link(scope, element, attrs) {
            var tp = element.find('.time-picker').timespinner();
            var postfixSelector = element.find('.time-postfix');
            var debounceInstance = DebounceService.getInstance();
            var onChange = null;

            scope.timePostfixOptions = [{name: 'AM'}, {name: 'PM'}];

            if (scope.value) {
                var initialValue = scope.value.toString() || '6:00 AM';
                var time = Time.parseTimeString(initialValue);
                setValue(time);

                onChange = function (e) {
                    scope.value = getValue();
                    $timeout(function () {
                        scope.$apply();

                        if (scope.onChange) {
                            debounceInstance.debounce(scope.onChange, scope.debounceTime,
                                scope.extraArgs, !!scope.debounceTime);
                        }
                    }, 0);
                }
            }

            scope.onChangeEvent && scope.$on(scope.onChangeEvent, onChangeEventHandler);

            tp.timespinner({change: onChange});
            postfixSelector.on('change', onChange);

            scope.$watch('disabled', function (value) {
                tp.timespinner(value ? 'disable' : 'enable');
                postfixSelector.prop("disabled", !!value);
            });

            function getValue() {
                var value = Globalize.format(new Date(tp.timespinner('value')), "t");
                return value.slice(0, -2) + scope.timePostfix.name;
            }

            function setValue(time) {
                var timeStr = time.toString();
                var postfix = timeStr.slice(-2);
                var postfixValue = scope.timePostfixOptions.filter(function (item) {
                    return item.name === postfix;
                })[0];

                scope.timePostfix = postfixValue || scope.timePostfixOptions[0];
                tp.timespinner('value', timeStr);
            }

            function onChangeEventHandler(e, data) {
                setValue(data);
            }
        }
    }

    $.widget("ui.timespinner", $.ui.spinner, {
        options: {
            step: 60 * 1000, // 1 minute
            page: 60 // 1 hour
        },
        _parse: function (value) {
            if (typeof value === "string") {
                // already a timestamp
                if (Number(value) == value) {
                    return Number(value);
                }
                // add postfix for correct parsing to timestamp. Then this postfix will be sliced.
                if (value.indexOf('AM') + value.indexOf('PM') < -1) {
                    value = value + " AM";
                }

                value = Time.parseTimeString(value).toString();

                return +Globalize.parseDate(value);
            }
            return value;
        },
        _format: function (value) {
            return Globalize.format(new Date(value), "t").slice(0, -3);
        }
    });

})();