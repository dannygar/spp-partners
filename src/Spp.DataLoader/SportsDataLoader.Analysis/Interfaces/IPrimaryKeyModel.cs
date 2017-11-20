/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using SportsDataLoader.Shared;

namespace SportsDataLoader.FileProcessing.Interfaces
{
    public interface IPrimaryKeyModel
    {
        int Id { get; set; }

        bool DoesMatch(TokenizedString tokenizedString);
    }

    public interface IPrimaryKeyModel<T> : IPrimaryKeyModel
    {
        T Model { get; set; }
    }
}