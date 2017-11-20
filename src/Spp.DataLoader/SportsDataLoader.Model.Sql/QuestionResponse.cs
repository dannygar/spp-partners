/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
namespace SportsDataLoader.Model.Sql
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("QuestionResponse")]
    public partial class QuestionResponse
    {
        public int Id { get; set; }

        public int Value { get; set; }

        public int SessionId { get; set; }

        public int QuestionId { get; set; }

        public int QuestionnaireId { get; set; }

        public virtual Question Question { get; set; }

        public virtual Questionnaire Questionnaire { get; set; }

        public virtual Session Session { get; set; }
    }
}
