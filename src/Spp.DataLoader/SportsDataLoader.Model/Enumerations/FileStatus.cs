/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿namespace SportsDataLoader.Model.Enumerations
{
    public enum FileStatus
    {
        Unknown = 0,
        Uploaded,
        Processing,
        Processed,
        ProcessingFailed
    }
}