/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿// -----------------------------------------------------------------------
// <copyright file="date-picker.drv.js" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

(function () {
    'use strict';

    angular
        .module('app.directives')
        .directive('datePicker', datePicker);

    datePicker.$inject = ['$timeout'];

    function datePicker($timeout) {
        var directive = {
            restrict: 'E',
            scope: {
                showIcon: '=',
                value: '=',
                onSelect: '=',
                disabled: '=',
                inputClass: '@',
                format: '@'
            },
            link: link,
            templateUrl: '/Client/Scripts/Directives/Views/date-picker.html'
        };
        return directive;

        function link(scope, element, attrs) {
            var callback = scope.onSelect
                ? scope.onSelect
                : scope.value ? setValue : null;

            var dp = element.find('.date-picker');
            dp.datepicker({
                showOn: scope.showIcon && !scope.disabled ? 'both' : 'focus',
                dateFormat: scope.format,
                onSelect: onSelect
            });
            dp.change(onChange);

            if (scope.showIcon) {
                dp.datepicker('option', {buttonImage: '/Client/CSS/images/calendar.png'});

                $timeout(function () {
                    dp.addClass('has-icon');
                }, 0);
            }

            // Set initial value
            dp.datepicker('setDate', scope.value || new Date);

            function onSelect(value) {
                if (callback) {
                    callback(value);
                    scope.$apply();
                }
            }

            function setValue(value) {
                scope.value = value;
            }

            function onChange(event) {
                onSelect(event.target.value);
            }
        }
    }

})();