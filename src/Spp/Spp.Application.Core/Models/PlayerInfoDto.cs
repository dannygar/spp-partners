/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.Collections.Generic;

namespace Spp.Application.Core.Models
{
    public class PlayerInfoDto : ModelBase
    {
        public int? Depth { get; set; }

        public string DominantSkill { get; set; }

        public List<RestrictionDto> Restrictions { get; set; }

        public bool? IsResting { get; set; }
    }
}
