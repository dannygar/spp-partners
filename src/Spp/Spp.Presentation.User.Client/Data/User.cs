/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using System;

namespace Spp.Presentation.User.Client.Data
{
    public class User
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MiddleName { get; set; }

        public string Nickname { get; set; }

        public int? NationalityId { get; set; }

        public int? RoleId { get; set; }

        public string Gender { get; set; }

        public decimal? Height { get; set; }

        public decimal? Weight { get; set; }

        public int? EducationId { get; set; }

        public int? LocaleId { get; set; }

        public DateTime? DateofBirth { get; set; }

        public bool? isActive { get; set; }

        public string Email { get; set; }

        public string PathToPhoto { get; set; }

        public bool? isEnabled { get; set; }

        public DateTime? TurnedProfessional { get; set; }

        public string FullName { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? AMSId { get; set; }

        public string AADId { get; set; }

        public Player PlayerInfo { get; set; }

        //Navigation Keys
        public int TeamId { get; set; }


        public bool IsAthlete => (RoleTypes)this.RoleId == RoleTypes.Player;
        public bool IsCoach => (RoleTypes)this.RoleId == RoleTypes.Coach;
    }
}
