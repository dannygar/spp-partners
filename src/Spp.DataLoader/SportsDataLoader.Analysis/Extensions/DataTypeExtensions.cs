/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using SportsDataLoader.Model.Enumerations;

namespace SportsDataLoader.FileProcessing.Extensions
{
    public static class DataTypeExtensions
    {
        public static string ToSqlDataTypeName(this DataType dataType)
        {
            switch (dataType)
            {
                case DataType.Boolean:
                    return "bit";
                case DataType.DateTime:
                    return "datetime2";
                case DataType.Guid:
                    return "uniqueidentifier";
                case DataType.DateTimeOffset:
                    return "datetimeoffset";
                case DataType.Double:
                case DataType.TimeSpan:
                    return "float";
                case DataType.Integer:
                    return "int";
                default:
                    return "nvarchar(max)";
            }
        }
    }
}