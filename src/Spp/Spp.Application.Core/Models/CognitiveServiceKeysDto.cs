/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
namespace Spp.Application.Core.Models
{
    public class CognitiveServiceKeysDto : ModelBase
    {
        public string WorkspaceKey { get; set; }
        public string FaceApiKey { get; set; }
        public string EmotionApiKey { get; set; }
        public string BingApiKey { get; set; }
        public string CameraName { get; set; }
        public string Location { get; set; }
        public uint MinDetectableFaceCoveragePercentage { get; set; }

        //Navigation keys
        public int TeamId { get; set; }
    }
}
