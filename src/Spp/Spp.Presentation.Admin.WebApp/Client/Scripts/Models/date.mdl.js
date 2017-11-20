/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.


(function () {
    'use strict';

    Date.getLocalTimezoneOffset = getLocalTimezoneOffset;
    Date.getLocalTimezoneOffsetAsInteger = getLocalTimezoneOffsetAsInteger;

    Date.prototype.getDayName = getDayName;
    Date.prototype.getMonthName = getMonthName;
    Date.prototype.nextDate = nextDate;
    Date.prototype.onTheSameDay = onTheSameDay;
    Date.prototype.previousDate = previousDate;
    Date.prototype.toMidnight = toMidnight;
    Date.prototype.toQueryParameterString = toQueryParameterString;
    Date.prototype.toUTCDateTime = toUTCDateTime;

    function getLocalTimezoneOffset() {
        var today = new Date();
        var offset = getTimezoneOffsetAsTime(today);
        var sign = today.getTimezoneOffset() < 0
            ? '+'
            : '-';

        return sign + pad(offset.hours) + ':' + pad(offset.minutes);
    }

    function getLocalTimezoneOffsetAsInteger() {
        return -(new Date().getTimezoneOffset() / 60);
    }

    function getDayName() {
        var days = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];

        return days[this.getDay()];
    }

    function getMonthName() {
        var months = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October',
            'November', 'December'];

        return months[this.getMonth()];
    }

    function nextDate() {
        var result = new Date(this);
        result.setHours(0, 0, 0, 0);
        result.setDate(result.getDate() + 1);

        return result;
    }

    function onTheSameDay(date) {
        return this.toDateString() === new Date(date).toDateString();
    }

    function previousDate() {
        var result = new Date(this);
        result.setHours(0, 0, 0, 0);
        result.setDate(result.getDate() - 1);

        return result;
    }

    function toMidnight() {
        this.setHours(0, 0, 0, 0);
        return this;
    }

    function toQueryParameterString(ignoreTimezone) {
        var timezoneOffset = ignoreTimezone
            ? 0
            : -this.getTimezoneOffset();

        var sign = timezoneOffset >= 0
            ? '+'
            : '-';

        return this.getFullYear()
            + '-' + pad(this.getMonth() + 1)
            + '-' + pad(this.getDate())
            + 'T' + pad(this.getHours())
            + ':' + pad(this.getMinutes())
            + ':' + pad(this.getSeconds())
            + sign + pad(timezoneOffset / 60)
            + ':' + pad(timezoneOffset % 60);
    }

    function toUTCDateTime() {
        var result = new Date(this);
        result.setHours(this.getUTCHours(), this.getUTCMinutes());

        return result;
    }

    function pad(num) {
        var norm = Math.abs(Math.floor(num));
        return (norm < 10 ? '0' : '') + norm;
    }

    function getTimezoneOffsetAsTime(date) {
        var timezoneOffset = -date.getTimezoneOffset();
        var hours = pad(timezoneOffset / 60);
        var minutes = pad(timezoneOffset % 60);

        return new Time(hours, minutes);
    }

})();