/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Spp.Data;
using Spp.Application.Services;

namespace Spp.Application.Api.Controllers.V1
{
    [Authorize]
    [Route(ApiRoutes.DrillRoute)]
    public class DrillController : SppBaseController
    {
        private readonly DrillService _service;

        public DrillController(SppDbContext context) : base(context)
        {
            _service = new DrillService(context);
        }

        /// <summary>
        /// Retrieves all drills
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllDrills()
        {
            try
            {
                var drills = await _service.GetAllDrills();

                if (drills == null)
                    return NotFound();

                return new ObjectResult(drills);

            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
