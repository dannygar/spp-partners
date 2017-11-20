/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
// Copyright (c) Microsoft Corporation. All rights reserved.


(function () {
    'use strict';

    angular
        .module('app.controllers')
        .controller('LoginCtrl', LoginCtrl);

    LoginCtrl.$inject = ['$state', '$rootScope', 'adalAuthenticationService', 'ResourceService'];

    function LoginCtrl($state, $rootScope, adalService, ResourceService) {
        console.log("LoginCtrl");
  
        var initialState = 'app.users';
        if ($rootScope.userInfo.isAuthenticated) {
            $rootScope.$emit('loginStarted'); 
            (function authenticateUser() {
                var oid = $rootScope.userInfo.profile.oid;
                ResourceService.authenUserV2(oid).then(authenSuccess);

                function authenSuccess(authResonse) {
                    ResourceService.getTeamPlayers(authResonse).then(getTeamSuccess, getTeamFailed);

                    function getTeamSuccess(response) {
                        $rootScope.team = response;
                        localStorage.setItem("currentTeam", angular.toJson(response));
                        var currentUser;
                        response.users.forEach(function (player) {
                            if (player.aadId === $rootScope.userInfo.profile.oid) {
                                currentUser = player;
                            }
                        });
                        localStorage.setItem("currentUser", angular.toJson(currentUser));
                        $state.go(initialState);
                    }

                    function getTeamFailed(response) {
                        $state.go('error');
                    }

                }

            })();
        }
        else {
            adalService.login();
        }
    }

})();
