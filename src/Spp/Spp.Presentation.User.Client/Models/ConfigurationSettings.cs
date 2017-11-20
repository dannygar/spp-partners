/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;

namespace MicrosoftSportsScience.Models
{
    [Serializable]
    public class ConfigurationSettings
    {
        public string APIEndpointUrl { get; set; }
        public string ClientId { get; set; }
        public string TenantId { get; set; }

        public string MLEndpointUrl { get; set; }
        public string MLClientKey { get; set; }

        public DateTime SessionDate { get; set; }
    }
}