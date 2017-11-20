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
        .factory('DebounceService', DebounceService);

    DebounceService.$inject = ['$timeout', '$q'];

    function DebounceService($timeout, $q) {
        var service = {
            getInstance: getInstance
        };

        return service;

        function getInstance() {
            return {
                timeout: null,
                deferred: $q.defer(),
                debounce: debounce
            };
        }

        function debounce(func, wait, args, immediate) {
            var context = this;

            var callNow = immediate && !context.timeout;

            if (context.timeout) {
                $timeout.cancel(context.timeout);
            }

            context.timeout = $timeout(later, wait);

            if (callNow) {
                context.deferred.resolve(func.call(context, args));
                context.deferred = $q.defer();
            }

            return context.deferred.promise;

            function later() {
                context.timeout = null;

                if (!immediate) {
                    context.deferred.resolve(func.call(context, args));
                    context.deferred = $q.defer();
                }
            }
        }
    }

})();