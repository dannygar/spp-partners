/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using System.Threading.Tasks;

namespace Spp.Presentation.User.Client.ViewModels
{
    public class MatchViewModel : NotificationBase
    {
        public string Date;
        public string Match;
        public string Result;
        public string Appearance;
        public string Mins;
        public string Goals;
        public string Assists;
        public string Shots;
        public string Sog;

        public MatchViewModel()
        {
        }

        public override async Task Load()
        {
        }
    }
}
