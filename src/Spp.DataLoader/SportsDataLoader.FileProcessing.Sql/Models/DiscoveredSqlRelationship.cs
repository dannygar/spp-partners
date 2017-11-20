/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using System.Collections.Generic;
using SportsDataLoader.FileProcessing.Models;
using SportsDataLoader.Model;

namespace SportsDataLoader.FileProcessing.Sql.Models
{
    public class DiscoveredSqlRelationship
    {
        public DiscoveredSqlRelationship()
        {
            RelatedRecordIds = new Dictionary<Guid, int>();
        }

        public DiscoveredSqlRelationship(DiscoveredRelationship possibleRelationship, Schema fkSchema)
        {
            if (possibleRelationship == null)
                throw new ArgumentNullException(nameof(possibleRelationship));

            if (fkSchema == null)
                throw new ArgumentNullException(nameof(fkSchema));

            RelationshipTableName = $"{fkSchema.SchemaName}_{possibleRelationship.PrimaryKeyType.Name}";
            PrimaryKeyColumnName = $"{possibleRelationship.PrimaryKeyType.Name}_Id";
            ForeignKeyColumnName = $"{fkSchema.SchemaName}_Id";

            RelatedRecordIds = possibleRelationship.RelatedRecordIds;
        }

        public string RelationshipTableName { get; set; }
        public string PrimaryKeyColumnName { get; set; }
        public string ForeignKeyColumnName { get; set; }

        public Dictionary<Guid, int> RelatedRecordIds { get; set; }
    }
}