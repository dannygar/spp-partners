/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿// -----------------------------------------------------------------------
// <copyright file="switcher.drv.js" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

(function () {
    'use strict';

    angular
        .module('app.directives')
        .directive('switcher', switcher);

    switcher.$inject = ['UtilsService'];

    function switcher(UtilsService) {
        var directive = {
            restrict: 'E',
            scope: {
                value: '=',
                onSwitch: '=',
                toggledByDefault: '=',
                initialText: '@',
                toggledText: '@'
            },
            link: link,
            templateUrl: '/Client/Scripts/Directives/Views/switcher.html'
        };
        return directive;

        function link(scope, element, attrs) {
            var toggledClass = 'toggled';
            var input = element.find('input[type="number"]');
            var switcher = element.find('.switcher');

            // Initial values
            initialize();

            function initialize() {
                scope.isToggled = !!scope.toggledByDefault;

                switcher.attr('initial-text', scope.initialText || 'ON');
                switcher.attr('toggled-text', scope.toggledText || 'OFF');

                input.on('change', onInputChange);
                switcher.on('click', onSwitcherClick);

                switcher.toggleClass(toggledClass, scope.isToggled);

                scope.shownValue = scope.isToggled
                    ? Math.round(scope.onSwitch(scope.isToggled, scope.value))
                    : Math.round(scope.value);
            }

            function onInputChange() {
                scope.shownValue = Math.round(scope.shownValue);
                scope.value = (scope.isToggled && scope.onSwitch)
                    ? scope.onSwitch(false, scope.shownValue)
                    : scope.shownValue;

                scope.$parent.$digest();
            }

            function onSwitcherClick(event) {
                UtilsService.stopEventPropagation(event);

                toggle();

                scope.shownValue = scope.isToggled
                    ? Math.round(scope.onSwitch(scope.isToggled, scope.value))
                    : Math.round(scope.value);

                scope.$parent.$digest();
            }

            function toggle() {
                scope.isToggled = !scope.isToggled;
                switcher.toggleClass(toggledClass, scope.isToggled)
            }


        }
    }

})();