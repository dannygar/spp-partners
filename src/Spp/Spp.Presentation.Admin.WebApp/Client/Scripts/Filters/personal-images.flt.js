/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.


(function () {
    'use strict';

    angular
        .module('app.filters')
        .filter('personalImages', personalImages);

    personalImages.$inject = ['config'];

    function personalImages(config) {
        return function (images, value, players) {
            var filtered = [];
            var caseInsensitivePlayers = angular.copy(players);
            var caseInsensitiveValue = value
                ? value.toLowerCase()
                : '';

            caseInsensitivePlayers.forEach(nameToLowerCase);

            for (var i = 0; i < images.length; i++) {
                var item = toLowerCaseImage(images[i]);
                isFilteredItem(item) && filtered.push(images[i]);
            }

            return filtered;

            function isFilteredItem(item) {
                var everyone = config.defaultAllActivePlayersTag.toLowerCase();
                var filteredPlayers = caseInsensitivePlayers.filter(filterByName);

                if (filteredPlayers.length < 1) {
                    return false;
                }

                return !caseInsensitiveValue
                    || item.Tags.indexOf(everyone) + 1
                    || item.Tags.some(bindToPlayer);

                function filterByName(player) {
                    return player.LastName.indexOf(caseInsensitiveValue) + 1
                        || player.Name.indexOf(caseInsensitiveValue) + 1;
                }

                function bindToPlayer(tag) {
                    for (var i = 0; i < filteredPlayers.length; i++) {
                        var playerTag = filteredPlayers[i].Name + " " + filteredPlayers[i].LastName;
                        var playerPositionTag = filteredPlayers[i].Position
                            ? filteredPlayers[i].Position.Name.toLowerCase()
                            : null;

                        if (tag === playerPositionTag || tag === playerTag) {
                            return true;
                        }
                    }

                    return false;
                }
            }

            function toLowerCaseImage(image) {
                var output = angular.copy(image);

                if (output.Tags && output.Tags.map) {
                    output.Tags = output.Tags.map(tagToLowerCase);
                }

                return output;

                function tagToLowerCase(tag) {
                    return tag.toLowerCase();
                }
            }

            function nameToLowerCase(player) {
                player.Name = player.Name.toLowerCase();
                player.LastName = player.LastName.toLowerCase();
            }
        };
    }

})();