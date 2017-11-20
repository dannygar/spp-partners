/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
namespace Spp.Application.Core.Models
{
    public class AthleteDrillDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int PlannedTrainingLoad { get; set; }
        public string Size { get; set; }
        public int NumberOfPLayers { get; set; }
        public string Note { get; set; }
        public int DurationInMinutes { get; set; }
        public string ImageUrl { get; set; }
        public int PracticeDrillId { get; set; }
    }
}
