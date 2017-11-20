/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿// -----------------------------------------------------------------------
// <copyright file="composite-switcher.drv.js" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

(function () {
    'use strict';

    angular
        .module('app.directives')
        .directive('compositeSwitcher', compositeSwitcher);

    compositeSwitcher.$inject = ['UtilsService'];

    function compositeSwitcher(UtilsService) {
        var directive = {
            restrict: 'E',
            scope: {
                value: '=',
                onSwitch: '=',
                toggledByDefault: '=',
                initialText: '@',
                toggledText: '@',
                factor: '@',
                firstCompositeMeasureName: '@',
                secondCompositeMeasureName: '@'
            },
            link: link,
            templateUrl: '/Client/Scripts/Directives/Views/composite-switcher.html'
        };
        return directive;

        function link(scope, element, attrs) {
            var toggledClass = 'toggled';
            var firstInput = element.find('.first-value');
            var secondInput = element.find('.second-value');
            var singleInput = element.find('.single-value'); // Other measure
            var switcher = element.find('.switcher');

            initialize();

            function initialize() {
                scope.isToggled = !!scope.toggledByDefault;
                scope.compositeValue = toComposite(scope.value, scope.factor);

                firstInput.on('change', changeValue);
                secondInput.on('change', changeValue);
                singleInput.on('change', changeValue);

                switcher.attr('initial-text', scope.initialText || 'ON');
                switcher.attr('toggled-text', scope.toggledText || 'OFF');

                switcher.on('click', onCompositeSwitcherClick);

                switcher.toggleClass(toggledClass, scope.isToggled);

                if (scope.isToggled && scope.onSwitch) {
                    scope.singleValue = Math.round(scope.onSwitch(scope.isToggled, scope.value));
                } else {
                    scope.compositeValue = toComposite(scope.value, scope.factor);
                }
            }

            function onCompositeSwitcherClick(event) {
                UtilsService.stopEventPropagation(event);
                toggle();

                if (scope.isToggled && scope.onSwitch) {
                    scope.singleValue = Math.round(scope.onSwitch(scope.isToggled, scope.value));
                } else {
                    scope.compositeValue = toComposite(scope.value, scope.factor);
                }

                scope.$parent.$digest();
            }

            function changeValue() {
                if (scope.isToggled && scope.onSwitch) {
                    scope.singleValue = Math.round(scope.singleValue);
                    scope.value = scope.onSwitch(false, scope.singleValue);
                } else {
                    scope.compositeValue.first = Math.round(scope.compositeValue.first);
                    scope.compositeValue.second = Math.round(scope.compositeValue.second);
                    scope.value = toSimple(scope.compositeValue, scope.factor);
                }

                scope.$parent.$digest();
            }

            function toggle() {
                scope.isToggled = !scope.isToggled;
                switcher.toggleClass(toggledClass, scope.isToggled);
            }

            function toComposite(value, factor) {
                return {
                    first: Math.floor(value / factor),
                    second: Math.round(value % factor)
                };
            }

            function toSimple(value, factor) {
                return value.first * factor + value.second;
            }
        }
    }

})();