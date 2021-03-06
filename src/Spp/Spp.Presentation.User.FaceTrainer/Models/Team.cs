/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using System.Collections.Generic;

namespace FaceAPITrainer.Models
{
    public class Team : ModelBase
    {
        public string Name { get; set; }

        public int? SportId { get; set; }

        public int? CoachId { get; set; }

        public int? PlayerCaptainId { get; set; }

        public DateTime? Founded { get; set; }

        public bool? IsAffiliate { get; set; }

        public int? MinAge { get; set; }

        public int? MaxAge { get; set; }

        public string Abbreviation { get; set; }

        public int? TeamId { get; set; }

        public int? LocaleId { get; set; }

        public bool? isNationalTeam { get; set; }

        public string PathtoPhoto { get; set; }

        public string Competition { get; set; }

        public string Gender { get; set; }

        public int? GroupId { get; set; }


        public IEnumerable<User> Users { get; set; }
    }
}