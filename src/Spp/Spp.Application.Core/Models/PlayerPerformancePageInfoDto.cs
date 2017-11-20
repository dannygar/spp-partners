/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
namespace Spp.Application.Core.Models
{
    public class PlayerPerformancePageInfoDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? Age { get; set; }
        public decimal? Height { get; set; }
        public string DominantSkill { get; set; }
        public string BattingGroup { get; set; }
        public string PathToPhoto { get; set; }
        public bool? IsResting { get; set; }
    }
}
