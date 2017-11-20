/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Spp.Data.Entities
{
    [Table("CognitiveService")]
    public partial class CognitiveService : EntityBase
    {
        [Required]
        [StringLength(50)]
        public string WorkspaceKey { get; set; }

        [Required]
        [StringLength(50)]
        public string FaceApiKey { get; set; }

        [StringLength(50)]
        public string EmotionApiKey { get; set; }

        [StringLength(50)]
        public string BingApiKey { get; set; }

        [StringLength(50)]
        public string CameraName { get; set; }

        [StringLength(50)]
        public string Location { get; set; }

        public int MinDetectableFaceCoveragePercentage { get; set; }
    }
}
