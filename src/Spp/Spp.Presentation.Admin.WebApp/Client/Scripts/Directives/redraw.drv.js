/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿// -----------------------------------------------------------------------
// <copyright file="redraw.drv.js" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

(function () {
    'use strict';

    angular
        .module('app.directives')
        .directive('redraw', redraw);

    redraw.$inject = ['$timeout'];

    // This directive has been created due to IE 11 "freeze multiple selects" issue
    function redraw($timeout) {
        var directive = {
            restrict: 'A',
            link: link
        };
        return directive;

        function link(scope, element, attrs) {
            $timeout(redraw);

            element.on('DOMCharacterDataModified', postponedRedraw);

            function redraw() {
                element.css('visibility', 'hidden');
                element.css('visibility', 'visible');
            }

            function postponedRedraw() {
                $timeout(forceRedraw);

                function forceRedraw() {
                    element.css('width', element.innerWidth() + 1);
                    $timeout(clearChanges);

                    function clearChanges() {
                        element.css('width', '');
                    }
                }
            }
        }
    }

})();