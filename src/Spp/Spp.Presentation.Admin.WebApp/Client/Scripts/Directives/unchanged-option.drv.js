/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿// -----------------------------------------------------------------------
// <copyright file="unchanged-option.drv.js" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

(function () {
    'use strict';

    angular
        .module('app.directives')
        .directive('unchangedOption', unchangedOption);

    unchangedOption.$inject = ['$timeout'];

    function unchangedOption($timeout) {
        var directive = {
            restrict: 'A',
            scope: {
                optionLabel: '@',
                needSelect: '=', /* Should we select the option? */
                removeSelection: '@' /* If needSelect == false, should we remove selection? */
            },
            link: link
        };
        return directive;

        function link(scope, element, attrs) {
            element.change(onChange);
            $timeout(onChange, 0);

            scope.$watch('needSelect', onChange);

            function onChange() {
                var option = element.find("option[label='" + scope.optionLabel + "']");

                if (scope.needSelect) {
                    option.prop('selected', 'selected');
                    option.attr('selected', 'selected');
                } else if (scope.removeSelection) {
                    option.removeAttr('selected');
                }

                if (option.prop('disabled') === false) {
                    option.prop('disabled', true);
                }
            }
        }
    }

})();