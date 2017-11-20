/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.ComponentModel.DataAnnotations.Schema;

namespace Spp.Data.Entities
{
    [Table("SessionUser")]
    public class SessionUser : EntityBase
    {
        [ForeignKey("Session")]
        public int SessionId { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        public Session Session { get; set; }
        public User User { get; set; }
    }
}
