/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using Microsoft.AspNetCore.Mvc;
using Spp.Data;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860
namespace Spp.Application.Api.Controllers
{
    public class SppBaseController : Controller
    {
        private readonly SppDbContext _context;
        private readonly BaseDbContext _testContext;

        public SppBaseController(SppDbContext context)
        {
            _context = context;
        }

        //Unit Tests
        public SppBaseController(BaseDbContext context)
        {
            _testContext = context;
        }
    }
}
