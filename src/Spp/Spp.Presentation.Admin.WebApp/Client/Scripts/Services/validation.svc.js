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
        .factory('ValidationService', ValidationService);

    ValidationService.$inject = ['UtilsService', 'config'];

    function ValidationService(UtilsService, config) {
        var emailRegexp = /^(([^<>()[\]\.,;:\s@\"]+(\.[^<>()[\]\.,;:\s@\"]+)*)|(\".+\"))@(([^<>()[\]\.,;:\s@\"]+\.)+[^<>()[\]\.,;:\s@\"]{2,})$/i;

        var service = {
            validateFutureSession: validateFutureSession,
            validatePastSession: validatePastSession,
            validateSessionRemoval: validateSessionRemoval,
            validateUser: validateUser,
            validateUserPhoto: validateUserPhoto,
            validateUserPhotoExtension: validateUserPhotoExtension
        };

        return service;

        function validateFutureSession(session) {
            if (session.players.length === 0) {
                return 'Please add players to session.';
            }

            if (!session.durationMinutes
                || !UtilsService.isInteger(session.durationMinutes)
                || session.durationMinutes < 0
                || session.durationMinutes > config.session.maxDuration) {
                return 'Please enter valid duration. Duration should not exceed ' + config.session.maxDuration +
                    ' and should be integer number.';
            }
            /* coach rating and alertAfterMinutes removed
            if (!session.coachRating
                || typeof session.coachRating !== 'number'
                || session.coachRating < 1
                || session.coachRating > 10
                || Math.floor(session.coachRating % config.session.coachRatingStep) !== 0) {
                return 'Please enter valid coaches rating. Coaches rating should not exceed ' + config.session.maxCoachesRating +
                    ', should be more than one and should be divisible by ' + config.session.coachRatingStep + '.';
            }
            
            if (!session.alertAfterMinutes
                || !UtilsService.isInteger(session.alertAfterMinutes)
                || session.alertAfterMinutes < 0
                || session.alertAfterMinutes > config.session.maxAlertsAfterMinutes) {
                return 'Please enter valid Alert after (mins). Alert after (mins) should not exceed ' +
                    config.session.maxAlertsAfterMinutes + ' and should be integer number.';
            }
            */
            if (!session.startDateTime
                || session.startDateTime < new Date()) {
                return 'Please enter valid Date and Time. You cannot create a new session if start date time' +
                    ' of this session has already past.';
            }

            return null;
        }

        function validatePastSession(session) {
            if (!session.durationMinutes
                || !UtilsService.isInteger(session.durationMinutes)
                || session.durationMinutes < 0
                || session.durationMinutes > config.session.maxDuration) {
                return 'Please enter valid duration. Duration should not exceed ' + config.session.maxDuration +
                    ' and should be integer number.';
            }

            if (!session.coachRating
                || typeof session.coachRating !== 'number'
                || session.coachRating < 1
                || session.coachRating > 10
                || Math.floor(session.coachRating % config.session.coachRatingStep) !== 0) {
                return 'Please enter valid coaches rating. Coaches rating should not exceed ' + config.session.maxCoachesRating +
                    ', should be more than one and should be divisible by ' + config.session.coachRatingStep + '.';
            }

            return null;
        }

        function validateSessionRemoval(session) {
            var startDateTime = new Date(Date.parse(session.StartDateTime));

            if (startDateTime <= new Date()) {
                return 'You cannot delete a session if start date time of this session has already past.';
            }
        }

        function validateUser(user) {
            if (!emailRegexp.test(user.email)) {
                return 'Please enter a valid email.';
            }

            if (user.dateOfBirth) {
                var unixTime = Date.parse(user.dateOfBirth);
                var dateTime = new Date(unixTime);

                if (dateTime == 'Invalid Date') {
                    return 'Please enter a valid date of birth.';
                }
            }

            return null;
        }

        function validateUserPhoto(cropperData, image) {
            if (!cropperData) return null;

            var message = 'Crop area must be inside image boundaries.';

            if (cropperData.cropArea.x < 0 || cropperData.cropArea.y < 0
                || cropperData.cropArea.x + cropperData.cropArea.width > image.naturalWidth
                || cropperData.cropArea.y + cropperData.cropArea.height > image.naturalHeight) {
                return message;
            }

            return null;
        }

        function validateUserPhotoExtension(fileName) {
            var message = 'Must be a valid image file format (e.g. png, jpeg, jpg)';
            var fileExtension = fileName.split('.').pop().toLowerCase();
            var allowedExtensions = /(\jpg|\jpeg|\png)$/i;

            if (!allowedExtensions.exec(fileExtension)) {
                return message;
            }

            return null;
        }
    }

})();