/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
namespace Spp.Application.Core.Models
{
    public class ExerciseDto : ModelBase
    {
        public int SequenceNumber { get; set; }

        public string Category { get; set; }

        public string Description { get; set; }

        public string Name { get; set; }

        public ImageDto Image { get; set; }

    }
}
