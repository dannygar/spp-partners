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
        .filter('relatedById', relatedById);

    relatedById.$inject = ['config'];

    function relatedById(config) {
        return function (input, boundArray, property, value, relatedObj, relatedProperty, setDefault) {
            input = input || [];
            var output = input;

            if (boundArray && property && value) {
                output = input.filter(function (item) {
                    var boundItem = findRelatedItem(boundArray, value);

                    if (!boundItem || isEmptyElement(boundItem)) {
                        return false;
                    }

                    var id = getObjId(item);

                    return boundItem[property].indexOf(id) + 1;
                });
            }

            if (relatedObj && relatedProperty && !relatedObj[relatedProperty]) {
                relatedObj[relatedProperty] = setDefault ? getObjId(output[0]) : null;
            }

            return output;
        };

        function findRelatedItem(boundArray, value) {
            return boundArray.filter(function (obj) {
                return getObjId(obj) == value;
            })[0];
        }

        function getObjId(obj) {
            if (obj) {
                return obj.Id || obj.id;
            }

            return null;
        }

        function isEmptyElement(item) {
            return getObjId(item) == config.emptyElement.Id;
        }
    }

})();