/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.


(function () {
    'use strict';

    angular
        .module('app.controllers')
        .controller('SettingsCtrl', SettingsCtrl);

    SettingsCtrl.$inject = ['$q', '$rootScope', 'personalImages', 'sessionTypes', 'sessionSubtypes',
        'sessionDayTypes', 'sessionMesocycles', 'sessionMicrocycles', 'playerPositions',
        'playerSubpositions', 'UtilsService', 'ResourceService', 'SettingsService', 'config', 'team', 'ValidationService', 'azureBlob', '$scope'];

    function SettingsCtrl($q, $rootScope, personalImages, sessionTypes, sessionSubtypes,
                          sessionDayTypes, sessionMesocycles, sessionMicrocycles, playerPositions,
                          playerSubpositions, UtilsService, ResourceService, SettingsService, config, team, ValidationService, azureBlob, $scope) {

        console.log("SettingsCtrl");

        SettingsService.initializeData({
            sessionTypes: sessionTypes,
            sessionSubtypes: sessionSubtypes,
            sessionDayTypes: sessionDayTypes,
            sessionMesocycles: sessionMesocycles,
            sessionMicrocycles: sessionMicrocycles,
            playerPositions: playerPositions,
            playerSubpositions: playerSubpositions
        });
       
        var vm = this;

        vm.sessionTypes = sessionTypes;
        vm.sessionSubtypes = sessionSubtypes;
        vm.sessionDayTypes = sessionDayTypes;
        vm.sessionMesocycles = sessionMesocycles;
        vm.sessionMicrocycles = sessionMicrocycles;
        vm.playerPositions = playerPositions;
        vm.playerSubpositions = playerSubpositions;
        
        personalImages = prepareMotivationalImages(personalImages);
        vm.personalImages = personalImages;

        /* Carousel settings */

        vm.deleteFRImageAction = ResourceService.deleteFRImage;
        vm.saveFRImageAction = ResourceService.createFRImages;

        /*Team Settings*/
        vm.createTeam = ResourceService.createTeam;

        /* Other settings */

        vm.showNameError = false;

        vm.currentItem = {};
        vm.currentIndex = {};

        vm.context = {
            itemTypeName: null,
            collection: null,
            action: null,
            useArrayArgument: false,

            parentItemTypeName: null,
            parentCollection: null,
            arrayNameInParents: null,
            updateParentAction: null,

            selectedParents: null
        };

        vm.onAddClick = onAddClick;
        vm.onEditClick = onEditClick;
        vm.onDeleteClick = onDeleteClick;

        vm.executeCreateAction = executeCreateAction;
        vm.executeEditAction = executeEditAction;
        vm.executeDeleteAction = executeDeleteAction;
        var primaryColor = "";
        var secondaryColor = "";
        vm.primaryColor = primaryColor;
        vm.secondaryColor = secondaryColor;
        vm.resetPrimaryColor = team.primaryColor;
        vm.resetSecondaryColor = team.secondaryColor;
        vm.updateTeamColors = updateTeamColors;
        vm.resetTeamColors = resetTeamColors;
        vm.teamLogo = team.pathtoPhoto || config.defaultUserPhoto;
        vm.createTeam = createTeam;
        vm.teamName = "";


        vm.cropperData = {};
        vm.originalImageData = {};
        vm.fileObject;
        vm.newLogo;
        vm.savePhotoChanges = savePhotoChanges;


        vm.colorMessage = 'Save Changes';

        return vm;

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
                vm.user.pathToPhoto = "";
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

            vm.newLogo = imageURL;
            azureBlob.upload(upload_config);

            function savePhotoSuccessCallback() {
                $rootScope.$emit('hideSpinnerInPhotoModal');
                team.pathtoPhoto = vm.newLogo || config.defaultUserPhoto;
                vm.teamLogo = vm.newLogo; 

                ResourceService.updateTeam(team).then(updateTeamSuccessCallback);
                function updateTeamSuccessCallback() {
                    ResourceService.getTeam(team.id).then(getTeamSuccessCallback);
                    function getTeamSuccessCallback(response) {
                        localStorage.setItem("currentTeam", angular.toJson(response));
                    }
                }
                $scope.$emit('teamLogoChanged', { pathToPhoto: vm.newLogo });
                modal.modal('hide');
                
            }

            function savePhotoErrorCallback(error) {
                hideSpinnerAndShowPopover('hideSpinnerInPhotoModal', 'showPopoverInPhotoModal');
            }
        }

        function updateTeamColors() {
            team.primaryColor = vm.primaryColor;
            team.secondaryColor = vm.secondaryColor;

            ResourceService.updateTeam(team).then(updateTeamSuccessCallback);
            function updateTeamSuccessCallback() {
                ResourceService.getTeamPlayers(team.id).then(getTeamSuccessCallback);
                function getTeamSuccessCallback(response) {
                    localStorage.setItem("currentTeam", angular.toJson(response));
                    vm.colorMessage = "Colors Updated";
                    $("#reset-colors-btn").removeClass("disabled");
                }
            }
        }

        function resetTeamColors() {
            team.primaryColor = vm.resetPrimaryColor;
            team.secondaryColor = vm.resetSecondaryColor;

            ResourceService.updateTeam(team).then(updateTeamSuccessCallback);
            function updateTeamSuccessCallback() {
                ResourceService.getTeamPlayers(team.id).then(getTeamSuccessCallback);
                function getTeamSuccessCallback(response) {
                    localStorage.setItem("currentTeam", angular.toJson(response));
                    $(".primary-team-color").css("background-color", vm.resetPrimaryColor);
                    $(".secondary-team-color").css("background-color", vm.resetSecondaryColor);
                    $("#reset-colors-btn").addClass("disabled");
                }
            }
        }
        function onAddClick(itemType) {
            vm.showNameError = false;
            vm.currentItem = {};
            vm.context = SettingsService.initializeAddContext(itemType);
        }

        function onEditClick(itemType, item, index) {
            vm.showNameError = false;

            vm.currentItem = angular.copy(item);
            vm.currentIndex = index;

            vm.context = SettingsService.initializeEditContext(itemType, vm.currentItem);
        }

        function onDeleteClick(itemType, item, event) {
            UtilsService.stopEventPropagation(event);
            $('#deleteModal').modal('show');

            vm.currentItem = item;
            vm.context = SettingsService.initializeDeleteContext(itemType);
        }

        function executeCreateAction(modal) {
            executeCommand(
                modal,
                true,
                'showSpinnerInCreateModal',
                'hideSpinnerInCreateModal',
                'showPopoverInCreateModal',
                action,
                true
            );

            function action(item) {
                vm.context.collection.push(item);
            }
        }

        function executeEditAction(modal) {
            executeCommand(
                modal,
                true,
                'showSpinnerInEditModal',
                'hideSpinnerInEditModal',
                'showPopoverInEditModal',
                action,
                true
            );

            function action(item) {
                vm.context.collection[vm.currentIndex] = item;
            }
        }

        function executeDeleteAction(modal) {
            executeCommand(
                modal,
                false,
                'showSpinnerInDeleteModal',
                'hideSpinnerInDeleteModal',
                'showPopoverInDeleteModal',
                action,
                false
            );

            function action() {
                var index = vm.context.collection.indexOf(vm.currentItem);

                if (index === -1) {
                    return;
                }

                vm.context.collection.splice(index, 1);

                if (vm.context.parentCollection) {
                    SettingsService.removeChildInParentCollection(
                        vm.context.parentCollection,
                        vm.context.arrayNameInParents,
                        vm.currentItem.Id
                    );
                }
            }
        }

        function executeCommand(modal, checkCurrentItemName, spinnerShowEvent, spinnerHideEvent, popoverEvent, action, updateChildren) {
            if (checkCurrentItemName) {
                vm.showNameError = !vm.currentItem.name;

                if (!vm.currentItem.name) {
                    return;
                }
            }

            $rootScope.$emit(spinnerShowEvent);

            var argument = vm.context.useArrayArgument
                ? [vm.currentItem]
                : vm.currentItem;

            vm.context.action(argument).then(successCallback, errorCallback);

            function successCallback(result) {
                result = vm.context.useArrayArgument
                    ? result[0]
                    : result;
                if(parseInt(result)){
                    argument.id = result;
                }
                result = argument;

                action(result);

                if (!vm.context.parentCollection || !updateChildren) {
                    return hideSpinnerAndModal(spinnerHideEvent, modal);
                }

                updateChildrenInParentCollection(result.id)
                   .then(updateChildrenSuccessCallback, updateChildrenErrorCallback);

                function updateChildrenSuccessCallback() {
                    hideSpinnerAndModal(spinnerHideEvent, modal);
                }

                function updateChildrenErrorCallback() {
                    var popoverMessage = 'Something went wrong. Cannot update ' + vm.context.parentItemTypeName +
                        's. Please reload the page and try again.';
                    hideSpinnerAndShowPopover(spinnerHideEvent, popoverEvent, popoverMessage);
                }
            }

            function errorCallback() {
                hideSpinnerAndShowPopover(spinnerHideEvent, popoverEvent);
            }
        }

        function updateChildrenInParentCollection(childID) {
            var deferred = $q.defer();

            // This action can be failed so we should use copied collection.
            var copiedCollection = angular.copy(vm.context.parentCollection);
            var copiedSelectedParents = angular.copy(vm.context.selectedParents);

            UtilsService.replaceItemsInCollectionById(copiedCollection, copiedSelectedParents);

            SettingsService.moveChildToNewParents(copiedCollection, vm.context.arrayNameInParents, childID, copiedSelectedParents);

            UtilsService.executePromiseForCollection(copiedCollection, vm.context.updateParentAction)
                .then(successCallback, deferred.reject);

            return deferred.promise;

            function successCallback() {
                UtilsService.replaceItemsInCollectionById(vm.context.parentCollection, copiedCollection);
                deferred.resolve();
            }
        }

        function hideSpinnerAndModal(spinnerHideEvent, modal) {
            $rootScope.$emit(spinnerHideEvent);
            modal.modal('hide');
        }

        function hideSpinnerAndShowPopover(spinnerHideEvent, popoverEvent, popoverMessage) {
            $rootScope.$emit(spinnerHideEvent);
            $rootScope.$emit(popoverEvent, {
                content: popoverMessage || config.defaultErrorMessage,
                time: config.errorPopoverTime
            });
        }

        function prepareMotivationalImages(images) {

            var motivationalImages = [];

            images.forEach(function (item) {
                if (item.tags === "Motivational") {
                    motivationalImages.push(item);
                }
                
            });
            return motivationalImages;

        }

        function createTeam() {

            var team = {
                name: vm.teamName,
                sportId: 1,
                coachId: 1,
                playerCaptainId: null,
                founded: null,
                isAffiliate: true,
                minAge: null,
                maxAge: null,
                abbreviation: null,
                teamId: null,
                localeId: null,
                isNationalTeam: true,
                pathtoPhoto: null,
                competition: null,
                gender: null,
                groupId: null,
                primaryColor: null,
                secondaryColor: null,
                users: [{}],
                id: 0
            }

            ResourceService.createTeam(team);
        }

        function getFileExtension(fileName) {
            return fileName.split('.').pop().toLowerCase();
        }
    }

})();