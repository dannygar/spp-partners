/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿// -----------------------------------------------------------------------
// <copyright file="on-off-slider.drv.js" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

(function () {
    'use strict';

    angular
        .module('app.directives')
        .directive('onOffSlider', onOffSlider);

    onOffSlider.$inject = ['UtilsService', 'DebounceService'];

    function onOffSlider(UtilsService, DebounceService) {
        var directive = {
            restrict: 'E',
            scope: {
                value: '=',
                onChange: '=',
                extraArgs: '=',
                debounceTime: '=',
                checkedText: '@',
                uncheckedText: '@',
                cacheAs: '@'
            },
            link: link,
            templateUrl: '/Client/Scripts/Directives/Views/on-off-slider.html'
        };
        return directive;

        function link(scope, element, attrs) {
            var toggledClass = 'toggled';
            var debounceInstance = DebounceService.getInstance();

            // Initial values
            var isChecked = getValue();
            element.toggleClass(toggledClass, isChecked);

            attrs.$set('checked-text', scope.checkedText || 'ON');
            attrs.$set('unchecked-text', scope.uncheckedText || 'OFF');

            element.on('click', onSliderClick);

            scope.$watch('value', onChangedValue);

            function onSliderClick(event) {
                UtilsService.stopEventPropagation(event);

                toggle();
                setValue(isChecked);

                scope.$parent.$digest();

                if (scope.onChange) {
                    debounceInstance.debounce(
                        scope.onChange,
                        scope.debounceTime,
                        scope.extraArgs,
                        !scope.debounceTime
                    );
                }
            }

            function onChangedValue(value) {
                isChecked = !!value;
                element.toggleClass(toggledClass, isChecked);
            }

            function toggle() {
                isChecked = !isChecked;
                element.toggleClass(toggledClass, isChecked)
            }

            function setValue(value) {
                scope.value = value;

                if (scope.cacheAs) {
                    localStorage.setItem(scope.cacheAs, scope.value);
                }
            }

            function getValue() {
                if (scope.cacheAs && localStorage.getItem(scope.cacheAs)) {
                    scope.value = localStorage.getItem(scope.cacheAs) === 'true';
                }

                return !!scope.value;
            }

        }
    }

})();