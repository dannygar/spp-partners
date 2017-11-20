/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
namespace Spp.Application.Core.Models
{
    public sealed class SessionUserDto : ModelBase
    {
        public int SessionId { get; set; }
        public int UserId { get; set; }
    }
}
