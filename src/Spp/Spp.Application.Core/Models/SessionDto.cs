/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;

namespace Spp.Application.Core.Models
{
    public sealed class SessionDto : ModelBase
    {
        public string SessionType { get; set; }
        public DateTime Scheduled { get; set; }
        public LocationDto Location { get; set; }
        public IEnumerable<UserDto> Users { get; set; }
    }
}
