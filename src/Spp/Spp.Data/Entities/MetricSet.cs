/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.ComponentModel.DataAnnotations.Schema;

namespace Spp.Data.Entities
{
    [Table("MetricSet")]
    public class MetricSet : EntityBase
    {
        public string ShortName { get; set; }

        public string FullName { get; set; }

        public string Tooltip { get; set; }
    }
}
