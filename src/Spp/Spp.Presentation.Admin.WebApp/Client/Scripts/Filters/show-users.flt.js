/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.


(function () {
    'use strict';

    angular
        .module('app.filters')
        .filter('showUsers', showUsers);

    function showUsers() {
        return function (items, showActive, showNonActive) {
            var filtered = [];

            for (var i = 0; i < items.length; i++) {
                var item = items[i];
                isFilteredItem(item) && filtered.push(item);
            }

            return filtered;

            function isFilteredItem(item) {
                return (showActive && item.isActive)
                    || (showNonActive && !item.isActive);
            }
        };
    }

})();