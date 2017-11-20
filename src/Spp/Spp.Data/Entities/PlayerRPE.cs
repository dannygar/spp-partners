/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;


namespace Spp.Data.Entities
{
    public partial class PlayerRPE : EntityBase
    {
        public Nullable<int> Microciclo { get; set; }
        public string PlayerName { get; set; }
        public Nullable<int> PlayerId { get; set; }
        public Nullable<int> MatchDayTypeId { get; set; }
        public Nullable<int> MeasurementTypeId { get; set; }
        public Nullable<double> Value { get; set; }
    }
}
