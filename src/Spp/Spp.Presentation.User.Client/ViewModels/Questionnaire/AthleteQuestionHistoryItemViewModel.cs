/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Spp.Presentation.User.Client.Data;
using static Spp.Presentation.User.Client.Defines;

namespace Spp.Presentation.User.Client.ViewModels
{
    public class AthleteQuestionHistoryItemViewModel : NotificationBase<AthleteQuestionHistory>
    {
        public AthleteQuestionHistoryItemViewModel(AthleteQuestionHistory history) : base(history)
        {

        }

        public string Title
        {
            get { return This.Question.Text; }
        }

        public List<string> Days
        {
            get { return This.Responses.Select(x => x.AnswerDateTime.DayOfWeek.ToString().Substring(0, 1)).ToList<string>(); }
        }

        public List<int> Values
        {
            get { return This.Responses.Select(x => x.Answer.Value).ToList<int>(); }
        }

        public GraphMode GraphMode
        {
            get { return (This.Question != null && This.Question.Id % 2 == 1) ? GraphMode.Line : GraphMode.Bar; }
        }

        public override Task Load()
        {
            return null;
        }
    }
}
