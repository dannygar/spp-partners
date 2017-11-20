/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿// -----------------------------------------------------------------------
// <copyright file="content-for.drv.js" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

(function () {
    'use strict';

    angular
        .module('app.directives')
        .directive('contentFor', contentFor);

    contentFor.$inject = ['$rootScope'];

    function contentFor($rootScope) {
        var directive = {
            restrict: 'A',
            scope: {
                contentFor: '@'
            },
            link: link
        };
        return directive;

        function link(scope, element, attrs) {
            var target = $('dynamic-content[content-id=' + scope.contentFor + ']');
            element.hide();

            target && bindElement();

            function bindElement() {
                var container = target.find('[ng-transclude]');

                if (container.length === 0) {
                    var listener = $rootScope.$on('dynamicContentReady', function () {
                        listener();
                        bindElement();
                    });
                    return;
                }

                container.html(element);
                element.show();
            }
        }
    }

})();