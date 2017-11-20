/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.


(function () {
    'use strict';

    angular.module('app', [ 'app.controllers',
            'app.services',
            'app.directives',
            'app.filters',
            'ui.router',
            'AdalAngular',
            'azureBlobUpload'
        ])
        .config(configuration);

    function configuration() {
        String.prototype.format = String.prototype.format || stringFormat;
        configureMomentJs();
    }

    function stringFormat() {
        var args = arguments;

        return this.replace(/{(\d+)}/g, replacer);

        function replacer(match, number) {
            return typeof args[number] != 'undefined'
                ? args[number]
                : match;
        }
    }

    function configureMomentJs() {
        var abbrs = {
            EST: 'Eastern Standard Time',
            EDT: 'Eastern Daylight Time',
            CST: 'Central Standard Time',
            CDT: 'Central Daylight Time',
            MST: 'Mountain Standard Time',
            MDT: 'Mountain Daylight Time',
            PST: 'Pacific Standard Time',
            PDT: 'Pacific Daylight Time'
        };

        moment.fn.zoneName = function () {
            var abbr = this.zoneAbbr();
            return abbrs[abbr] || abbr;
        };
    }

})();