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
        .factory('AuthService', AuthService);

    AuthService.$inject = ['config', '$http'];

    function AuthService(config, $http) {
        var userInfoKey = 'userInfo';
        var tokenKey = 'api_token';

        var restoredToken = localStorage.getItem(tokenKey);

        var service = {
            token: restoredToken
                ? JSON.parse(restoredToken)
                : {access_token: "", token_type: "", expires: new Date()},
            requestToken: requestToken,
            updateToken: updateToken,
            login: login,
            logout: logout,
            getUserInfo: getUserInfo,
            updateApiAccessToken: updateApiAccessToken
        };

        return service;

        function login(userName, password) {
            return requestToken(userName, password)
                .then(loginSuccessCallback, loginErrorCallback);

            function loginSuccessCallback(response) {
                if (response.data && response.data.accountType === 'Administrator') {
                    localStorage.setItem(userInfoKey, JSON.stringify({
                        userName: userName,
                        password: password,
                        userId: response.data.userId,
                        accountType: response.data.accountType
                    }));

                    return true;
                }

                return false;
            }

            function loginErrorCallback() {
                return false;
            }
        }

        function logout() {
            localStorage.removeItem(userInfoKey);
        }

        function getUserInfo() {
            var userInfo = localStorage.getItem(userInfoKey);
            return userInfo ? JSON.parse(userInfo) : null;
        }

        function requestToken(userName, password) {
            if (!userName || !password) {
                throw "userName and password cannot be null";
            }

            var data = 'grant_type=password&userName=' + userName + '&password=' + password;
            return tryToGetToken(data);
        }

        function updateToken() {
            var userInfo = localStorage.getItem(userInfoKey);

            if (!userInfo) {
                return null;
            }

            userInfo = JSON.parse(userInfo);
            var data = 'grant_type=password&userName=' + userInfo.userName + '&password=' + userInfo.password;

            return tryToGetToken(data);
        }

        function tryToGetToken(data) {
            var promise = $http({
                method: 'POST',
                url: config.tokenEndpoint,
                data: data,
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded'
                }
            });

            promise.then(getTokenSuccessCallback, getTokenErrorCallback);

            function getTokenSuccessCallback(response) {
                service.token = {
                    access_token: response.data.access_token,
                    token_type: response.data.token_type,
                    expires: new Date(response.data['.expires'])
                };

                localStorage.setItem(tokenKey, JSON.stringify(service.token));
            }

            function getTokenErrorCallback(response) {

            }

            return promise;
        }

        function updateApiAccessToken() {
            var data = {
                aadInstance: config.aadInstance_b2b,
                tenant: config.tenant_b2b,
                audience: config.audience_b2b,
                clientId: config.clientId_b2b,
                clientKey: config.clientKey_b2b
            }
            var promise = $http({
                    method: 'POST',
                    url: config.apiTokenEndpoint,
                    params: null,
                    data: data,
                    headers: {'Content-Type': 'application/json'}
                });

            promise.then(getTokenSuccessCallback, getTokenErrorCallback);

            function getTokenSuccessCallback(response) {
                service.token = {
                    access_token: response.data.token,
                    token_type: 'Bearer',
                    expires: 'Expiration'
                };
                localStorage.setItem(tokenKey, JSON.stringify(service.token));
            }

            function getTokenErrorCallback(response) {

            }

            return promise;

        }
    }

})();