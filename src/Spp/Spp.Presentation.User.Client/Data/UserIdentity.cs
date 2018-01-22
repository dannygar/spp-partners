/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
namespace Spp.Presentation.User.Client.Data
{
    public class UserIdentity
    {
        public double Confidence { get; set; }
        public string FullName { get; set; }
        public Emotions Emotions { get; set; }
    }
}
