/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using SportsDataLoader.FileProcessing.Extensions;
using SportsDataLoader.FileProcessing.Interfaces;
using SportsDataLoader.FileProcessing.Models;

namespace SportsDataLoader.FileProcessing.Scrubbers
{
    public class TimeZoneCodeDataTableScrubber : IDataTableScrubber
    {
        private static readonly Dictionary<string, string> timeZoneDictionary =
            new Dictionary<string, string>
            {
                { "ACDT", "+1030" },
                { "ACST", "+0930" },
                { "ADT", "-0300" },
                { "AEDT", "+1100" },
                { "AEST", "+1000" },
                { "AHDT", "-0900" },
                { "AHST", "-1000" },
                { "AST", "-0400" },
                { "AT", "-0200" },
                { "AWDT", "+0900" },
                { "AWST", "+0800" },
                { "BAT", "+0300" },
                { "BDST", "+0200" },
                { "BET", "-1100" },
                { "BST", "-0300" },
                { "BT", "+0300" },
                { "BZT2", "-0300" },
                { "CADT", "+1030" },
                { "CAST", "+0930" },
                { "CAT", "-1000" },
                { "CCT", "+0800" },
                { "CDT", "-0500" },
                { "CED", "+0200" },
                { "CET", "+0100" },
                { "CEST", "+0200" },
                { "CST", "-0600" },
                { "EAST", "+1000" },
                { "EDT", "-0400" },
                { "EED", "+0300" },
                { "EET", "+0200" },
                { "EEST", "+0300" },
                { "EST", "-0500" },
                { "FST", "+0200" },
                { "FWT", "+0100" },
                { "GMT", "GMT" },
                { "GST", "+1000" },
                { "HDT", "-0900" },
                { "HST", "-1000" },
                { "IDLE", "+1200" },
                { "IDLW", "-1200" },
                { "IST", "+0530" },
                { "IT", "+0330" },
                { "JST", "+0900" },
                { "JT", "+0700" },
                { "MDT", "-0600" },
                { "MED", "+0200" },
                { "MET", "+0100" },
                { "MEST", "+0200" },
                { "MEWT", "+0100" },
                { "MST", "-0700" },
                { "MT", "+0800" },
                { "NDT", "-0230" },
                { "NFT", "-0330" },
                { "NT", "-1100" },
                { "NST", "+0630" },
                { "NZ", "+1100" },
                { "NZST", "+1200" },
                { "NZDT", "+1300" },
                { "NZT", "+1200" },
                { "PDT", "-0700" },
                { "PST", "-0800" },
                { "ROK", "+0900" },
                { "SAD", "+1000" },
                { "SAST", "+0900" },
                { "SAT", "+0900" },
                { "SDT", "+1000" },
                { "SST", "+0200" },
                { "SWT", "+0100" },
                { "USZ3", "+0400" },
                { "USZ4", "+0500" },
                { "USZ5", "+0600" },
                { "USZ6", "+0700" },
                { "UT", "-0000" },
                { "UTC", "-0000" },
                { "UZ10", "+1100" },
                { "WAT", "-0100" },
                { "WET", "-0000" },
                { "WST", "+0800" },
                { "YDT", "-0800" },
                { "YST", "-0900" },
                { "ZP4", "+0400" },
                { "ZP5", "+0500" },
                { "ZP6", "+0600" }
            };

        public Task<DataTable> ScrubDataTable(DataTable dataTable, CultureInfo cultureInfo = null)
        {
            if (dataTable == null)
                throw new ArgumentNullException(nameof(DataTable));

            cultureInfo = (cultureInfo ?? CultureInfo.CurrentCulture);

            foreach (var row in dataTable.Rows)
            {
                foreach (var key in row.Where(r => (r.Value.StringValue != null)).Select(r => r.Key))
                {
                    var value = row[key].StringValue.Trim();
                    var timeZones = timeZoneDictionary.Where(tz => (value.EndsWith(tz.Key))).ToList();

                    if (timeZones.Count == 1)
                    {
                        var timeZone = timeZones[0];

                        value = value.Replace(timeZone.Key, timeZone.Value);

                        if ((value.TryParseDateTimeOffset(cultureInfo) ??
                             value.TryParseDateTimeOffset(CultureInfo.InvariantCulture)) != null)
                        {
                            row[key].StringValue = value;
                        }
                    }
                }
            }

            return Task.FromResult(dataTable);
        }
    }
}