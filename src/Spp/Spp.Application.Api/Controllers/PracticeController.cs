/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Spp.Application.Core.Models;
using Spp.Data;
using Spp.Application.Services;

namespace Spp.Application.Api.Controllers.V1
{
    [Authorize]
    [Route(ApiRoutes.PracticeRoute)]
    public class PracticeController : SppBaseController
    {
        private readonly PracticeService _service;

        public PracticeController(SppDbContext context) : base(context)
        {
            _service = new PracticeService(context);
        }

        /// <summary>
        /// Retrieves all practices
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllPractices()
        {
            try
            {
                var practices = await _service.GetAllPractices();

                if (practices == null)
                    return NotFound();

                return new ObjectResult(practices);

            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves the list of practices for the current session Id
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        //GET api/v1/practices/5
        [HttpGet("{sessionId:int}")]
        public async Task<IActionResult> GetSessionPractices(int sessionId)
        {
            try
            {
                var practices = await _service.GetSessionPractices(sessionId);

                if (practices == null)
                    return NotFound();

                return new ObjectResult(practices);

            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves all practice drills for the given practice
        /// </summary>
        /// <param name="practiceId"></param>
        /// <returns></returns>
        //GET api/v1/drills/5
        [HttpGet, Route("drills/{practiceId:int}")]
        public async Task<IActionResult> GetPracticeDrills(int practiceId)
        {
            try
            {
                var drills = await _service.GetPracticeDrills(practiceId);

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

        /// <summary>
        /// Retrieves all practice drills
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("drills")]
        public async Task<IActionResult> GetAllDrills()
        {
            try
            {
                var drills = await _service.GetPracticeDrills();

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

        /// <summary>
        /// Retrieves the practice by its Id
        /// </summary>
        /// <param name="practiceId"></param>
        /// <returns></returns>
        //GET api/v1/practices/id/5
        [HttpGet, Route("id/{practiceId:int}")]
        public async Task<IActionResult> Get(int practiceId)
        {
            try
            {
                var practice = await _service.GetPractice(practiceId);

                if (practice == null)
                    return NotFound();

                return new ObjectResult(practice);

            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        // POST api/v1/practices
        /// <summary>
        /// Adds a new practice to the Db
        /// </summary>
        /// <param name="practiceDto"></param>
        /// <param name="note"></param>
        /// <returns>true, if successfull; otherwise - false</returns>
        /// <response code="201">true, if successfull; otherwise - false</response>
        /// <response code="400">If the object is null</response>
        [HttpPost]
        [ProducesResponseType(typeof(PracticeDto), 201)]
        [ProducesResponseType(typeof(PracticeDto), 400)]
        public async Task<IActionResult> CreatePractice([FromBody, Required] PracticeDto practiceDto)
        {
            try
            {
                if (practiceDto == null)
                    return BadRequest();

                var result = await _service.CreatePractice(practiceDto);

                return CreatedAtRoute("default", result);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        // POST api/v1/practices/update
        /// <summary>
        /// Updates the existing practice
        /// </summary>
        /// <param name="practiceDto"></param>
        /// <returns>true, if successfull; otherwise - false</returns>
        /// <response code="201">true, if successfull; otherwise - false</response>
        /// <response code="400">If the object is null</response>
        [HttpPost, Route("update")]
        [ProducesResponseType(typeof(PracticeDto), 201)]
        [ProducesResponseType(typeof(PracticeDto), 400)]
        public async Task<IActionResult> UpdatePractice([FromBody, Required] PracticeDto practiceDto)
        {
            try
            {
                if (practiceDto == null)
                    return BadRequest();

                var result = await _service.UpdatePractice(practiceDto);

                return CreatedAtRoute("default", result);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        // POST api/v1/practices/delete/5
        /// <summary>
        /// Delete the practice with the specified Id
        /// </summary>
        /// <param name="practiceId"></param>
        /// <returns>true, if successfull; otherwise - false</returns>
        /// <response code="201">true, if successfull; otherwise - false</response>
        /// <response code="400">If the object is null</response>
        [HttpPost, Route("delete/{practiceId:int}")]
        [ProducesResponseType(typeof(PracticeDto), 201)]
        [ProducesResponseType(typeof(PracticeDto), 400)]
        public async Task<IActionResult> DeletePractice(int practiceId)
        {
            try
            {
                if (practiceId <= 0)
                    return BadRequest();

                var result = await _service.DeletePractice(practiceId);

                return CreatedAtRoute("default", result);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
