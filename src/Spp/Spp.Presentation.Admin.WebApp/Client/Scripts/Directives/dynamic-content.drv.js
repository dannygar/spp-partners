/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿// -----------------------------------------------------------------------
// <copyright file="dynamic-content.drv.js" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

(function () {
    'use strict';

    angular
        .module('app.directives')
        .directive('dynamicContent', dynamicContent);

    dynamicContent.$inject = ['$rootScope'];

    function dynamicContent($rootScope) {
        var directive = {
            restrict: 'E',
            transclude: true,
            scope: {
                contentId: '@'
            },
            link: link,
            templateUrl: '/Client/Scripts/Directives/Views/dynamic-content.html'
        };
        return directive;

        function link(scope, element, attrs) {
            var defaultContent = element.html();

            $rootScope.$on('$stateChangeStart', function () {
                element.html(defaultContent);
            });

            $rootScope.$emit('dynamicContentReady');
        }
    }

})();