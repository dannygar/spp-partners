/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.


(function () {
    'use strict';

    angular
        .module('app.services')
        .factory('UtilsService', UtilsService);

    UtilsService.$inject = ['$q', 'config'];

    function UtilsService($q, config) {
        var service = {
            getTodayDateAsString: getTodayDateAsString,
            getWeekForDate: getWeekForDate,
            isEarlierThanNow: isEarlierThanNow,
            stopEventPropagation: stopEventPropagation,
            isInteger: isInteger,
            isString: isString,
            isFunction: isFunction,
            isArray: isArray,
            generateUniqueName: generateUniqueName,
            executePromiseForCollection: executePromiseForCollection,
            replaceItemsInCollectionById: replaceItemsInCollectionById,
            hasCollectionId: hasCollectionId,
            initializeCollectionWithEmptyElement: initializeCollectionWithEmptyElement
        };

        return service;

        function getTodayDateAsString() {
            var today = new Date().toMidnight();
            return today.toString();
        }

        function getWeekForDate(date) {
            var numberOfWeekDay = new Date(date).getDay();
            var numberPreviousWeekDays = numberOfWeekDay ? numberOfWeekDay - 1 : 6; //Sunday is 0

            var currentDate = new Date(date).toMidnight();
            currentDate.setDate(currentDate.getDate() - numberPreviousWeekDays);

            var output = [];

            for (var i = 0; i < 7; i++) {
                output.push({
                    name: currentDate.getDayName(),
                    date: new Date(currentDate)
                });

                currentDate = currentDate.nextDate();
            }

            return output;
        }

        function isEarlierThanNow(date) {
            if (service.isString(date)) {
                date = new Date(date);
            }

            return date.getTime() < Date.now();
        }

        function stopEventPropagation(event) {
            event.stopPropagation();
            event.cancelBubble = true;
        }

        function isInteger(n) {
            return Number(n) === n && n % 1 === 0
        }

        function isString(val) {
            return typeof val === 'string' || val instanceof String
        }

        function isFunction(obj) {
            return typeof obj === 'function';
        }

        function isArray(obj) {
            return Array.isArray(obj);
        }

        function generateUniqueName() {
            var guid = generateGuid();
            var milliseconds = new Date().getTime();
            return guid + milliseconds;
        }

        function executePromiseForCollection(collection, promise) {
            var deferred = $q.defer();

            if (collection.length === 0) {
                deferred.resolve();
                return deferred.promise;
            }

            var handledItemsCount = 0;

            collection.forEach(function (item) {
                promise(item).then(promiseSuccessCallback, deferred.reject);
            });

            return deferred.promise;

            function promiseSuccessCallback() {
                handledItemsCount++;

                if (handledItemsCount === collection.length) {
                    deferred.resolve();
                }
            }
        }

        function replaceItemsInCollectionById(collection, items) {
            items.forEach(function (item) {
                replaceItemInCollectionById(collection, item);
            });
        }

        function replaceItemInCollectionById(collection, item) {
            for (var i = 0; i < collection.length; i++) {
                if (collection[i].Id === item.Id) {
                    collection[i] = item;
                }
            }
        }

        function hasCollectionId(collection, id) {
            return collection.some(function (item) {
                return item.Id === id;
            });
        }

        function initializeCollectionWithEmptyElement(collecton) {
            collecton.unshift && collecton.unshift(config.emptyElement);

            return collecton;
        }

        function generateGuid() {
            return s4() + s4() + '-' + s4() + '-' + s4() + '-' +
                s4() + '-' + s4() + s4() + s4();

            function s4() {
                return Math.floor((1 + Math.random()) * 0x10000)
                    .toString(16)
                    .substring(1);
            }
        }
    }

})();