/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿// -----------------------------------------------------------------------
// <copyright file="loading-spinner.drv.js" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

(function () {
    'use strict';

    angular
        .module('app.directives')
        .directive('loadingSpinner', loadingSpinner);

    loadingSpinner.$inject = ['$rootScope'];

    function loadingSpinner($rootScope) {
        var directive = {
            restrict: 'E',
            scope: {
                startEvent: '@',
                endEvent: '@',
                saveHeight: '=',
                saveWidth: '='
            },
            link: link,
            templateUrl: '/Client/Scripts/Directives/Views/loading-spinner.html'
        };
        return directive;

        function link(scope, element, attrs) {
            var parent = element.parent()[0];
            var hiddenElements = [];
            var elem = element[0];

            var spinnerIsShown = false;

            if (scope.startEvent) {
                var startListener = $rootScope.$on(scope.startEvent, onStartEvent);
            }

            if (scope.endEvent) {
                var endListener = $rootScope.$on(scope.endEvent, onEndEvent);
            }

            scope.$on('$destroy', onDestroy);

            function onStartEvent() {
                if (spinnerIsShown) {
                    return;
                } else {
                    spinnerIsShown = true;
                }

                if (scope.saveHeight) {
                    elem.style.height = parent.clientHeight + 'px';
                }

                if (scope.saveWidth) {
                    elem.style.width = parent.clientWidth + 'px';
                }

                hideOtherItems();
                elem.style.display = 'block';
            }

            function onEndEvent() {
                if (!spinnerIsShown && scope.startEvent) {
                    return;
                } else {
                    spinnerIsShown = false;
                }

                elem.style.display = 'none';
                showOtherItems();
            }

            function onDestroy() {
                startListener && startListener();
                endListener && endListener();
            }

            function hideOtherItems() {
                [].forEach.call(parent.children, function (item) {
                    if (item !== elem) {
                        hiddenElements.push({
                            prevDisplayValue: item.style.display,
                            elem: item
                        });

                        item.style.display = 'none';
                    }
                });
            }

            function showOtherItems() {
                hiddenElements.forEach(function (item) {
                    item.elem.style.display = item.prevDisplayValue;
                });
            }
        }
    }

})();