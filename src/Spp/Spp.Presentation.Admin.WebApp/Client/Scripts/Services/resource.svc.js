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
        .factory('ResourceService', ResourceService);

    ResourceService.$inject = ['config', '$http', '$state', 'AuthService', 'UtilsService', '$q'];

    function ResourceService(config, $http, $state, AuthService, UtilsService, $q) {
        var service = {
            getUserInfo: getUserInfo,
            updateUser: updateUser,
            updateUserPhoto: updateUserPhoto,
            removeUserPhoto: removeUserPhoto,
            changeUserPassword: changeUserPassword,
            setUserInGameTag: setUserInGameTag,
            getUserOptions: getUserOptions,
            getAllSessions: getAllSessions,
            getAllPractices: getAllPractices,
            getPractice: getPractice,
            getPracticeBySessionId: getPracticeBySessionId,
            getSession: getSession,
            createSession: createSession,
            createSessionPractice: createSessionPractice,
            updateSession: updateSession,
            deleteSession: deleteSession,
            getAllDrills: getAllDrills,
            updateQuestionnaire: updateQuestionnaire,
            deleteUserAnswer: deleteUserAnswer,
            getWellnessForPeriod: getWellnessForPeriod,
            switchWellness: switchWellness,
            getAllSquads: getAllSquads,
            getSquad: getSquad,
            createSquad: createSquad,
            updateSquad: updateSquad,
            deleteSquad: deleteSquad,
            syncWithAAD: syncWithAAD,
            getAllSessionTypes: getAllSessionTypes,
            getAllSessionMesocycles: getAllSessionMesocycles,
            createSessionMesocycle: createSessionMesocycle,
            updateSessionMesocycle: updateSessionMesocycle,
            deleteSessionMesocycle: deleteSessionMesocycle,
            getAllSessionMicrocycles: getAllSessionMicrocycles,
            createSessionMicrocycle: createSessionMicrocycle,
            updateSessionMicrocycle: updateSessionMicrocycle,
            deleteSessionMicrocycle: deleteSessionMicrocycle,
            updateSessionPractice: updateSessionPractice,
            getAllPositions: getAllPositions,
            createPosition: createPosition,
            updatePosition: updatePosition,
            deletePosition: deletePosition,
            getAllSubpositions: getAllSubpositions,
            createSubpositions: createSubpositions,
            updateSubposition: updateSubposition,
            deleteSubposition: deleteSubposition,
            getAllPersonalImages: getAllPersonalImages,
            createPersonalImage: createPersonalImage,
            updatePersonalImage: updatePersonalImage,
            deletePersonalImage: deletePersonalImage,
            getTeam: getTeam,
            getTeams: getTeams,
            getLocations: getLocations,
            getTeamPlayers: getTeamPlayers,
            createTeam: createTeam,
            updateTeam: updateTeam,
            deleteTeam: deleteTeam,
            getUserV2: getUserV2,
            createUserV2: createUserV2,
            updateUserV2: updateUserV2,
            deleteUserV2: deleteUserV2,
            authenUserV2: authenUserV2,
            addUserSession: addUserSession,
            addUsersToSession: addUsersToSession
        
    };

        return service;

        function asyncRequest(url, params, data, method, headers) {
            
            return AuthService.updateApiAccessToken().then(function () {
                return makeApiRequest(url, params, data, method, headers);
            });

        }

        function makeApiRequest(url, params, data, method, headers) {
            return $http({
                method: method || 'GET',
                url: config.apiEndpoint + url,
                params: params || null,
                data: data || null,
                headers: headers || {
                    'Authorization': 'Bearer ' + AuthService.token.access_token,
                    'X-Certified-For-Access': 'management-access',
                    'X-Time-Zone-Id': moment.tz.guess()
                }
            }).then(requestSuccessCallback, requestErrorCallback);

            function requestSuccessCallback(response) {
                if (response.errors || !isSuccessResponseStatus(response.status)) {
                    return $q.reject(requestErrorCallback(response));
                }

                return response.data;

                function isSuccessResponseStatus(status) {
                    return status
                        && status >= 200
                        && status < 300;
                }
            }

            function requestErrorCallback(response) {

                var errors = response.data
                    ? response.data.Errors
                    : response.errors;

                return $q.reject({errors: errors, status: response.status});
            }
        }

        function getUserInfo(id) {
            return asyncRequest('/api/v1/auth/user/' + id);
        }

        function updateUser(id, data) {
            return asyncRequest('/api/users/' + id, null, data, 'PATCH');
        }

        function changeUserPassword(id, password) {
            return asyncRequest('/api/users/' + id + '/setPassword', null, {newPassword: password}, 'POST');
        }

        function setUserInGameTag(id, date, value) {
            return asyncRequest('/api/users/' + id + '/ingame', {now: new Date(date), inGame: value}, null, 'POST');
        }

        function updateUserPhoto(id, data) {
            return asyncRequest('/api/users/' + id + '/avatar', null, data, 'POST');
        }

        function removeUserPhoto(id) {
            return asyncRequest('/api/users/' + id + '/avatar', null, null, 'DELETE');
        }

        function getUserOptions() {
            return asyncRequest('/api/users/options');
        }

        function getAllSessions(startDate, endDate) {
            var startDateParam = new Date(startDate).toMidnight();

            var endDateParam = new Date(endDate || startDateParam);
            endDateParam = endDateParam.nextDate();

            return asyncRequest('/api/v1/sessions', { from: startDateParam, to: endDateParam });
        }

        function getAllPractices() {
            return asyncRequest('/api/v1/practices');
        }

        function getPractice(id) {
            return asyncRequest('/api/v1/practices/id/'+ id);
        }

        function getPracticeBySessionId(id) {
            var practiceData = asyncRequest('/api/v1/practices/' + id);
            return practiceData;
        }

        function getSession(id) {
            return asyncRequest('/api/v1/sessions/' + id);
        }

        function createSession(session) {
            return asyncRequest('/api/v1/sessions', null, session, 'POST'); 
        }

        function updateSession(data) {
            return asyncRequest('/api/v1/sessions/update', null, data, 'POST');
        }

        function createSessionPractice(session) {
            return asyncRequest('/api/v1/practices', null, session, 'POST');
        }

        function updateSessionPractice(data) {
            return asyncRequest('/api/v1/practices/update', null, data, 'POST');
        }

        function deleteSession(id) {
            return asyncRequest('/api/v1/sessions/delete/' + id, null, null, 'POST');
        }

        function getAllDrills() {
            return asyncRequest('/api/v1/practices/drills');
        }

        function getAllDrillsByPracticeId(practiceId) {
            return asyncRequest('/api/v1/practices/drills' + practiceId);
        }

        function updateQuestionnaire(id, data) {
            return asyncRequest('/api/questionnaires/' + id, {timeZoneOffset: Date.getLocalTimezoneOffset()}, data, 'PATCH');
        }

        function deleteUserAnswer(userId, questionnaireId) {
            return asyncRequest('/api/questionnaires/answers', {
                userId: userId,
                questionnaireId: questionnaireId
            }, null, 'DELETE');
        }

        function getWellnessForPeriod(startDateTimeFrom, startDateTimeTo) {
            return asyncRequest('/api/questionnaires/wellness', {  // TODO: Update to TPP API Endpoint
                startDateTimeFrom: new Date(startDateTimeFrom).toQueryParameterString(),
                startDateTimeTo: new Date(startDateTimeTo).toQueryParameterString()
            });
        }

        function switchWellness(data) {
            return asyncRequest('/api/questionnaires/switchWellness', null, data, 'POST');
        }

        function getAllSquads() {
            return asyncRequest('/api/squads');
        }

        function getSquad(id) {
            return asyncRequest('/api/squads/' + id);
        }

        function createSquad(squad) {
            return asyncRequest('/api/squads', null, squad, 'POST');
        }

        function updateSquad(squad) {
            return asyncRequest('/api/squads/' + squad.id, null, squad, 'PATCH');
        }

        function deleteSquad(id) {
            return asyncRequest('/api/squads/' + id, null, null, 'DELETE');
        }

        function syncWithAAD() {
            return asyncRequest('/api/sync');
        }

        function getAllSessionTypes() {
            return asyncRequest('/api/v1/sessions/types');
        }

        function getAllPersonalImages() {
            return asyncRequest('/api/v1/settings/images');
        }

        function createPersonalImage(image) {
            return asyncRequest('/api/v1/settings/create/image', null, image, 'POST');

        }

        function updatePersonalImage(id, data) {
            return createPersonalImage(id, data);
        }

        function deletePersonalImage(imageId) {
            return asyncRequest('/api/v1/settings/delete/image/' + imageId, null, null, 'POST');
        }

        function getTeam(id) {
            return asyncRequest('/api/v1/teams/' + id);
        }

        function getTeams() {
            return asyncRequest('/api/v1/teams');
        }

        function getLocations(){
            return asyncRequest('/api/v1/locations');
        }

        function getTeamPlayers(id) {
            return asyncRequest('/api/v1/teams/players/' + id);
        }

        function createTeam(team) {
            return asyncRequest('/api/v1/teams', null, team, 'POST');
        }

        function updateTeam(team) {
            return asyncRequest('/api/v1/teams/update', null, team, 'POST');
        }

        function deleteTeam(id) {
            return asyncRequest('/api/v1/teams/delete/' + id, null, null, 'DELETE');
        }

        function authenUserV2(aaid) {
            return asyncRequest('/api/v1/auth/' + aaid);
        }

        function getUserV2(id) {
            return asyncRequest('/api/v1/auth/user/' + id);
        }

        function createUserV2(user) {
            return asyncRequest('/api/v1/auth/user', null, user, 'POST');
        }

        function updateUserV2(user) {
            return asyncRequest('/api/v1/auth/user/update', null, user, 'POST');
        }
        
        function deleteUserV2(id) {
            var user = JSON.parse('{"firstName": "string","lastName": "string","middleName": "string","nickname": "string","nationalityId": 0,"roleId": 0,"gender": "string","height": 0,"weight": 0,"educationId": 0,"localeId": 0,"dateofBirth": "2017-05-11T16:25:44.545Z","isActive": true,"email": "string","pathtoPhoto": "string","isEnabled": true,"turnedProfessional": "2017-05-11T16:25:44.545Z","fullName": "string","startDate": "2017-05-11T16:25:44.545Z","endDate": "2017-05-11T16:25:44.545Z","amsId": 0,"aadId": "string","teamId": 0,"id": ' + id + '}');
            return asyncRequest('/api/v1/auth/user/delete/' + id, null, user, 'POST');
        }

        function addUserSession(id) {
            var userSession = JSON.parse('{"sessionType": "string","scheduled": "2017-05-04T04:07:16.223Z","location": {"name": "string","address": "string","type": 0,"id": 0},"users": [{"firstName": "Roy","lastName": "Moran","middleName": null,"nickname": "string","nationalityId": 1,"roleId": 1,"gender": null,"height": 0,"weight": 0,"educationId": 0,"localeId": 0,"dateofBirth": "2017-05-04T04:07:16.223Z","isActive": true,"email": "roymorantest@outlook.com","pathtoPhoto": "string","isEnabled": true,"turnedProfessional": "2017-05-04T04:07:16.223Z","fullName": "string","startDate": "2017-05-04T04:07:16.223Z","endDate": "2017-05-04T04:07:16.223Z","amsId": 0,"aadId": "string","teamId": 0,"id": '+id+'}],"id": 0}');

            return asyncRequest('/api/v1/sessions/addusers', null, userSession, 'POST');
        }

        function addUsersToSession(session) {

            return asyncRequest('/api/v1/sessions/addusers', null, session, 'POST');
        }

        // General settings API endpoints
        
        // CRUD Position
        function getAllPositions() {
            return asyncRequest('/api/v1/settings/positions');
        }
        function createPosition(position) {
            return asyncRequest('/api/v1/settings/create/position', null, position, 'POST');
        }
        function updatePosition(position) {
            return asyncRequest('/api/v1/settings/update/position', null, position, 'POST');
        }
        function deletePosition(position) {
            return asyncRequest('/api/v1/settings/delete/position/' + position.id, null, null, 'POST');
        }

        // CRUD Subpostion
        function getAllSubpositions() {
            return asyncRequest('/api/v1/settings/subpositions');
        }
        function createSubpositions(subpositions) {
            return asyncRequest('/api/v1/settings/create/subposition', null, subpositions, 'POST');
        }
        function updateSubposition(subpositions) {
            return asyncRequest('/api/v1/settings/update/subposition', null, subpositions, 'POST');
        }
        function deleteSubposition(subposition) {
            return asyncRequest('/api/v1/settings/delete/subposition/' + subposition.id, null, null, 'POST');
        }

        // CRUD Mesocycle
        function getAllSessionMesocycles() {
            return asyncRequest('/api/v1/settings/mesocycles');
        }
        function createSessionMesocycle(sessionMesocycle) {
            return asyncRequest('/api/v1/settings/create/mesocycle', null, sessionMesocycle, 'POST');
        }
        function updateSessionMesocycle(sessionMesocycle) {

            return asyncRequest('/api/v1/settings/update/mesocycle', null, sessionMesocycle, 'POST');
        }
        function deleteSessionMesocycle(mesocycle) {
            return asyncRequest('/api/v1/settings/delete/mesocycle/' + mesocycle.id, null, null, 'POST');
        }

        // CRUD Microcycle
        function getAllSessionMicrocycles() {
            return asyncRequest('/api/v1/settings/microcycles');
        }
        function createSessionMicrocycle(sessionMicrocycle) {

            return asyncRequest('/api/v1/settings/create/microcycle', null, sessionMicrocycle, 'POST');
        }
        function updateSessionMicrocycle(sessionMicrocycle) {
            return asyncRequest('/api/v1/settings/update/microcycle', null, sessionMicrocycle, 'POST');
        }
        function deleteSessionMicrocycle(microcycle) {
            return asyncRequest('/api/v1/settings/delete/microcycle/' + microcycle.id, null, null, 'POST');
        }
    }

})();