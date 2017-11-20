/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿// -----------------------------------------------------------------------
// <copyright file="carousel.drv.js" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

(function () {
    'use strict';

    angular
        .module('app.directives')
        .directive('carousel', carousel);

    carousel.$inject = ['$rootScope', '$timeout', 'UtilsService', 'config', 'azureBlob', 'ResourceService'];

    /* Carousel will be used only once on the page. */
    function carousel($rootScope, $timeout, UtilsService, config, azureBlob, ResourceService) {
        var directive = {
            restrict: 'E',
            scope: {
                images: '=',
                saveAction: '=',
                deleteAction: '='
            },
            link: link,
            templateUrl: '/Client/Scripts/Directives/Views/carousel.html'
        };
        return directive;

        function link(scope, element, attrs) {
            var itemToDelete;

            scope.startCarousel = startCarousel;
            scope.stopCarousel = stopCarousel;

            scope.contextMenuOptions = contextMenuOptions;
            scope.removeImageFromServer = removeImageFromServer;

            /* Directive uses this schema of image object to upload on server. */
            scope.image = {
                Name: '',
                Image: '',
                MimeType: ''
            };
            scope.saveImage = saveImage;
            scope.showImageModal = showImageModal;
            scope.clearImageModal = clearImageModal;

            scope.showImagePreview = false;
            scope.showImageError = false;

            $timeout(scope.startCarousel, 0);

            function contextMenuOptions(item) {
                return [
                    ["Delete", function () {
                        $('#deleteImgModal').modal('show');
                        itemToDelete = item;
                    }]
                ];
            }

            function removeImageFromServer(modal) {
                $rootScope.$emit('showSpinnerInDeleteImgModal');

                scope.deleteAction(itemToDelete)
                    .then(removeImageSuccessCallback, removeImageErrorCallback);

                function removeImageSuccessCallback() {
                    removeImageFromCarousel(itemToDelete);

                    $rootScope.$emit('hideSpinnerInDeleteImgModal');
                    modal.modal('hide');
                }

                function removeImageErrorCallback() {
                    hideSpinnerAndShowPopover('hideSpinnerInDeleteImgModal', 'showPopoverInDeleteImgModal');
                }
            }

            function stopCarousel() {
                element.find('#carousel').carousel('pause');
            }

            function startCarousel() {
                element.find('#carousel').carousel('cycle');
            }

            function scrollCarousel(index) {
                $timeout(function () {
                    element.find('#carousel').carousel(index);
                }, 0);
            }

            function refreshCarousel(index) {
                var carousel = element.find("#carousel");

                carousel.carousel("pause")
                    .removeData();

                if (scope.images.length === index) {
                    index = 0;
                }

                carousel.find('.carousel-inner .item').eq(index).addClass('active');
                scrollCarousel(index);
            }

            function removeImageFromCarousel(item) {
                var index = scope.images.indexOf(item);
                scope.images.splice(index, 1);

                $timeout(refreshCarousel, 0, true, index);
            }

            /* Image add modal function*/

            function showImageModal() {
                scope.stopCarousel();
                var imageInput = element.find('#image-input');
                imageInput.change(onChange);
            }

            function onChange() {
                if (!this.files || !this.files.length) return;

                var file = this.files[0];

                scope.showImageError = false;
                scope.imageFile = this.files[0];
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
                    element.find('#image-preview').attr('src', event.target.result);

                    scope.image.Image = event.target.result.replace(/^.*base64,/i, '');
                    scope.image.MimeType = file.type;

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

            function clearImageModal() {
                $rootScope.$emit('hidePopoverOnUploadButton');

                var imageInput = element.find("#image-input");
                imageInput.replaceWith(imageInput.clone()); // To reset file input.
                element.find('.input-group').find(':text').val('');

                scope.showImagePreview = false;
                scope.showImageError = false;
                clearImageFields();

                scope.startCarousel();
            }

            function clearImageFields() {
                scope.image.Name = '';
                scope.image.Image = '';
                scope.image.MimeType = '';
            }

            function saveImage(modal) {
                var imageTimeStamp = (new Date).getTime();
                var imageURL = config.blob_baseUrl + imageTimeStamp + '.png';

                var upload_config = {
                    baseUrl: imageURL,
                    sasToken: config.blob_sasToken, 
                    file: scope.imageFile,
                    progress: null,
                    complete: savePhotoSuccessCallback,
                    error: savePhotoErrorCallback 
                };

                $rootScope.$emit('showSpinnerInImageModal');
                azureBlob.upload(upload_config);
                var team = JSON.parse(localStorage.getItem("currentTeam"));
                function savePhotoSuccessCallback() {
                    var teamImagesBuild = {
                        teamId: team.id,
                        dateCreated: '2017-03-21T17:57:38.15',
                        dateModified: null,
                        isActive: true,
                        images: [{
                            url: imageURL,
                            tags: 'FacialRecognition',
                            id: 0
                        }]
                    };
                    ResourceService.createFRImages(teamImagesBuild);

                    $rootScope.$emit('hideSpinnerInImageModal');
                    var result = { Name: "", url: imageURL };
                    scope.images.push(result);
                    if (scope.images.length !== 1) {
                        scrollCarousel(scope.images.length - 1);
                    }
                    modal.modal('hide');
                }

                function savePhotoErrorCallback() {
                    hideSpinnerAndShowPopover('hideSpinnerInPhotoModal', 'showPopoverInPhotoModal');
                }

                scope.showImageError = false;

                if (!scope.image.Image) {
                    scope.showImageError = true;
                    return;
                }
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