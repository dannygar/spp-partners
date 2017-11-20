/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿
using System;
using System.Linq;
using SportsDataLoader.Model;

namespace SportsDataLoader.FileProcessing.Extensions
{
    public static class SchemaExtensions
    {
        public static bool IsCompatibleWith(this Schema sourceSchema, Schema schema)
        {
            if (sourceSchema == null)
                throw new ArgumentNullException(nameof(sourceSchema));

            if (schema == null)
                throw new ArgumentNullException(nameof(schema));

            return (sourceSchema.SchemaColumns.Count == schema.SchemaColumns.Count) &&
                   (sourceSchema.SchemaColumns.All(sc => schema.SchemaColumns.Any(s => IsCompatibleWith(sc, s))));
        }

        private static bool IsCompatibleWith(SchemaColumn sourceSchemaColumn, SchemaColumn schemaColumn)
        {
            return (schemaColumn.LocalizedNames.Values.Contains(sourceSchemaColumn.Name)) &&
                   (schemaColumn.DataType == sourceSchemaColumn.DataType) &&
                   (schemaColumn.SqlDataType == sourceSchemaColumn.SqlDataType);
        }
    }
}