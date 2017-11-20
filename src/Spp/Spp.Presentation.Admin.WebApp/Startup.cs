/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
// Copyright (c) Microsoft Corporation. All rights reserved.

using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(WebPortal.Startup))]

namespace WebPortal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}
