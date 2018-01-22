/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;

namespace Spp.Presentation.User.Client.Data
{
    public class Session
    {
        public int Id { get; set; }
        public string SessionType { get; set; }
        public DateTime Scheduled { get; set; }
        public Location Location { get; set; }
        public IEnumerable<User> Users { get; set; }
    }
}
