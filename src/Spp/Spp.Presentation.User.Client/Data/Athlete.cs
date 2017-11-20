/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
namespace MicrosoftSportsScience.Data
{
    public class Athlete : User
    {
        public int PlayerId { get; set; }
        public string PlayerName { get; set; }
        public string Position { get; set; }
        public string PlayerPhoto { get; set; }
        public bool? IsResting { get; set; }
        public int? Age { get; set; }
    }
}
