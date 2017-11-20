/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using System.Globalization;

namespace SportsDataLoader.Web.Interfaces
{
    public interface IWebConfiguration
    {
        CultureInfo DefaultCulture { get; }
        Guid DefaultTenantId { get; }
    }
}