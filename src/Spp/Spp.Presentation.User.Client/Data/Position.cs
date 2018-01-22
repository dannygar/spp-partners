/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
namespace Spp.Presentation.User.Client.Data
{
    public class Position
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? SportId { get; set; }

        public string Abbreviation { get; set; }
    }
}
