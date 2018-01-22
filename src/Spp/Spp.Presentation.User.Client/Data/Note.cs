/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using System;

namespace Spp.Presentation.User.Client.Data
{
    public class Note
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime Created { get; set; }

    }
}
