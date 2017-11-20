/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.


var Time = function () {
    'use strict';

    var TimeObj = Time;

    TimeObj.noon = new Time(12, 0);
    TimeObj.compare = compare;
    TimeObj.getDateTime = getDateTime;
    TimeObj.parseTimeString = parseTimeString;
    TimeObj.timeDifference = timeDifference;

    TimeObj.prototype.addMinutes = addMinutes;
    TimeObj.prototype.toMilliseconds = toMilliseconds;
    TimeObj.prototype.toString = toString;
    TimeObj.prototype.to24FormatString = to24FormatString;

    return TimeObj;

    function Time() {
        var args = arguments;
        var time = this;

        if (args.length > 1) {
            time.hours = parseInt(args[0]) || 0;
            time.minutes = parseInt(args[1]) || 0;

        } else if ($.isNumeric(args[0])) {
            var timeInMinutes = Math.round(args[0] / (1000 * 60));
            var minutes = timeInMinutes % 60;
            var hours = (timeInMinutes - minutes) / 60;

            time.hours = hours;
            time.minutes = minutes;

        } else if (args[0] instanceof Time) {
            time.hours = args[0].hours;
            time.minutes = args[0].minutes;

        } else {
            throw new Error('Invalid constructor parameter');
        }
    }

    function compare(time1, time2) {
        var mill1 = time1.toMilliseconds();
        var mill2 = time2.toMilliseconds();

        if (mill1 < mill2) {
            return -1;
        }
        return mill1 > mill2 ? 1 : 0;
    }

    function getDateTime(date, ignoreTimezone) {
        var date = ignoreTimezone
            ? new Date(date).toUTCDateTime()
            : new Date(date);

        return new Time(date.getHours(), date.getMinutes());
    }

    function parseTimeString(str) {
        if (!isString(str)) {
            throw new Error('Parameter is not a String.')
        }

        var time = str.split(/\.|:|-|,/);
        if ((str.indexOf('AM') + 1) && time[0] && time[0] === "12") {
            time[0] = 0;
        }
        if ((str.indexOf('PM') + 1) && time[0] && time[0] !== "12") {
            time[0] = 12 + parseInt(time[0]);
        }

        return new Time(time[0], time[1]);
    }

    function timeDifference(time1, time2, isAbs) {
        var mill1 = time1.toMilliseconds();
        var mill2 = time2.toMilliseconds();

        return isAbs ? Math.abs(mill1 - mill2) : mill1 - mill2;
    }

    function addMinutes(minutes) {
        var mill1 = this.toMilliseconds();
        var mill2 = minutes * 60 * 1000;

        var newTime = new Time(mill1 + mill2);
        this.hours = newTime.hours;
        this.minutes = newTime.minutes;

        return this;
    }

    function toMilliseconds() {
        return (this.hours * 60 * 60 * 1000) + (this.minutes * 60 * 1000);
    }

    function toString() {
        var hours = this.hours;
        var minutes = this.minutes;
        var text = hours < 12 ? 'AM' : 'PM';

        minutes = (minutes < 10) ? '0' + minutes : minutes;
        hours = hours % 12;
        hours = hours ? hours : 12; // the hour '0' should be '12'

        return hours + ':' + minutes + ' ' + text;
    }

    function to24FormatString() {
        var hours = this.hours;
        var minutes = this.minutes;

        minutes = (minutes < 10) ? '0' + minutes : minutes;
        hours = (hours < 10) ? '0' + hours : hours;

        return hours + ':' + minutes;
    }

    function isString(val) {
        return typeof val === 'string' || val instanceof String
    }

}();
