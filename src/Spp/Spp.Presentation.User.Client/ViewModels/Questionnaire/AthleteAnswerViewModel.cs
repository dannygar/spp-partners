/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using MicrosoftSportsScience.Data;
using MicrosoftSportsScience.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrosoftSportsScience.ViewModels
{
    public class AthleteAnswerViewModel : NotificationBase<AthleteQuestionHistoryEntry>
    {
        public AthleteAnswerViewModel(AthleteQuestionHistoryEntry answer = null) : base(answer) { }
        public KeyValuePair<string, int> AnswerText
        {
            get { return This.Answer; }
            set { SetProperty(This.Answer, value, () => This.Answer = value); }
        }

        //Added label and Value to provide more options for display
        public string Caption
        {
            get { return This.Answer.Key; }
        }

        public int Value
        {
            get { return This.Answer.Value; }
        }

        public override Task Load()
        {
            return null;
        }
    }
}
