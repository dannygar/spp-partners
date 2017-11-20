/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SportsDataLoader.FileProcessing.Interfaces;
using SportsDataLoader.FileProcessing.Models;
using SportsDataLoader.Model;
using SportsDataLoader.Shared.Extensions;

namespace SportsDataLoader.FileProcessing.Discoverers
{
    public class RelationshipDiscoverer<T> : IRelationshipDiscoverer
    {
        public const double MinFkMatchPct = .75;

        private readonly IPrimaryKeyModelProvider<T> pkModelProvider;

        public RelationshipDiscoverer(IPrimaryKeyModelProvider<T> pkModelProvider)
        {
            this.pkModelProvider = pkModelProvider;
        }

        public async Task<DiscoveredRelationship> TryToDiscoverRelationshipAsync(
            IEnumerable<CustomDataEntity> fkEntities,
            string tenantId)
        {
            var pkModels = (await pkModelProvider.GetPrimaryKeyModels(tenantId)).ToList();
            var fkModels = fkEntities.Select(e => new CustomDataForeignKeyModel<T>(e)).ToList();

            IdentifyPossibleForeignKeys(pkModels, fkModels);

            return ToPossibleRelationship(fkModels);
        }

        private void IdentifyPossibleForeignKeys(IList<IPrimaryKeyModel<T>> pkModels,
                                                 IList<CustomDataForeignKeyModel<T>> fkModels)
        {
            for (var fi = 0; fi < fkModels.Count; fi++)
            {
                var fkModel = fkModels[fi];

                for (var pi = 0; pi < pkModels.Count; pi++)
                {
                    var pkModel = pkModels[pi];

                    foreach (var fkPropertyKey in fkModel.Properties.Keys)
                    {
                        var fkPropertyValue = fkModel.Properties[fkPropertyKey];

                        if (pkModel.DoesMatch(fkPropertyValue))
                        {
                            if (fkModel.PropertyMatches.ContainsKey(fkPropertyKey) == false)
                                fkModel.PropertyMatches[fkPropertyKey] = new List<IPrimaryKeyModel<T>>();

                            fkModel.PropertyMatches[fkPropertyKey].Add(pkModel);
                        }
                    }
                }
            }
        }

        private DiscoveredRelationship ToPossibleRelationship(IEnumerable<CustomDataForeignKeyModel<T>> fkEntities)
        {
            var fkEntityList = fkEntities.ToList();

            // Group matches by foreign key field.

            var fkPropertyGroups = fkEntities
                .SelectMany(e => e.PropertyMatches.Keys)
                .Group()
                .ToList();

            // Get rid of outlier foreign key fields.

            var fkPropertyList = fkPropertyGroups
                .Where(g => ((((double) (g.Count()))) / fkEntityList.Count) >= MinFkMatchPct)
                .Select(g => g.Key)
                .ToList();

            var relationship = new PossibleRelationship<T>();

            foreach (var fkEntity in fkEntityList)
            {
                // Get all confired foreign key matches.

                var fkMatches = fkEntity.PropertyMatches
                                        .Where(m => fkPropertyList.Contains(m.Key))
                                        .SelectMany(m => m.Value)
                                        .ToList();

                // Get unique primary key Ids.

                var uniquePkIds = fkMatches.Select(m => m.Id)
                                           .Distinct()
                                           .ToList();

                // If there is only one match, we have a possibly related record.

                if (uniquePkIds.Count == 1)
                    relationship.RelatedRecordIds.Add(fkEntity.CustomDataEntity.EntityId, uniquePkIds[0]);
            }

            // If we found any related records, we can assume that there is a relatioship.
            // If not, there probably isn't one (yet).

            return (relationship.RelatedRecordIds.Any() ? relationship : null);
        }
    }
}