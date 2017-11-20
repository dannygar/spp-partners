/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;

namespace SportsDataLoader.Model
{
    public class CustomDataValue
    {
        public string StringValue { get; set; }
        public bool? BooleanValue { get; set; }
        public DateTime? DateTimeValue { get; set; }
        public DateTimeOffset? DateTimeOffsetValue { get; set; }
        public double? DoubleValue { get; set; }
        public int? IntegerValue { get; set; }
        public Guid? GuidValue { get; set; }
        public TimeSpan? TimeSpanValue { get; set; }

        public bool HasValue => ((StringValue != null) ||
                                 (BooleanValue.HasValue) ||
                                 (DateTimeValue.HasValue) ||
                                 (DateTimeOffsetValue.HasValue) ||
                                 (DoubleValue.HasValue) ||
                                 (IntegerValue.HasValue) ||
                                 (GuidValue.HasValue) ||
                                 (TimeSpanValue.HasValue));

        public static CustomDataValue FromString(string source)
        {
            return new CustomDataValue { StringValue = source };
        }

        public static CustomDataValue FromBoolean(bool? source)
        {
            return new CustomDataValue { BooleanValue = source };
        }

        public static CustomDataValue FromDateTime(DateTime? source)
        {
            return new CustomDataValue { DateTimeValue = source };
        }

        public static CustomDataValue FromDateTimeOffset(DateTimeOffset? source)
        {
            return new CustomDataValue { DateTimeOffsetValue = source };
        }

        public static CustomDataValue FromDouble(double? source)
        {
            return new CustomDataValue { DoubleValue = source };
        }

        public static CustomDataValue FromInteger(int? source)
        {
            return new CustomDataValue { IntegerValue = source };
        }

        public static CustomDataValue FromGuid(Guid? source)
        {
            return new CustomDataValue { GuidValue = source };
        }

        public static CustomDataValue FromTimeSpan(TimeSpan? source)
        {
            return new CustomDataValue { TimeSpanValue = source };
        }

        public override string ToString()
        {
            return ((StringValue ??
                     (BooleanValue as object) ??
                     (DateTimeValue as object) ??
                     (DoubleValue as object) ??
                     (IntegerValue as object) ??
                     (GuidValue as object) ??
                     TimeSpanValue)?.ToString() ?? base.ToString());
        }
    }
}