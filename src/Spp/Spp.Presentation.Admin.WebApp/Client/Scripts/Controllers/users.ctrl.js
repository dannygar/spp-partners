/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
// Copyright (c) Microsoft Corporation. All rights reserved.


(function () {
    'use strict';

    angular
        .module('app.controllers')
        .controller('UsersCtrl', UsersCtrl);

    UsersCtrl.$inject = ['options', '$rootScope', '$state', 'config', 'ResourceService', 'team', 'teams', '$window'];

    function UsersCtrl(options, $rootScope, $state, config, ResourceService, team, teams, $window) {
        console.log("UsersCtrl");
        var team = JSON.parse(localStorage.getItem("currentTeam"));
        var vm = this;
        vm.showActiveUsers = true;
        vm.showNonActiveUsers = false;
        vm.openOnEdit = openOnEdit;
        vm.syncWithAAD = syncWithAAD;
        vm.switchActive = switchActive;
        vm.switchAlerts = switchAlerts;
        vm.teamSelected = teamSelected;
        vm.addUser = addUser;
        vm.users = team.users;
        if (vm.users) {
            vm.users = initializePlayers(team.users);
        } else {
            vm.users = [];
        }
        vm.teamOptions = intializeTeamsList(teams);
                
        return vm;

        function switchActive(user) {
            user.teamId = team.id; 
            updateUser(user, errorCallback);

            function errorCallback() {
                user.Active = !user.Active;
                $rootScope.showAlert();
            }
        }

        function switchAlerts(user) {
            user.teamId = team.id; 
            updateUser(user, errorCallback);

            function errorCallback() {
                user.GetsAlerts = !user.GetsAlerts;
                $rootScope.showAlert();
            }
        }

        function updateUser(user, errorCallback) {
            ResourceService.updateUserV2(user)
                .then(null, errorCallback);
        }

        function addUser() {
            $state.go('app.user-new');
        }

        function openOnEdit(item) {
            $state.go('app.user-detail', {id: item.id, user: item});
        }

        function syncWithAAD() {
            $rootScope.$emit('showSpinner');

            ResourceService.syncWithAAD()
                .then(syncWithAADSuccessCallback, syncWithAADErrorCallback);

            function syncWithAADSuccessCallback() {
                $rootScope.$emit('hideSpinner');
                $state.go($state.current, {}, {reload: true});
            }

            function syncWithAADErrorCallback() {
                $rootScope.$emit('hideSpinner');
                $rootScope.showAlert();
            }
        }

        function initializePlayers(players) {
            players.forEach(prepareUser);

            return players;
        }

        function intializeTeamsList(teams) {
            teams.forEach(prepareTeam);

            return teams;
        }
        function prepareTeam(team) {
            team.Id = team.id;
            team.Name = team.name;
        }
        function prepareUser(user) {
            user.PathToPhoto = user.pathtoPhoto || config.defaultUserPhoto;

            if (user.roleId == 1) {
                user.Role = "Coach";
            }
            else if (user.roleId == 2){
                user.Role = "Player";
            }
            else if (user.roleId == 3) {
                user.Role = "Administrator";
            }
            else {
                user.Role = "";
            }

        }

        function teamSelected() {
            ResourceService.getTeam(vm.id).then(getTeamSuccess, getTeamFailed);

            function getTeamSuccess(response) {
                ResourceService.getTeamPlayers(response.id).then(getTeamPlayersSuccess, getTeamPlayersFailed);

                $rootScope.team = response;
                localStorage.setItem("currentTeam", angular.toJson(response));

                function getTeamPlayersSuccess(response) {
                    $rootScope.team = response;
                    localStorage.setItem("currentTeam", angular.toJson(response));

                    $window.location.reload();

                }

                function getTeamPlayersFailed(response) {
                    $window.location.reload();
                }

            }

            function getTeamFailed(response) {
                $state.go('error');
            }
            
        }
    }

})();
