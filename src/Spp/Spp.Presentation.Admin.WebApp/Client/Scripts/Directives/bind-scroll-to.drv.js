/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿// -----------------------------------------------------------------------
// <copyright file="bind-scroll-to.drv.js" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

(function () {
    'use strict';

    angular
        .module('app.directives')
        .directive('bindScrollTo', bindScrollTo);

    function bindScrollTo() {
        var directive = {
            restrict: 'A',
            scope: {
                bindScrollTo: '@'
            },
            link: function (scope, element, attrs) {
                if (scope.bindScrollTo) {
                    var target = $(scope.bindScrollTo);

                    if (target[0]) {
                        target.scroll(function () {
                            element.scrollLeft(target.scrollLeft());
                        });
                    }
                }
            }
        };
        return directive;
    }

})();