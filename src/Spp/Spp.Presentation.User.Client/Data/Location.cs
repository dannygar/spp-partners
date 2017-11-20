/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿namespace MicrosoftSportsScience.Data
{
    public enum LocationType
    {
        Unknown = 0,
        Stadium = 1,
        Gym = 2
    }

    public class Location
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public LocationType Type { get; set; }

    }
}