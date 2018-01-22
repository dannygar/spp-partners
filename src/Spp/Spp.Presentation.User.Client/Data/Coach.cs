/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
namespace Spp.Presentation.User.Client.Data
{
    public class Coach : User
    {
        public int CoachId { get; set; }
        public string CoachName { get; set; }
        public string CoachPhoto { get; set; }
    }
}
