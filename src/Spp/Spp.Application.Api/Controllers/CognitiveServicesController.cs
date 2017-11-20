/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
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
    [Route(ApiRoutes.CognitiveServiceRoute)]
    public class CognitiveServicesController : SppBaseController
    {
        private readonly CognitiveService _service;

        public CognitiveServicesController(SppDbContext context) : base(context)
        {
            _service = new CognitiveService(context);
        }

        /// <summary>
        /// Gets the Cognitive Services API Keys for the current team
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        //GET api/v1/cs/keys/5
        [Authorize]
        [HttpGet, Route("keys/{teamId:int}")]
        public async Task<IActionResult> GetCSKeysForTeam(int teamId)
        {
            try
            {
                var csKeys = await _service.GetCognitiveServiceKeys(teamId);

                if (csKeys == null)
                    return NotFound();

                return new ObjectResult(csKeys);

            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        // POST api/v1/cs
        /// <summary>
        /// Stores new Cognitive Services API Keys into the Database
        /// </summary>
        /// <param name="csKeysDto"></param>
        /// <returns>true, if successfull; otherwise - false</returns>
        /// <response code="201">true, if successfull; otherwise - false</response>
        /// <response code="400">If the object is null</response>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(CognitiveServiceKeysDto), 201)]
        [ProducesResponseType(typeof(CognitiveServiceKeysDto), 400)]
        public async Task<IActionResult> CreateCognitiveServiceKeys([FromBody, Required] CognitiveServiceKeysDto csKeysDto)
        {
            try
            {
                if (csKeysDto == null)
                    return BadRequest();

                var result = await _service.CreateCognitiveServiceKeys(csKeysDto);

                return CreatedAtRoute("default", result);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        // POST api/v1/cs/update
        /// <summary>
        /// Updates the existing Cognitive Services Keys
        /// </summary>
        /// <param name="csKeysDto"></param>
        /// <returns>true, if successfull; otherwise - false</returns>
        /// <response code="201">true, if successfull; otherwise - false</response>
        /// <response code="400">If the object is null</response>
        [Authorize]
        [HttpPost, Route("update")]
        [ProducesResponseType(typeof(CognitiveServiceKeysDto), 201)]
        [ProducesResponseType(typeof(CognitiveServiceKeysDto), 400)]
        public async Task<IActionResult> UpdateCognitiveServiceKeys([FromBody, Required] CognitiveServiceKeysDto csKeysDto)
        {
            try
            {
                if (csKeysDto == null)
                    return BadRequest();

                var result = await _service.UpdateCognitiveServiceKeys(csKeysDto);

                return CreatedAtRoute("default", result);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        // POST api/v1/cs/faceid
        /// <summary>
        /// Authenticates user using the face recognition
        /// </summary>
        /// <param name="userIdentities"></param>
        /// <returns>true, if successfull; otherwise - false</returns>
        /// <response code="201">true, if successfull; otherwise - false</response>
        /// <response code="400">If the object is null</response>
        [HttpPost, Route("faceid")]
        [ProducesResponseType(typeof(IList<UserIdentityDto>), 201)]
        [ProducesResponseType(typeof(IList<UserIdentityDto>), 400)]
        public async Task<IActionResult> AuthenticateUserByFace([FromBody, Required] IList<UserIdentityDto> userIdentities)
        {
            try
            {
                if (userIdentities == null)
                    return BadRequest();

                var result = await _service.AuthenticateUserByFace(userIdentities);

                return CreatedAtRoute("default", result);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        // POST api/v1/cs/delete/5
        /// <summary>
        /// Deletes the Cognitive Services API Keys with the specified Id
        /// </summary>
        /// <param name="csId"></param>
        /// <returns>true, if successfull; otherwise - false</returns>
        /// <response code="201">true, if successfull; otherwise - false</response>
        /// <response code="400">If the object is null</response>
        [Authorize]
        [HttpPost, Route("delete/{csId:int}")]
        [ProducesResponseType(typeof(CognitiveServiceKeysDto), 201)]
        [ProducesResponseType(typeof(CognitiveServiceKeysDto), 400)]
        public async Task<IActionResult> DeleteCognitiveServicesKeys(int csId)
        {
            try
            {
                if (csId <= 0)
                    return BadRequest();

                var result = await _service.DeleteCognitiveServiceKeys(csId);

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
