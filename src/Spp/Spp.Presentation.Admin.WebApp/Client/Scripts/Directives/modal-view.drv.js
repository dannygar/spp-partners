/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿// -----------------------------------------------------------------------
// <copyright file="modal-view.drv.js" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

(function () {
    'use strict';

    angular
        .module('app.directives')
        .directive('modalView', modalView);

    modalView.$inject = ['$rootScope'];

    function modalView($rootScope) {
        var directive = {
            restrict: 'E',
            transclude: true,
            scope: {
                onShow: '=',
                onHide: '=',
                onSubmit: '=',
                modalId: '@',
                title: '@',
                submitText: '@',
                spinnerShowEvent: '@',
                spinnerHideEvent: '@',
                popoverShowEvent: '@',
                popoverHideEvent: '@'
            },
            controller: function () {},
            compile: function compile() {
                return {
                    pre: preLink,
                    post: postLink
                }
            },
            templateUrl: '/Client/Scripts/Directives/Views/modal-view.html'
        };

        return directive;

        function preLink(scope, iElement, iAttrs, controller) {
            if (!scope.popoverHideEvent) {
                /* 
                    Default value. If popover has no end-event, it uses timer.
                    Need to hide popover when closing modal view. 
                */
                scope.popoverHideEvent = 'hideModalPopover';
            }
        }

        function postLink(scope, element, attrs, controller) {
            var modal = element.find('.modal');

            controller.getModalView = getModalView;

            scope.submit = submit;

            scope.onShow && modal.on('shown.bs.modal', scope.onShow);
            scope.onHide && modal.on('hidden.bs.modal', scope.onHide);

            modal.on('hidden.bs.modal', onHiddenModal);

            if (scope.spinnerShowEvent) {
                var showSpinnerEventListener = $rootScope.$on(scope.spinnerShowEvent, function () {
                    blockElements(true);
                });
            }

            if (scope.spinnerHideEvent) {
                var hideSpinnerEventListener = $rootScope.$on(scope.spinnerHideEvent, function () {
                    blockElements(false);
                });
            }

            var stateChangedListener = $rootScope.$on('$stateChangeStart', function () {
                modal.modal('hide');
            });

            scope.$on('$destroy', onDestroy);

            function getModalView() {
                return modal;
            }

            function submit() {
                scope.onSubmit && scope.onSubmit(modal);
            }

            function onHiddenModal() {
                if (scope.spinnerHideEvent) {
                    $rootScope.$emit(scope.spinnerHideEvent);
                }

                $rootScope.$emit(scope.popoverHideEvent);
                blockElements(false);
            }

            function onDestroy() {
                showSpinnerEventListener && showSpinnerEventListener();
                hideSpinnerEventListener && hideSpinnerEventListener();
                stateChangedListener && stateChangedListener();
            }

            function blockElements(disable) {
                var inputs = $('.modal-body input');
                setDisabledProperty(inputs);

                var textAreas = $('.modal-body textarea');
                setDisabledProperty(textAreas);

                var selects = $('.modal-body select');
                setDisabledProperty(selects);

                function setDisabledProperty(elements) {
                    elements.each(function () {
                        $(this).prop('disabled', disable);
                    });
                }
            }
        }
    }

})();