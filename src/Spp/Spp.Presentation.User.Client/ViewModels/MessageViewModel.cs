/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using MicrosoftSportsScience.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrosoftSportsScience.ViewModels
{
    public class MessageViewModel : NotificationBase<Message>
    {
        public MessageViewModel(Message message) : base(message)
        {

        }

        public string Firstname => This.FirstName;
        public string Lastname => This.LastName;
        public string Text => This.MessageText;
        public string SenderPhotoUrl => This.SenderPhotoUrl;

        public string DateSent
        {
            get
            {
                if (This.TimeStamp > DateTime.Now.AddDays(-1) && DateTime.Now.Hour - This.TimeStamp.Hour > 0)
                    return DateTime.Now.Hour - This.TimeStamp.Hour + " hours ago";

                if (This.TimeStamp > DateTime.Now.AddDays(-2) && DateTime.Now.Day - This.TimeStamp.Day == 1)
                    return "Yesterday";

                return This.TimeStamp.ToString("d");
            }
        }

        public override Task Load() => null;
    }
}
