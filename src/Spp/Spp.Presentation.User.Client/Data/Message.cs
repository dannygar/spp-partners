/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Runtime.Serialization;

namespace Spp.Presentation.User.Client.Data
{
    [DataContract]
    public class Message
    {
        [DataMember(Name = "SenderFirstName")]
        public string FirstName { get; set; }
        [DataMember(Name = "SenderLastName")]
        public string LastName { get; set; }
        [DataMember(Name = "MessageId")]
        public int MessageId { get; set; }
        [DataMember(Name = "SenderPhotoUrl")]
        public string SenderPhotoUrl { get; set; }
        [DataMember(Name = "MessageText")]
        public string MessageText { get; set; }
        [DataMember(Name = "Active")]
        public bool Active { get; set; }
        [DataMember(Name = "TimeStamp")]
        public DateTime TimeStamp { get; set; }
    }
}
