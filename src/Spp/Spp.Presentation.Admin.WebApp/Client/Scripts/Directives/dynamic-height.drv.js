/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿// -----------------------------------------------------------------------
// <copyright file="dynamic-height.drv.js" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

(function () {
    'use strict';

    angular
        .module('app.directives')
        .directive('dynamicHeight', dynamicHeight);

    function dynamicHeight() {
        var directive = {
            restrict: 'A',
            scope: {
                dynamicHeight: '=',
                maxHeight: '=',
                maxValue: '='
            },
            link: link
        };
        return directive;

        function link(scope, element, attrs) {
            var value = scope.dynamicHeight || 0;
            var maxValue = scope.maxValue || 100;
            var maxHeight = scope.maxHeight || 100;

            var height = maxHeight / (maxValue / value);
            element.css('height', height + 'px');
        }
    }

})();