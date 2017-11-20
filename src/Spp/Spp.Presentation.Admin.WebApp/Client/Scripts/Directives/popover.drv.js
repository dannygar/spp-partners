/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿// -----------------------------------------------------------------------
// <copyright file="popover.drv.js" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

(function () {
    'use strict';

    angular
        .module('app.directives')
        .directive('popover', popover);

    popover.$inject = ['$rootScope', 'config'];

    function popover($rootScope, config) {
        var directive = {
            restrict: 'A',
            scope: {
                startEvent: '@',
                endEvent: '@'
            },
            link: link
        };
        return directive;

        function link(scope, element, attrs) {
            if (scope.startEvent) {
                var showListener = $rootScope.$on(scope.startEvent, showPopover);
            }

            if (scope.endEvent) {
                var hideListener = $rootScope.$on(scope.endEvent, hidePopover);
            }

            element.on('$destroy', onDestroy);

            function showPopover(event, data) {
                var popover = element.find('.notification-popover');

                if (popover.length > 0) {
                    return;
                }

                var content = data && data.content
                    ? data.content
                    : config.defaultErrorMessage;

                var htmlTemplate = '<span class="notification-popover">' + content + '</span>';

                element.addClass('notification-popover-container');
                element.append(htmlTemplate);

                centerPopover();

                if (data && data.time) {
                    setTimeout(hidePopover, data.time);
                }
            }

            function hidePopover() {
                element.find('.notification-popover').remove();
                element.removeClass('notification-popover-container');
            }

            function centerPopover() {
                var popover = element.find('.notification-popover');
                var popoverShift = parseInt(popover.css('width')) / 2 - parseInt(element.css('width')) / 2;

                popover.css('left', -Math.round(popoverShift) + 'px');
            }

            function onDestroy() {
                showListener && showListener();
                hideListener && hideListener();
            }
        }
    }

})();