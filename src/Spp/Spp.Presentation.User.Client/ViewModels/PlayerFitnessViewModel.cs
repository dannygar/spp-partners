/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using System.Threading.Tasks;

namespace Spp.Presentation.User.Client.ViewModels
{
    public class PlayerFitnessViewModel : NotificationBase
    {
        public string PlayerImage;
        public string PlayerName;
        public string PlayerTrendNumber;
        public string Color;
        public string PlayerAge;
        public string PlayerPosition;
        public string PlayerHeight;
        public string PlayerJerseyNum;

        public PlayerFitnessViewModel()
        {
        }

        public override async Task Load()
        {
        }
    }
}
