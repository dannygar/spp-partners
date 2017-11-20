/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿namespace SportsDataLoader.Model.Enumerations
{
    public enum DataType // Listed in descnding order of specificity.
    {
        Undefined = 0,
        String,
        Double,
        Integer,
        Boolean,
        DateTime,
        DateTimeOffset,
        TimeSpan,
        Guid
    }
}