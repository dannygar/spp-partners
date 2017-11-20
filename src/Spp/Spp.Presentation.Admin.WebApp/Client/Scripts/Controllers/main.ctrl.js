/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
// Copyright (c) Microsoft Corporation. All rights reserved.


(function () {
    'use strict';

    angular
        .module('app.controllers', [])
        .controller('MainCtrl', MainCtrl);

    MainCtrl.$inject = ['$rootScope', '$state', '$scope', 'config', 'AuthService', 'adalAuthenticationService', 'ResourceService', 'team', 'currentUser'];

    function MainCtrl($rootScope, $state, $scope, config, AuthService, adalService, ResourceService, team, currentUser) {
        console.log('MainCtrl');
        var vm = this;
        team = JSON.parse(localStorage.getItem("currentTeam"));
        vm.warningMessage = null;

        vm.footerDate = new Date();

        vm.primaryColor = team.primaryColor;
        vm.secondaryColor = team.secondaryColor;
        vm.currentUser = initializeUser(currentUser);
        
        vm.logout = logout;

        $scope.$on('userPhotoChanged', onUserPhotoChanged);
        $scope.$on('teamLogoChanged', onTeamLogoChanged);

        $rootScope.$on("$viewContentLoaded", function () {
            var selects = $('select');
            selects.css('visibility', 'hidden');
            selects.css('visibility', 'visible');
        });

        $rootScope.showAlert = showAlert;

        return vm;

        function logout() {
            localStorage.removeItem("currentUser");
            localStorage.removeItem("currentTeam");
            adalService.logOut();
        }

        function initializeUser(user) {            
            var result = {
                id: user.id,
                name: user.firstName,
                lastName: user.lastName,
                pathToPhoto: user.pathtoPhoto || config.defaultUserPhoto,
                teamLogo: team.pathtoPhoto
            };
            
            return result;
        }

        function onUserPhotoChanged(event, data) {
            if (vm.currentUser.id === data.userId) {
                vm.currentUser.pathToPhoto = data.pathToPhoto;
            }
        }

        function onTeamLogoChanged(event, data) {
            vm.currentUser.teamLogo = data.pathToPhoto;
        }

        function showAlert(message) {
            vm.warningMessage = message || config.defaultErrorMessage;
            $('#alert').modal('show');
        }
    }

})();
