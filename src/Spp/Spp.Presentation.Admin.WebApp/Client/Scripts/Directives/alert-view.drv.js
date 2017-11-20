/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿// -----------------------------------------------------------------------
// <copyright file="alert-view.drv.js" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

(function () {
    'use strict';

    angular
        .module('app.directives')
        .directive('alertView', alertView);

    alertView.$inject = ['$rootScope'];

    function alertView($rootScope) {
        var directive = {
            restrict: 'E',
            transclude: true,
            scope: {
                onShow: '=',
                onHide: '=',
                modalId: '@',
                title: '@'
            },
            link: link,
            templateUrl: '/Client/Scripts/Directives/Views/alert-view.html'
        };
        return directive;

        function link(scope, element, attrs) {
            var modal = element.find('.modal');

            if (scope.onShow) {
                modal.on('shown.bs.modal', scope.onShow);
            }

            if (scope.onHide) {
                modal.on('hidden.bs.modal', scope.onHide);
            }

            var stateChangedListener = $rootScope.$on('$stateChangeStart', function () {
                modal.modal('hide');
            });

            scope.$on('$destroy', onDestroy);

            function onDestroy() {
                stateChangedListener && stateChangedListener();
            }
        }
    }

})();