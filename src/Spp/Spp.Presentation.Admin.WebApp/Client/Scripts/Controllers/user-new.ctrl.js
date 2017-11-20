/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿// Copyright (c) Microsoft Corporation. All rights reserved.


(function () {
	'use strict';

	angular
        .module('app.controllers')
        .controller('UserNewCtrl', UserNewCtrl);

	UserNewCtrl.$inject = ['$rootScope', 'options', '$state',
        '$scope', 'config', 'ResourceService', 'ValidationService', 'UtilsService', 'azureBlob', 'team', 'playerPositions', 'playerSubpositions'];

	function UserNewCtrl($rootScope, options, $state, $scope, config, ResourceService, ValidationService, UtilsService, azureBlob, team, playerPositions, playerSubpositions) {
		console.log("UserNewCtrl");
		var vm = this;

		vm.squadsOptions = options.Squads;
		vm.positionOptions = playerPositions;
		vm.subPositionOptions = playerSubpositions;
		vm.roleOptions = options.Roles;
		vm.genderOptions = options.Genders;
	    vm.dateOfBirth = new Date();

		vm.cropperData = {};
		vm.originalImageData = {};
		vm.fileObject;
		vm.newPhoto;

		vm.createUser = createUser;
		vm.cancel = cancel;
		vm.savePhotoChanges = savePhotoChanges;

		vm.shownWeight = 0;
		vm.shownHeight = 0;
		vm.switchWeightType = switchWeightType;
		vm.switchHeightType = switchHeightType;

		vm.pathToPhoto = config.defaultUserPhoto;

		vm.showNewPasswordError = false;
		vm.showConfirmPasswordError = false;
		vm.showPasswordMismatchError = false;

		return vm;

		function switchWeightType(toggled, currentValue) {
			var poundToKillo = 0.453592;
			return convertType(toggled, currentValue, poundToKillo);
		}

		function switchHeightType(toggled, currentValue) {
			var inchToCm = 2.54;
			return convertType(toggled, currentValue, inchToCm);
		}

		function convertType(toggled, currentValue, origin) {
			return toggled
                ? (currentValue * origin)
                : (currentValue / origin);
		}

		function changePassword(modal) {
			vm.showNewPasswordError = false;
			vm.showConfirmPasswordError = false;
			vm.showPasswordMismatchError = false;

			if (vm.newPassword === '' || vm.confirmedPassword === '') {
				if (vm.newPassword === '') {
					vm.showNewPasswordError = true;
				}

				if (vm.confirmedPassword === '') {
					vm.showConfirmPasswordError = true;
				}
				return;
			}

			if (vm.newPassword !== vm.confirmedPassword) {
				vm.showPasswordMismatchError = true;
				return;
			}

			$rootScope.$emit('showSpinnerInPasswordModal');

			ResourceService.changeUserPassword(vm.user.id, vm.newPassword)
                .then(changePasswordSuccessCallback, changePasswordErrorCallback);

			function changePasswordSuccessCallback() {
				$rootScope.$emit('hideSpinnerInPasswordModal');
				modal.modal('hide');
			}

			function changePasswordErrorCallback() {
				hideSpinnerAndShowPopover(
                    'hideSpinnerInPasswordModal',
                    'showPopoverInPasswordModal',
                    'Something went wrong. Password has not been changed. Please reload the page and try again.'
                );
			}
		}

		function clearPassword() {
			vm.newPassword = '';
			vm.confirmedPassword = '';

			$scope.$apply();
		}

		function savePhotoChanges(modal) {
			var validationMessage = ValidationService.validateUserPhoto(vm.cropperData, vm.originalImageData);
			if (validationMessage) {
				$rootScope.$emit('showPopoverInPhotoModal', {
					content: validationMessage,
					time: config.errorPopoverTime
				});
				return;
			}

			var validationFileMessage = ValidationService.validateUserPhotoExtension(vm.fileObject.name);
			if (validationFileMessage) {
			    $rootScope.$emit('showPopoverInPhotoModal', {
			        content: validationFileMessage,
			        time: config.errorPopoverTime
			    });
			    return;
			}

			$rootScope.$emit('showSpinnerInPhotoModal');

			if (vm.cropperData === null) {
			    $rootScope.$emit('hideSpinnerInPhotoModal');
			    vm.user.pathToPhoto = config.defaultUserPhoto;
			    modal.modal('hide');
			    return;
			}

			var imageURL = config.blob_baseUrl + UtilsService.generateUniqueName() + '.' + getFileExtension(vm.fileObject.name);
			var upload_config = {
			    baseUrl: imageURL,
			    sasToken: config.blob_sasToken, 
			    file: vm.fileObject,
			    progress: null,
			    complete: savePhotoSuccessCallback,
			    error: savePhotoErrorCallback
			};

			vm.newPhoto = imageURL;
			azureBlob.upload(upload_config);


			function savePhotoSuccessCallback() {
				$rootScope.$emit('hideSpinnerInPhotoModal');

				vm.pathToPhoto = vm.newPhoto || config.defaultUserPhoto;

				modal.modal('hide');
			}

			function savePhotoErrorCallback() {
				hideSpinnerAndShowPopover('hideSpinnerInPhotoModal', 'showPopoverInPhotoModal');
			}
		}

		function createUser() {
		    var today = new Date();
		    var year = today.getFullYear();
		    var month = today.getMonth();
		    var day = today.getDate();
		    var todayPlusYear = new Date(year + 1, month, day);

		    $rootScope.$emit('showUserPageSpinner');
		    var user = {
                id: 0,
		        firstName: vm.user.name,
		        lastName: vm.user.lastName,
		        middleName: null,
		        nickname: null,
		        nationalityId: 1,
		        roleId: vm.roleId,
		        gender: vm.user.gender,
		        educationId: 1, 
		        localeId: 1,
		        dateofBirth: vm.dateOfBirth,
		        isActive: vm.user.active,
		        isEnabled: vm.user.getsAlerts,
		        turnedProfessional: new Date(),
		        fullName: vm.user.name + ' ' + vm.user.lastName,
		        startDate: today,
		        endDate: todayPlusYear,
		        teamId: team.id,
		        email: vm.user.email,
		        weight: vm.user.weight,
		        height: vm.user.height,
		        pathtoPhoto: vm.pathToPhoto || config.defaultUserPhoto,
		        amsId: null,
		        aadId: null,
		        playerInfo: {
		            userId: 0,
		            jerseyNumber: vm.user.shirtNumber,
		            positionId: vm.user.positionId,
		            subPositionId: vm.user.subPositionId,
		            playerDepth: 0,
		            dominantSkillId: 1,
		            isWatchlist: true,
		            isResting: false,
		            isInjured: false,
		            availability: null,
		            isKeeper: null,
		            cricket: {
		                bowlingGroup: null,
		                bowlingHand: null,
		                battingGroup: null,
		                battingHand: null
		            }
		        }

		    };


		    ResourceService.createUserV2(user).then(function(response) {
		        ResourceService.addUserSession(response).then(createUserSuccessCallback, createUserErrorCallback);
		    });

		    function createUserSuccessCallback(response) {
		        ResourceService.getTeamPlayers(team.id)
                .then(updateTeamSuccessCallback);
		        function updateTeamSuccessCallback(response) {
		            localStorage.setItem("currentTeam", angular.toJson(response));
		            $rootScope.$emit('hideUserPageSpinner');
		            $state.go('app.users');
		        }
		    }

		    function createUserErrorCallback(result) {
		        $rootScope.$emit('hideUserPageSpinner');

		        result.status === 400
                    ? $rootScope.showAlert('There was an issue creating the player. Please try again later.') 
                    : $rootScope.showAlert();
		    }
		}

		function cancel() {
			$state.go('app.users');
		}

		function retrieveGenderOption(gender) {
			return vm.genderOptions.filter(function (item) {
				return item === gender;
			})[0];
		}

		function initializeUser(user) {
			var result = {
				id: user.id,
				name: user.firstName,
				lastName: user.lastName,
				dateOfBirth: new Date(),
				email: user.email,
				aadEmail: user.email,
				positionId: -1,
				subPositionId: -1,
				shirtNumber: user.ShirtNumber,
				weight: user.weight,
				height: user.height,
				gender: vm.genderOptions[0],
				pathToPhoto: user.pathtoPhoto || config.defaultUserPhoto,
				getsAlerts: user.isEnabled,
				certifiedForAccess: user.CertifiedForAccess, 
				roleIds: null, 
				roleId: user.roleId,
				active: user.isActive,
				squadIds: []
			};

			result.dateOfBirth = user.DateOfBirth
                ? new Date(user.DateOfBirth)
                : result.dateOfBirth;

			result.positionId = user.Position && UtilsService.hasCollectionId(options.Positions, user.Position.Id)
                ? user.Position.Id
                : result.positionId;

			result.subPositionId = user.SubPosition && UtilsService.hasCollectionId(options.SubPositions, user.SubPosition.Id)
                ? user.SubPosition.Id
                : result.subPositionId;

			result.roleIds = user.Roles
                ? user.Roles.map(function (role) {
                	return role.Id
                })
                : result.roleIds;

			result.gender = retrieveGenderOption(user.Gender) || result.gender;

			var userSquads = vm.squadsOptions.filter(isIdInSquadPlayerIds);

			result.squadIds = userSquads.map(function (squad) {
				return squad.Id;
			});

			return result;

			function isIdInSquadPlayerIds(item) {
				return item.PlayerIds.indexOf(result.id) + 1;
			}
		}

		function isActivePlayer(user) {
			var playerRoleID = 2; 
			return (user.roleId == playerRoleID) && user.active
		}

		function addAllPlayersSquad(squads) {
			squads.unshift({
				Id: -1,
				Name: 'All Players'
			});
		}

		function hideSpinnerAndShowPopover(spinnerHideEvent, popoverEvent, popoverMessage) {
			$rootScope.$emit(spinnerHideEvent);
			$rootScope.$emit(popoverEvent, {
				content: popoverMessage || config.defaultErrorMessage,
				time: config.errorPopoverTime
			});
		}

		function getFileExtension(fileName) {
		    return fileName.split('.').pop().toLowerCase();
		}
	}

})();
