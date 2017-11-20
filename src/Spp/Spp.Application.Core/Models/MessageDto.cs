/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;

namespace Spp.Application.Core.Models
{
    public class MessageDto
    {
        public string SenderFirstName { get; set; }
        public string SenderLastName { get; set; }
        public int MessageId { get; set; }
        public string SenderPhotoUrl { get; set; }
        public string MessageText { get; set; }
        public bool Active { get; set; }
        public DateTime TimeStamp { get; set; }

    }
}
