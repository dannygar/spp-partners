/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿// -----------------------------------------------------------------------
// <copyright file="nav-tabs.drv.js" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

(function () {
    'use strict';

    angular
        .module('app.directives')
        .directive('navTabs', navTabs);

    navTabs.$inject = ['$state', '$rootScope'];

    function navTabs($state, $rootScope) {
        var directive = {
            restrict: 'C',
            link: link
        };
        return directive;

        function link(scope, element, attrs) {
            var currentTab = findCurrentTab();
            currentTab && currentTab.addClass('active');

            window.onhashchange = onHashChange;

            var viewLoadedListener = $rootScope.$on('$viewContentLoaded', onViewContentLoaded);

            denyFocusOnNotActiveLinks();

            scope.$on('$destroy', onDestroy);

            function findCurrentTab(fullURL) {
                var parentSection = fullURL
                    ? getParentSectionNameFromFullUrl(fullURL)
                    : getParentSectionNameFromCurrentState();

                var items = element.find('li a');

                for (var i = 0; i < items.length; i++) {
                    var hrefHashIndex = items[i].href.indexOf('#');
                    var hrefHashUrl = items[i].href.substr(hrefHashIndex);

                    if (hrefHashUrl.indexOf(parentSection) !== -1) {
                        return $(items[i]).parent();
                    }
                }
            }

            function onHashChange(event) {
                highlightNavTab(event.newURL || event.target.location.hash);
            }

            function onViewContentLoaded() {
                var stateParentSection = getParentSectionNameFromCurrentState();
                var urlParentSection = getParentSectionNameFromFullUrl(window.location.href);

                // If UI router doesn't update url (it's ui router bug)
                if (stateParentSection !== urlParentSection) {
                    $state.go($state.current.name);
                    highlightNavTab();
                }
            }

            function highlightNavTab(url) {
                if (currentTab) {
                    currentTab.removeClass('active');
                    currentTab.find('a').blur();
                }

                currentTab = findCurrentTab(url);

                if (currentTab) {
                    currentTab.addClass('active');
                    currentTab.focus();
                }
            }

            function getParentSectionNameFromFullUrl(url) {
                var hashIndex = url.indexOf('#');
                var hashUrl = url.substr(hashIndex);// "#/app/parent-section/other section"
                var sections = hashUrl.split('/');

                return sections[2];
            }

            function getParentSectionNameFromCurrentState() {
                var sections =  $state.current.url.split('/');// "/parent-section/other-section"
                return sections[1];
            }

            function denyFocusOnNotActiveLinks() {
                var links = element.find('li a');
                links.each(setFocusinEventHandler);
            }

            function setFocusinEventHandler(i, el) {
                var el = $(el);

                el.focusin(onFocusin);

                function onFocusin() {
                    if(currentTab && el.text() !== currentTab.text()) {
                        el.blur();
                    }
                }
            }

            function onDestroy() {
                viewLoadedListener();
            }
        }
    }

})();