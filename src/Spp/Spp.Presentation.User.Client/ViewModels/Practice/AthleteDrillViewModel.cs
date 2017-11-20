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
using Windows.UI.Xaml.Media.Imaging;

namespace MicrosoftSportsScience.ViewModels
{
    public class AthleteDrillViewModel : NotificationBase<PracticeDrill>
    {
        public AthleteDrillViewModel(PracticeDrill drill) : base(drill)
        {

        }

        public string Category => This.Drill?.Category;

        public string SubCategory => This.Drill?.SubCategory;

        public string Name => This.Drill?.Name;

        public string Note => This.Note?.Text;

        public int PracticeDrillId => This.Id;

        public int DrillId => This.Drill.Id;

        public string ImageUrl => This.Drill?.Image != null ? This.Drill.Image.Url : " ";

        public int Duration => This.Duration ?? 0;

        public string Description => This.Drill?.Description;

        public int NumberOfPlayers => This.NumberOfPlayers ?? 0;

        public int TrainingLoad => This.CalculatedTrainingLoad ?? 0;

        public int RecommendedTrainingLoad { get; set; }

        public string Size => This.Size;


        public override Task Load()
        {
            return null;
        }
    }
}
