/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿// -----------------------------------------------------------------------
// <copyright file="personal-image-gallery.drv.js" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

(function () {
    'use strict';

    angular
        .module('app.directives')
        .directive('pager', pager);

    pager.$inject = ['$filter', 'UtilsService', 'config'];

    function pager($filter, UtilsService, config) {
        var directive = {
            restrict: 'E',
            transclude: true,
            scope: {
                input: '=',
                output: '@',
                filter: '@',
                numberOfItemsOnPage: '=',
                numberOfVisiblePages: '='
            },
            link: link,
            templateUrl: '/Client/Scripts/Directives/Views/pager.html'
        };
        return directive;

        function link(scope, element, attrs, ctrl, transclude) {
            if (!UtilsService.isInteger(scope.numberOfItemsOnPage)) {
                throw new Error('Attribute "number-of-items-on-page" is required and should be an integer.');
            }

            /* Translude scope */
            var transcludeScope = scope.$parent.$new();

            transcludeScope.filteredItems = scope.input;
            transcludeScope.filter = parseFilter(scope.input, scope.filter, transcludeScope, updateFilteredItems);

            transcludeScope.$watchCollection('filteredItems', updateAvailablePages);

            transclude(transcludeScope, handleTransclude);

            /* Directive scope */
            scope.numberOfVisiblePages = scope.numberOfVisiblePages || config.pager.defaultNumberOfVisiblePages;
            scope.selectedPage = null;
            scope.pages = splitToPages(scope.numberOfItemsOnPage, transcludeScope.filteredItems);

            scope.openPage = openPage;
            scope.openPreviousPage = openPreviousPage;
            scope.openNextPage = openNextPage;

            // Open first page
            scope.openPage(scope.pages[0]);

            scope.$watchCollection('input', onInputUpdate);


            function parseFilter(collection, filterStr, filterParamsScope, watchCallback) {
                var filter = {
                    func: null,
                    params: [{name: "targetCollection", value: angular.copy(collection)}],
                    apply: apply,
                    updateTargetCollection: updateTargetCollection
                };

                var splitResult = filterStr.split(':');
                var filterName = splitResult[0];

                var filterFunc = $filter(filterName);

                if (angular.isFunction(filterFunc)) {
                    filter.func = filterFunc;

                    splitResult.slice(1).forEach(function (paramName) {
                        var param = {
                            name: paramName,
                            value: filterParamsScope[paramName]
                        };

                        filter.params.push(param);
                        filterParamsScope.$watch(paramName, updateValue);

                        function updateValue(newVal, oldVal) {
                            var index = filter.params.indexOf(param);

                            filter.params[index].value = newVal;

                            watchCallback(newVal, oldVal)
                        }
                    });
                }

                return filter;

                function apply() {
                    if (!this.func) {
                        return this.params[0].value;
                    }

                    var params = this.params.map(function (item) {
                        return item.value;
                    });

                    return this.func.apply(this, params);
                }

                function updateTargetCollection(newCollection) {
                    this.params[0].value = newCollection;
                }
            }

            function splitToPages(numberOfItemsOnPage, collection) {
                var output = [];
                var range = Math.ceil(collection.length / numberOfItemsOnPage);

                for (var i = 0; i < range; i++) {
                    output.push({
                        index: i,
                        number: i + 1,
                        isActive: false
                    });
                }

                return output;
            }

            function onInputUpdate(newCollection) {
                transcludeScope.filter.updateTargetCollection(newCollection);
                transcludeScope.filteredItems = transcludeScope.filter.apply();
            }

            function updateFilteredItems() {
                transcludeScope.filteredItems = transcludeScope.filter.apply();
            }

            function updateAvailablePages() {
                scope.selectedPage = null;
                scope.pages = splitToPages(scope.numberOfItemsOnPage, transcludeScope.filteredItems);
                scope.openPage(scope.pages[0]);
            }

            function updateOutputCollection() {
                var startIndex = scope.selectedPage.index * scope.numberOfItemsOnPage;
                var endIndex = startIndex + scope.numberOfItemsOnPage;

                transcludeScope[scope.output] = transcludeScope.filteredItems.slice(startIndex, endIndex);
            }

            function openPage(page) {
                if (!page) {
                    transcludeScope[scope.output] = [];
                    return;
                }

                if (scope.selectedPage) {
                    if (page.index === scope.selectedPage.index) {
                        return;
                    }
                    scope.selectedPage.isActive = false;
                }

                scope.selectedPage = page;
                scope.selectedPage.isActive = true;

                updateOutputCollection();
            }

            function openNextPage() {
                var index = scope.selectedPage.index + 1;
                tryToOpenPageByIndex(index);
            }

            function openPreviousPage() {
                var index = scope.selectedPage.index - 1;
                tryToOpenPageByIndex(index);
            }

            function tryToOpenPageByIndex(index) {
                var page = scope.pages.filter(byIndex)[0];

                page && openPage(page);

                function byIndex(item) {
                    return item.index == index;
                }
            }

            function handleTransclude(clone, scope) {
                element.find('[ng-transclude]').replaceWith(clone);
            }
        }
    }

})();