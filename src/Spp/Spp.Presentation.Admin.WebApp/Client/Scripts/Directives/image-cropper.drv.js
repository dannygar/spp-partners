/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿// -----------------------------------------------------------------------
// <copyright file="image-cropper.drv.js" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

(function () {
    'use strict';

    angular
        .module('app.directives')
        .directive('imageCropper', imageCropper);

    imageCropper.$inject = ['$timeout', 'config', 'azureBlob'];

    function imageCropper($timeout, config, azureBlob) {
        var directive = {
            restrict: 'E',
            require: '^modalView',
            scope: {
                src: '=',
                data: '=',
                class: '@',
                originalImageData: '=',
                fileObject: '='
            },
            link: link,
            templateUrl: '/Client/Scripts/Directives/Views/image-cropper.html'
        };

        return directive;

        function link(scope, element, attrs, modalCtrl) {
            var image = element.find('#cropper-preview');

            var options = {
                aspectRatio: 1,
                autoCropArea: 1,
                dragMode: 'move',
                toggleDragModeOnDblclick: false,
                crop: crop,
                center: false,
                guides: false
            };

            scope.reset = reset;
            scope.remove = remove;

            $timeout(setModalEventHandlers, 0);

            // Import image
            var inputImage = element.find('#inputImage');
            var URL = window.URL || window.webkitURL;
            var blobURL;

            if (URL) {
                inputImage.change(changeImage);
            } else {
                inputImage.prop('disabled', true).parent().addClass('disabled');
            }

            // Initial values
            getImageDataByUri(scope.src, saveInfoAboutImage);

            function setModalEventHandlers() {
                var modal = modalCtrl.getModalView();

                modal.on('shown.bs.modal', function () {
                    initializeDataIfNull();

                    getImageDataByUri(scope.src, saveInfoAboutImage);

                    image.cropper(options);
                    saveInfoAboutCroppedArea(image.cropper('getData'));
                });

                modal.on('hidden.bs.modal', function () {
                    $timeout(function () {
                        image.cropper('destroy');

                        image.attr('src', function () {
                            return image.attr('ng-src');
                        });

                        scope.$apply();
                    });
                });
            }

            function crop(e) {
                saveInfoAboutCroppedArea(e);
            }

            function reset() {
                image.cropper('reset');
            }

            function remove() {
                image.cropper('replace', config.defaultUserPhoto);
                image.cropper('clear');
                image.cropper('disable');

                scope.data = null;
            }

            function changeImage() {
                var files = this.files;
                var file;

                if (files && files.length) {
                    file = files[0];
                    setFile(file);

                    initializeDataIfNull();

                    if (/^image\/\w+$/.test(file.type)) {
                        blobURL = URL.createObjectURL(file);

                        image.one('built.cropper', onCropperBuilt)
                            .cropper('enable')
                            .cropper('replace', blobURL);

                        getImageDataByUri(blobURL, saveInfoAboutImage);
                        inputImage.val('');
                    } else {
                        window.alert('Please select an image file.');
                    }
                }

                function onCropperBuilt() {
                    URL.revokeObjectURL(blobURL);
                    image.cropper('reset');
                }
            }

            function saveInfoAboutCroppedArea(data) {
                if (scope.data) {
                    scope.data.cropArea = {};
                    scope.data.cropArea.x = Math.round(data.x);
                    scope.data.cropArea.y = Math.round(data.y);
                    scope.data.cropArea.width = Math.round(data.width);
                    scope.data.cropArea.height = Math.round(data.height);
                }
            }

            function saveInfoAboutImage(image, mimeType) {
                if (scope.data) {
                    scope.data.image = image;
                    scope.data.mimeType = mimeType;
                }
            }

            function getImageDataByUri(url, callback) {
                var image = new Image();
                image.crossOrigin = 'anonymous';

                var mimeType = 'image/png';

                image.onload = onImageLoad;
                image.src = url;

                function onImageLoad() {
                    var canvas = document.createElement('canvas');
                    canvas.width = this.naturalWidth;
                    canvas.height = this.naturalHeight;

                    setOriginalImageData(image);

                    canvas.getContext('2d').drawImage(this, 0, 0);

                    var base64Image = canvas.toDataURL(mimeType).replace(/^data:image\/(png|jpg);base64,/, '');
                    callback(base64Image, mimeType);
                }
            }

            function initializeDataIfNull() {
                if (!scope.data) {
                    scope.data = {};
                    scope.$apply();
                }
            }

            function setOriginalImageData(originalImage) {
                scope.originalImageData = {
                    naturalWidth: originalImage.naturalWidth,
                    naturalHeight: originalImage.naturalHeight
                };

                scope.$apply();
            }

            function setFile(file) {

                scope.fileObject = file;
                
                scope.$apply();
            }
        }
    }

})();