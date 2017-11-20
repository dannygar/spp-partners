/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿// -----------------------------------------------------------------------
// <copyright file="personal-image-gallery.drv.js" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

(function () {
    'use strict';

    angular
        .module('app.directives')
        .directive('personalImageGallery', personalImageGallery);

    personalImageGallery.$inject = ['$rootScope', 'ResourceService', 'UtilsService', 'config', 'azureBlob', '$window'];

    function personalImageGallery($rootScope, ResourceService, UtilsService, config, azureBlob, $window) {
        var directive = {
            restrict: 'E',
            scope: {
                images: '=',
                players: '='
            },
            link: link,
            templateUrl: '/Client/Scripts/Directives/Views/personal-image-gallery.html'
        };
        return directive;

        function link(scope, element, attrs) {
            /* Directive uses this schema of image object to upload on server. */
            scope.data = {
                Id: '',
                Tags: '',
                File: ''
            };
            scope.selectedImage = null;
            scope.showImagePreview = false;
            scope.showImageError = false;
            scope.personalImageSearch = "";

            scope.numberOfPlayerPersonalImages = numberOfPlayerPersonalImages;
            scope.contextMenuOptions = contextMenuOptions;
            scope.showAddImageModal = showAddImageModal;
            scope.showEditImageModal = showEditImageModal;
            scope.saveNewImage = saveNewImage;
            scope.saveEditedImage = saveEditedImage;
            scope.removeImageFromServer = removeImageFromServer;
            scope.clearAddImageModal = clearAddImageModal;
            scope.clearImageFields = clearImageFields;

            function numberOfPlayerPersonalImages(player) {
                return scope.images.filter(playerImages).length;

                function playerImages(image) {
                    var playerPersonalTag = (player.Name + " " + player.LastName).toLowerCase();
                    var everyone = config.defaultAllActivePlayersTag.toLowerCase();
                    var playerPositionTag = player.Position
                        ? player.Position.Name.toLowerCase()
                        : '';

                    return image.Tags.indexOf(playerPersonalTag) + 1
                        || image.Tags.indexOf(playerPositionTag) + 1
                        || image.Tags.indexOf(everyone) + 1;
                }
            }

            function contextMenuOptions(item) {
                return [
                    ["Delete", function () {
                        $('#deletePersonalImgModal').modal('show');
                        scope.selectedImage = item;
                    }]
                ];
            }

            function showAddImageModal() {
                var imageInput = $('#personal-image-input');
                imageInput.change(onChange);
            }

            function showEditImageModal(image) {
                scope.selectedImage = image;
            }

            function saveNewImage(modal) {
                scope.showImageError = false;

                if (!scope.data.File) {
                    scope.showImageError = true;
                    return;
                }

                $rootScope.$emit('showSpinnerInImageModal');
                var imageTimeStamp = (new Date).getTime();
                var imageURL = config.blob_baseUrl + imageTimeStamp + '.png';

                var upload_config = {
                    baseUrl: imageURL,
                    sasToken: config.blob_sasToken, 
                    file: scope.data.File,
                    progress: null,
                    complete: savePhotoSuccessCallback,
                    error: savePhotoErrorCallback 
                };

                $rootScope.$emit('showSpinnerInImageModal');
                azureBlob.upload(upload_config);
                function savePhotoSuccessCallback() {
                    var imageBuild = { url: imageURL, tags: "Motivational", sportId: 1, id: 0 }
               
                ResourceService.createPersonalImage(imageBuild);

                    $rootScope.$emit('hideSpinnerInImageModal');
                    var result = { Name: "", url: imageURL };
                    scope.images.push(result);
                    
                    modal.modal('hide');

                    $window.location.reload();
                }
                function savePhotoErrorCallback() {
                    hideSpinnerAndShowPopover('hideSpinnerInPhotoModal', 'showPopoverInPhotoModal');
                }
            }

            function saveEditedImage(modal) {
                scope.showImageError = false;

                if (!scope.selectedImage || !scope.selectedImage.Tags) {
                    scope.showImageError = true;
                    return;
                }

                $rootScope.$emit('showSpinnerInImageModal');

                var data = new FormData();
                data.append('tags', scope.selectedImage.Tags.toString().split(',').map(Function.prototype.call, String.prototype.trim));

                var promise = ResourceService.updatePersonalImage(scope.selectedImage.Id, data);

                promise.then(updateSuccessCallback, errorCallback);

                function updateSuccessCallback(result) {
                    if (result[0]) {
                        scope.selectedImage.Tags = result[0].Tags;
                    }

                    $rootScope.$emit('hideSpinnerInImageModal');
                    modal.modal('hide');
                }
            }

            function removeImageFromServer(modal) {
                $rootScope.$emit('showSpinnerInDeleteImgModal');

                ResourceService.deletePersonalImage(scope.selectedImage.id)
                    .then(removeImageSuccessCallback, removeImageErrorCallback);

                function removeImageSuccessCallback(result) {
                    scope.images = scope.images.filter(function (item) {
                        return item.id != scope.selectedImage.id;
                    });

                    $rootScope.$emit('hideSpinnerInDeleteImgModal');
                    modal.modal('hide');
                }

                function removeImageErrorCallback() {
                    hideSpinnerAndShowPopover('hideSpinnerInDeleteImgModal', 'showPopoverInDeleteImgModal');
                }
            }

            function clearAddImageModal() {
                $rootScope.$emit('hidePopoverOnUploadButton');

                var fileInput = element.find("#personal-image-input");
                fileInput.replaceWith(fileInput.clone()); // To reset file input.
                element.find('.input-group').find(':text').val('');

                scope.showImagePreview = false;
                clearImageFields();
            }

            function clearImageFields() {
                scope.data.Id = '';
                scope.data.Tags = '';
                scope.data.File = '';

                scope.showImageError = false;
            }

            function onChange() {
                if (!this.files || !this.files.length) return;

                var file = this.files[0];

                $rootScope.$emit('hidePopoverOnUploadButton');

                clearImageFields();

                var fileName = $(this).val()
                    .replace(/\\/g, '/')
                    .replace(/.*\//, '');
                var textInput = element.find('.input-group').find(':text');

                textInput.val(fileName);

                if (!/^image\/\w+$/.test(file.type)) {
                    scope.showImageError = true;
                    scope.showImagePreview = false;
                    scope.$apply();
                    return;
                }

                var reader = new FileReader();
                reader.onload = onReaderLoad;
                reader.onerror = onReaderError;
                reader.readAsDataURL(file);

                function onReaderLoad(event) {
                    element.find('[modal-id=addPersonalImageModal] .image-preview').attr('src', event.target.result);

                    scope.data.File = file;

                    scope.showImagePreview = true;
                    scope.$apply();
                }

                function onReaderError(event) {
                    var message = 'An error occurred reading this file.';

                    switch (event.target.error.code) {
                        case event.target.error.NOT_FOUND_ERR:
                            message = 'File not found';
                            break;
                        case event.target.error.NOT_READABLE_ERR:
                            message = 'File is not readable';
                            break;
                        case event.target.error.ABORT_ERR:
                            message = 'The read was aborted';
                            break;
                    }

                    $rootScope.$emit('showPopoverOnUploadButton', {
                        content: message,
                        time: config.errorPopoverTime
                    });
                }
            }

            function errorCallback() {
                hideSpinnerAndShowPopover('hideSpinnerInImageModal', 'showPopoverInImageModal');
            }

            function hideSpinnerAndShowPopover(spinnerHideEvent, popoverEvent) {
                $rootScope.$emit(spinnerHideEvent);
                $rootScope.$emit(popoverEvent, {
                    content: config.defaultErrorMessage,
                    time: config.errorPopoverTime
                });
            }
        }
    }

})();