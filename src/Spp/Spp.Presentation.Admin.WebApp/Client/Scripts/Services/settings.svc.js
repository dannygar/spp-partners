/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.


(function () {
    'use strict';

    angular
        .module('app.services')
        .factory('SettingsService', SettingsService);

    SettingsService.$inject = ['ResourceService'];

    function SettingsService(ResourceService) {
        var service = {
            initializeData: initializeData,

            initializeAddContext: initializeAddContext,
            initializeEditContext: initializeEditContext,
            initializeDeleteContext: initializeDeleteContext,

            getSessionTypeContext: getSessionTypeContext,
            getSessionSubtypeContext: getSessionSubtypeContext,
            getSessionDayTypeContext: getSessionDayTypeContext,
            getSessionMesocycleContext: getSessionMesocycleContext,
            getSessionMicrocycleContext: getSessionMicrocycleContext,
            getPlayerPositionContext: getPlayerPositionContext,
            getPlayerSubpositionContext: getPlayerSubpositionContext,

            findParentsForChild: findParentsForChild,
            moveChildToNewParents: moveChildToNewParents,
            removeChildInParentCollection: removeChildInParentCollection
        };

        return service;

        function initializeData(data) {
            var keys = Object.keys(data);

            keys.forEach(function(key){
                service[key] = data[key];
            });
        }

        function initializeAddContext(itemType) {
            var context;

            switch (itemType.toLowerCase()) {
                case 'sessiontype':
                    context = service.getSessionTypeContext(ResourceService.createSessionType);
                    break;
                case 'sessionsubtype':
                    context = service.getSessionSubtypeContext(ResourceService.createSessionSubtypes, true, true);
                    context.selectedParents = [context.parentCollection[0]];
                    break;
                case 'sessiondaytype':
                    context = service.getSessionDayTypeContext(ResourceService.createSessionDayType, true);
                    break;
                case 'sessionmesocycle':
                    context = service.getSessionMesocycleContext(ResourceService.createSessionMesocycle);
                    break;
                case 'sessionmicrocycle':
                    context = service.getSessionMicrocycleContext(ResourceService.createSessionMicrocycle);
                    break;
                case 'playerposition':
                    context = service.getPlayerPositionContext(ResourceService.createPosition);
                    break;
                case 'playersubposition':
                    context = service.getPlayerSubpositionContext(ResourceService.createSubpositions);
                    break;
            }

            return context;
        }

        function initializeEditContext(itemType, item) {
            var context;

            switch (itemType.toLowerCase()) {
                case 'sessiontype':
                    context = service.getSessionTypeContext(ResourceService.updateSessionType);
                    break;
                case 'sessionsubtype':
                    context = service.getSessionSubtypeContext(ResourceService.updateSessionSubtype, false, true);
                    context.selectedParents = service.findParentsForChild(context.parentCollection, context.arrayNameInParents, item.Id);
                    break;
                case 'sessiondaytype':
                    context = service.getSessionDayTypeContext(ResourceService.updateSessionDayType, false);
                    break;
                case 'sessionmesocycle':
                    context = service.getSessionMesocycleContext(ResourceService.updateSessionMesocycle, false);
                    break;
                case 'sessionmicrocycle':
                    context = service.getSessionMicrocycleContext(ResourceService.updateSessionMicrocycle, false);
                    break;
                case 'playerposition':
                    context = service.getPlayerPositionContext(ResourceService.updatePosition);
                    break;
                case 'playersubposition':
                    context = service.getPlayerSubpositionContext(ResourceService.updateSubposition, false, false);
                    break;
            }

            return context;
        }

        function initializeDeleteContext(itemType) {
            var context;

            switch (itemType.toLowerCase()) {
                case 'sessiontype':
                    context = service.getSessionTypeContext(ResourceService.deleteSessionType);
                    break;
                case 'sessionsubtype':
                    context = service.getSessionSubtypeContext(ResourceService.deleteSessionSubtype, false, false);
                    break;
                case 'sessiondaytype':
                    context = service.getSessionDayTypeContext(ResourceService.deleteSessionDayType, false);
                    break;
                case 'sessionmesocycle':
                    context = service.getSessionMesocycleContext(ResourceService.deleteSessionMesocycle, false);
                    break;
                case 'sessionmicrocycle':
                    context = service.getSessionMicrocycleContext(ResourceService.deleteSessionMicrocycle, false);
                    break;
                case 'playerposition':
                    context = service.getPlayerPositionContext(ResourceService.deletePosition);
                    break;
                case 'playersubposition':
                    context = service.getPlayerSubpositionContext(ResourceService.deleteSubposition, false, false);
                    break;
            }

            return context;
        }

        function getSessionTypeContext(action) {
            return {
                itemTypeName: 'Session Type',
                collection: service.sessionTypes,
                action: action
            }
        }

        function getSessionSubtypeContext(action, useArrayArgument, includeParentInfo) {
            var context = {
                itemTypeName: 'Session Subtype',
                collection: service.sessionSubtypes,
                action: action,
                useArrayArgument: useArrayArgument,
                parentCollection: service.sessionTypes,
                arrayNameInParents: 'SubTypeIds'
            };

            if (includeParentInfo) {
                context.parentItemTypeName = 'Session Type';
                context.updateParentAction = ResourceService.updateSessionType;
            }

            return context;
        }

        function getSessionDayTypeContext(action, useArrayArgument) {
            return {
                itemTypeName: 'Session Day Type',
                collection: service.sessionDayTypes,
                action: action,
                useArrayArgument: useArrayArgument
            };
        }

        function getSessionMesocycleContext(action, useArrayArgument) {
            return {
                itemTypeName: 'Session Mesocycle',
                collection: service.sessionMesocycles,
                action: action,
                useArrayArgument: useArrayArgument
            };
        }

        function getSessionMicrocycleContext(action, useArrayArgument) {
            return {
                itemTypeName: 'Session Microcycle',
                collection: service.sessionMicrocycles,
                action: action,
                useArrayArgument: useArrayArgument
            };
        }

        function getPlayerPositionContext(action) {
            return {
                itemTypeName: 'Player Position',
                collection: service.playerPositions,
                action: action
            };
        }

        function getPlayerSubpositionContext(action, useArrayArgument, includeParentInfo) {
            var context = {
                itemTypeName: 'Player Subposition',
                collection: service.playerSubpositions,
                action: action
            };

            if (includeParentInfo) {
                context.parentItemTypeName = 'Player Position';
                context.updateParentAction = ResourceService.updatePosition;
            }

            return context;
        }

        function findParentsForChild(parentCollection, childArrayName, childID) {
            var parents = [];

            parentCollection.forEach(function (parent) {
                if (!parent[childArrayName]) {
                    return;
                }

                var children = parent[childArrayName];
                children.forEach(function (child) {
                    if (child === childID) {
                        parents.push(parent);
                    }
                })
            });

            return parents;
        }

        function moveChildToNewParents(parentCollection, childArrayName, childID, newParents) {
            for (var i = 0; i < parentCollection.length; i++) {
                var hasChildID = false;

                for (var j = 0; j < newParents.length; j++) {
                    if (parentCollection[i].Id !== newParents[j].Id) {
                        continue;
                    }

                    if (parentCollection[i][childArrayName].indexOf(childID) === -1) {
                        parentCollection[i][childArrayName].push(childID);
                    }

                    hasChildID = true;
                    break;
                }

                if (!hasChildID) {
                    var childIdIndex = parentCollection[i][childArrayName].indexOf(childID);

                    if (childIdIndex != -1) {
                        parentCollection[i][childArrayName].splice(childIdIndex, 1);
                    }
                }
            }
        }

        function removeChildInParentCollection(parentCollection, childArrayName, childID) {
            parentCollection.forEach(function (parent) {
                var childIdIndex = parent[childArrayName].indexOf(childID);

                if (childIdIndex != -1) {
                    parent[childArrayName].splice(childIdIndex, 1);
                }
            });
        }
    }
})();