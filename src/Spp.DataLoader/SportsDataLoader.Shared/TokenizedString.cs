/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using System.Collections.Generic;
using SportsDataLoader.Shared.Extensions;

namespace SportsDataLoader.Shared
{
    public class TokenizedString
    {
        public TokenizedString(object source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            var sourceString = source.ToString();

            OriginalString = sourceString;
            Tokens = new List<string>();

            if (string.IsNullOrEmpty(sourceString) == false)
                Tokens.AddRange(sourceString.Split(c => (char.IsLetterOrDigit(c) == false)));
        }

        public string OriginalString { get; }
        public List<string> Tokens { get; }

        public override string ToString()
        {
            return (OriginalString ?? base.ToString());
        }
    }
}