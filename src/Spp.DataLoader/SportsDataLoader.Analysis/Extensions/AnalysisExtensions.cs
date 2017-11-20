/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using System.Linq;
using SportsDataLoader.Model;
using SportsDataLoader.Model.Enumerations;

namespace SportsDataLoader.FileProcessing.Extensions
{
    public static class AnalysisExtensions
    {
        public static bool DoesConformToSchema(this Analysis analysis, Schema schema)
        {
            if (analysis == null)
                throw new ArgumentNullException(nameof(analysis));

            if (schema == null)
                throw new ArgumentNullException(nameof(schema));

            return (analysis.Columns.Count == schema.SchemaColumns.Count) &&
                   (analysis.Columns.All(ac => schema.SchemaColumns.Any(sc => DoesConformToSchema(ac, sc))));
        }

        private static bool DoesConformToSchema(ColumnAnalysis columnAnalysis, SchemaColumn schemaColumn)
        {
            return (schemaColumn.LocalizedNames.Values.Contains(columnAnalysis.ColumnName)) &&
                   ((columnAnalysis.ColumnDataType == DataType.Undefined) ||
                    (columnAnalysis.PossibleColumnDataTypes.Contains(schemaColumn.DataType)));
        }
    }
}