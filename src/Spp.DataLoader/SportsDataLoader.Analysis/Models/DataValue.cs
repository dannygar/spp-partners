/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using SportsDataLoader.Model.Enumerations;

namespace SportsDataLoader.FileProcessing.Models
{
    public class DataValue
    {
        public string StringValue { get; set; }
        public bool? BooleanValue { get; set; }
        public DateTime? DateTimeValue { get; set; }
        public DateTimeOffset? DateTimeOffsetValue { get; set; }
        public double? DoubleValue { get; set; }
        public Guid? GuidValue { get; set; }
        public int? IntegerValue { get; set; }
        public TimeSpan? TimeSpanValue { get; set; }

        public DataType DataType { get; set; }

        public bool HasValue => ((StringValue != null) ||
                                 (BooleanValue != null) ||
                                 (DateTimeValue != null) ||
                                 (DateTimeOffsetValue != null) ||
                                 (DoubleValue != null) ||
                                 (GuidValue != null) ||
                                 (IntegerValue != null) ||
                                 (TimeSpanValue != null));

        public override string ToString()
        {
            if (StringValue != null)
                return StringValue;

            if (BooleanValue != null)
                return BooleanValue.ToString();

            if (DateTimeValue != null)
                return DateTimeValue.ToString();

            if (DateTimeOffsetValue != null)
                return DateTimeOffsetValue.ToString();

            if (DoubleValue != null)
                return DoubleValue.ToString();

            if (GuidValue != null)
                return GuidValue.ToString();

            if (IntegerValue != null)
                return IntegerValue.ToString();

            if (TimeSpanValue != null)
                return TimeSpanValue.ToString();

            return base.ToString();
        }

        public static DataValue FromString(string source)
        {
            return new DataValue { DataType = DataType.String, StringValue = source };
        }

        public static DataValue FromBoolean(bool? source)
        {
            return new DataValue { DataType = DataType.Boolean, BooleanValue = source };
        }

        public static DataValue FromDateTime(DateTime? source)
        {
            return new DataValue { DataType = DataType.DateTime, DateTimeValue = source };
        }

        public static DataValue FromDateTimeOffsetValue(DateTimeOffset? source)
        {
            return new DataValue { DataType = DataType.DateTimeOffset, DateTimeOffsetValue = source };
        }

        public static DataValue FromDouble(double? source)
        {
            return new DataValue { DataType = DataType.Double, DoubleValue = source };
        }

        public static DataValue FromGuid(Guid? source)
        {
            return new DataValue { DataType = DataType.Guid, GuidValue = source };
        }

        public static DataValue FromInteger(int? source)
        {
            return new DataValue { DataType = DataType.Integer, IntegerValue = source };
        }

        public static DataValue FromTimeSpan(TimeSpan? source)
        {
            return new DataValue { DataType = DataType.TimeSpan, TimeSpanValue = source };
        }
    }
}