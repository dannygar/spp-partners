/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
namespace Spp.Application.Core.Models
{
    public class UserIdentityDto
    {
        public double Confidence { get; set; }
        public string FullName { get; set; }
        public EmotionsDto Emotions { get; set; }
    }
}
