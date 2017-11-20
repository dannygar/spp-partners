/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using System.Collections.Generic;
using System.Linq;
using SportsDataLoader.FileProcessing.Interfaces;
using SportsDataLoader.Model;
using SportsDataLoader.Shared;
using SportsDataLoader.Shared.Extensions;

namespace SportsDataLoader.FileProcessing.Models
{
    public class CustomDataForeignKeyModel<TPrimaryKey>
    {
        public CustomDataForeignKeyModel()
        {
            Properties = new Dictionary<string, TokenizedString>();
            PropertyMatches = new Dictionary<string, List<IPrimaryKeyModel<TPrimaryKey>>>();
        }

        public CustomDataForeignKeyModel(CustomDataEntity customDataEntity)
        {
            if (customDataEntity == null)
                throw new ArgumentNullException(nameof(customDataEntity));

            CustomDataEntity = customDataEntity;

            Properties = customDataEntity
                .Columns
                .Where(c => (c.Value.StringValue != null) ||
                            (c.Value.GuidValue != null) ||
                            (c.Value.IntegerValue != null))
                .ToDictionary(c => c.Key,
                              c => c.Value.ToString().ToLower().Tokenize());

            PropertyMatches = new Dictionary<string, List<IPrimaryKeyModel<TPrimaryKey>>>();
        }

        public CustomDataEntity CustomDataEntity { get; set; }
        public Dictionary<string, TokenizedString> Properties { get; set; }
        public Dictionary<string, List<IPrimaryKeyModel<TPrimaryKey>>> PropertyMatches { get; set; }
    }
}