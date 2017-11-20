/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.ComponentModel.DataAnnotations.Schema;

namespace Spp.Data.Entities
{
    [Table("UserQuestionnaire")]
    public class UserQuestionnaire : EntityBase
    {
        public int? UserId { get; set; }

        public int? QuestionnaireId { get; set; }

        public int? StatusId { get; set; }
    }
}
