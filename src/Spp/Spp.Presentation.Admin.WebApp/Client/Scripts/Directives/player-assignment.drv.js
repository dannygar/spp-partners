/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿// -----------------------------------------------------------------------
// <copyright file="player-assignment.drv.js" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

(function () {
    'use strict';

    angular
        .module('app.directives')
        .directive('playerAssignment', playerAssignment);

    function playerAssignment() {
        var directive = {
            restrict: 'E',
            scope: {
                allSquads: '=',
                allPlayers: '=',
                session: '=',
                disabled: '='
            },
            link: link,
            templateUrl: '/Client/Scripts/Directives/Views/player-assignment.html'
        };

        return directive;

        function link(scope, element, attrs) {
            var defaultGroupName = 'No Position';

            scope.players = scope.allPlayers;

            scope.addAll = addAll;
            scope.removeAll = removeAll;
            scope.transferFromAvailablePlayersToSession = transferFromAvailablePlayersToSession;
            scope.transferFromSessionToAvailablePlayers = transferFromSessionToAvailablePlayers;

            setPlayerAndSessionGroups();

            function addAll() {
                if(!scope.disabled) {
                    scope.players.forEach(addToSession);

                    scope.players = [];

                    setPlayerAndSessionGroups();
                }
            }

            function removeAll() {
                if(!scope.disabled) {
                    scope.session.players.forEach(addToAvailablePlayers);

                    scope.session.playerIds = [];
                    scope.session.players = [];

                    setPlayerAndSessionGroups();
                }
            }

            function transferFromAvailablePlayersToSession(player) {
                if(!scope.disabled) {
                    addToSession(player);
                    removeFromAvailablePlayers(player);

                    setPlayerAndSessionGroups();
                }
            }

            function transferFromSessionToAvailablePlayers(player) {
                if(!scope.disabled) {
                    removeFromSession(player);
                    addToAvailablePlayers(player);

                    setPlayerAndSessionGroups();
                }
            }

            function addToAvailablePlayers(player) {
                scope.players.push(player);
            }

            function removeFromAvailablePlayers(player) {
                scope.players.splice(scope.players.indexOf(player), 1);
            }

            function addToSession(player) {
                scope.session.playerIds.push(player.Id);
                scope.session.players.push(player);
            }

            function removeFromSession(player) {
                scope.session.playerIds.splice(scope.session.playerIds.indexOf(player.Id), 1);
                scope.session.players.splice(scope.session.players.indexOf(player), 1);
            }

            function setPlayerAndSessionGroups() {
                scope.groups = getGroups(scope.players);
                scope.sessionGroups = getGroups(scope.session.players);
            }

            function getGroups(players) {
                var groups = {};

                players.forEach(addPlayerToGroup);

                var sortedGroups = {};
                Object.keys(groups)
                    .sort(sortGroups)
                    .forEach(function (key) {
                        sortedGroups[key] = groups[key];
                    });

                return sortedGroups;

                function addPlayerToGroup(player) {
                    var groupName = player.Position
                        ? player.Position.Name
                        : defaultGroupName;

                    if (!groups[groupName]) {
                        groups[groupName] = [];
                    }

                    groups[groupName].push(player);
                }
            }

            function sortGroups(key1, key2) {
                if (key1 === defaultGroupName) return 1;
                if (key2 === defaultGroupName) return -1;

                if (key1 < key2) return -1;
                if (key1 > key2) return 1;
                return 0;
            }
        }
    }

})();