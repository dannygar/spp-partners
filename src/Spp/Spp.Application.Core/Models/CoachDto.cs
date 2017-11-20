/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
namespace Spp.Application.Core.Models
{
    public class CoachDto : UserDto
    {
        public int CoachId { get; set; }
        public string CoachName { get; set; }
        public string CoachPhoto { get; set; }
    }
}
