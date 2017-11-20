/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using SportsDataLoader.FileProcessing.Extensions;
using SportsDataLoader.FileProcessing.Models;
using SportsDataLoader.Model;
using SportsDataLoader.Model.Enumerations;

namespace SportsDataLoader.FileProcessing.Normalizers
{
    public class DataTableNormalizer
    {
        private readonly Dictionary<DataType, Func<string, DataValue>> dataTypeParsers;
        private readonly Schema schema;

        public DataTableNormalizer(CultureInfo cultureInfo, Schema schema)
        {
            if (cultureInfo == null)
                throw new ArgumentNullException(nameof(cultureInfo));

            if (schema == null)
                throw new ArgumentNullException(nameof(schema));

            this.schema = schema;

            dataTypeParsers = new Dictionary<DataType, Func<string, DataValue>>
            {
                [DataType.Guid] =
                    s => DataValue.FromGuid(s.TryParseGuid(cultureInfo) ??
                                            s.TryParseGuid(CultureInfo.InvariantCulture)),
                [DataType.Boolean] =
                    s => DataValue.FromBoolean(s.TryParseBoolean(cultureInfo) ??
                                               s.TryParseBoolean(CultureInfo.InvariantCulture)),
                [DataType.DateTime] =
                    s => DataValue.FromDateTime(s.TryParseDateTime(cultureInfo) ??
                                                s.TryParseDateTime(CultureInfo.InvariantCulture)),
                [DataType.DateTimeOffset] =
                    s => DataValue.FromDateTimeOffsetValue(s.TryParseDateTimeOffset(cultureInfo) ??
                                                           s.TryParseDateTimeOffset(CultureInfo.InvariantCulture)),
                [DataType.Double] =
                    s => DataValue.FromDouble(s.TryParseDouble(cultureInfo) ??
                                              s.TryParseDouble(CultureInfo.InvariantCulture)),
                [DataType.Integer] =
                    s => DataValue.FromInteger(s.TryParseInteger(cultureInfo) ??
                                               s.TryParseInteger(CultureInfo.InvariantCulture)),
                [DataType.TimeSpan] =
                    s => DataValue.FromTimeSpan(s.TryParseTimeSpan(cultureInfo) ??
                                                s.TryParseTimeSpan(CultureInfo.InvariantCulture))
            };
        }

        public DataTable Normalize(DataTable dataTable)
        {
            dataTable = NormalizeColumnNames(dataTable);
            dataTable = NormalizeRowValues(dataTable);

            return dataTable;
        }

        private DataTable NormalizeColumnNames(DataTable dataTable)
        {
            for (var ri = 0; ri < dataTable.Rows.Count; ri++)
            {
                var row = dataTable.Rows[ri];
                var columnKeys = row.Keys.ToList();

                foreach (var key in columnKeys)
                {
                    var column = schema.SchemaColumns.SingleOrDefault(c => (c.Name == key));

                    if (column == null)
                    {
                        column = schema.SchemaColumns.Single(c => c.LocalizedNames.ContainsValue(key));

                        row.Add(column.Name, row[key]);
                        row.Remove(key);
                    }
                }
            }

            return dataTable;
        }

        private DataTable NormalizeRowValues(DataTable dataTable)
        {
            foreach (var column in schema.SchemaColumns)
            {
                for (var ri = 0; ri < dataTable.Rows.Count; ri++)
                {
                    var row = dataTable.Rows[ri];

                    if ((row.ContainsKey(column.Name)) &&
                        (row[column.Name].DataType != column.DataType))
                    {
                        var columnDataType = column.DataType;
                        var columnValue = row[column.Name];

                        if ((columnDataType == DataType.String) ||
                            (columnDataType == DataType.Undefined))
                        {
                            row[column.Name] = DataValue.FromString(columnValue.ToString());
                        }
                        else
                        {
                            row[column.Name] = dataTypeParsers[columnDataType](columnValue.ToString());
                        }
                    }
                }
            }

            return dataTable;
        }
    }
}