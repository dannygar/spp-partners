/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System.Collections.Generic;

namespace SportsDataLoader.Shared.Interfaces
{
    public interface ITokenizable
    {
        IDictionary<string, TokenizedString> Tokenize();
    }
}