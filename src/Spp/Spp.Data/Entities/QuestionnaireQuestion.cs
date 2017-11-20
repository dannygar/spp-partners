/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.ComponentModel.DataAnnotations.Schema;

namespace Spp.Data.Entities
{
    [Table("QuestionnaireQuestion")]
    public class QuestionnaireQuestion : EntityBase
    {
        [ForeignKey("Question")]
        public int QuestionId { get; set; }

        [ForeignKey("Questionnaire")]
        public int QuestionnaireId { get; set; }

        //Navigation properties
        public Question Question { get; set; }
        public Questionnaire Questionnaire { get; set; }
    }
}
