/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
    [Route(ApiRoutes.SessionRoute)]
    public class SessionController : SppBaseController
    {
        private readonly SessionService _service;

        public SessionController(SppDbContext context) : base(context)
        {
            _service = new SessionService(context);
        }

        /// <summary>
        /// Retrieves the session for the specified date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        //GET api/v1/sessions/01-01-2016
        [HttpGet("{date:datetime}")]
        //[Route(ApiRoutes.SessionRoute + "/{date:datetime:regex(\\d{2}-\\d{2}-\\d{4})}")]
        public async Task<IActionResult> Get(DateTime date)
        {
            try
            {
                var result = await _service.GetSessionsByDate(date);

                if (result == null)
                    return NotFound();
                if (!result.Any())
                    return NoContent();

                return new ObjectResult(result);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                if (ex.Message.StartsWith("Sequence contains no matching element"))
                    return NotFound();
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves the list of sessions within the specified dates range
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        // GET api/v1/sessions?from=01-01-2016&to=02-01-2016
        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery(Name = "from")]DateTime from,
            [FromQuery(Name = "to")] DateTime to)
        {
            try
            {
                var result = await _service.GetSessionsRange(from, to);

                if (result == null)
                    return NotFound();
                if (!result.Any())
                    return NoContent();

                return new ObjectResult(result);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                if (ex.Message.StartsWith("Sequence contains no matching element"))
                    return NotFound();
                return BadRequest(ex.Message);
            }
        }

        // POST api/v1/sessions
        /// <summary>
        /// Creates a new session
        /// </summary>
        /// <param name="sessionDto"></param>
        /// <returns>true, if successfull; otherwise - false</returns>
        /// <response code="201">true, if successfull; otherwise - false</response>
        /// <response code="400">If the object is null</response>
        [HttpPost]
        [ProducesResponseType(typeof(SessionDto), 201)]
        [ProducesResponseType(typeof(SessionDto), 400)]
        public async Task<IActionResult> CreateSession([FromBody, Required] SessionDto sessionDto)
        {
            try
            {
                if (sessionDto == null)
                    return BadRequest();

                var result = await _service.CreateSession(sessionDto);
                return CreatedAtRoute("default", result);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves the session by its Id
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        //GET api/v1/sessions/1
        [HttpGet("{sessionId:int}")]
        public async Task<IActionResult> Get(int sessionId)
        {
            try
            {
                var result = await _service.GetSession(sessionId);

                return new ObjectResult(result);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves all session types
        /// </summary>
        /// <returns></returns>
        //GET api/v1/sessions/types
        [HttpGet("types")]
        public async Task<IActionResult> GetSessionTypes()
        {
            try
            {
                var result = await _service.GetSessionTypes();

                return new ObjectResult(result);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        // POST api/v1/sessions/update
        /// <summary>
        /// Updates the existing session
        /// </summary>
        /// <param name="sessionDto"></param>
        /// <returns>true, if successfull; otherwise - false</returns>
        /// <response code="201">true, if successfull; otherwise - false</response>
        /// <response code="400">If the object is null</response>
        [HttpPost, Route("update")]
        [ProducesResponseType(typeof(SessionDto), 201)]
        [ProducesResponseType(typeof(SessionDto), 400)]
        public async Task<IActionResult> UpdateSession([FromBody, Required] SessionDto sessionDto)
        {
            try
            {
                if (sessionDto == null)
                    return BadRequest();

                if (await _service.UpdateSession(sessionDto))
                    return CreatedAtRoute("default", true);

                return BadRequest("Failed to update the database");

            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        // POST api/v1/sessions/delete
        /// <summary>
        /// Deletes the session with the specified Id
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns>true, if successfull; otherwise - false</returns>
        /// <response code="201">true, if successfull; otherwise - false</response>
        /// <response code="400">If the object is null</response>
        [HttpPost, Route("delete/{sessionId:int}")]
        [ProducesResponseType(typeof(SessionDto), 201)]
        [ProducesResponseType(typeof(SessionDto), 400)]
        public async Task<IActionResult> DeleteSession(int sessionId)
        {
            try
            {
                if (sessionId <= 0)
                    return BadRequest();

                if (await _service.DeleteSession(sessionId))
                    return CreatedAtRoute("default", true);

                return BadRequest("Failed to update the database");
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        // POST api/v1/sessions/addusers
        /// <summary>
        /// Add collection of users to the specific session
        /// </summary>
        /// <param name="sessionDto"></param>
        /// <returns>true, if successfull; otherwise - false</returns>
        /// <response code="201">true, if successfull; otherwise - false</response>
        /// <response code="400">If the object is null</response>
        [HttpPost, Route("addusers")]
        [ProducesResponseType(typeof(SessionDto), 201)]
        [ProducesResponseType(typeof(SessionDto), 400)]
        public async Task<IActionResult> AddUserToSession([FromBody, Required] SessionDto sessionDto)
        {
            try
            {
                if (sessionDto == null)
                    return BadRequest();

                foreach (var user in sessionDto.Users)
                {
                    await _service.AddUserToSession(sessionDto.Id, user.Id);
                }

                return CreatedAtRoute("default", true);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        // POST api/v1/sessions/addusers
        /// <summary>
        /// Add collection of users to the specific session
        /// </summary>
        /// <param name="sessionDto"></param>
        /// <returns>true, if successfull; otherwise - false</returns>
        /// <response code="201">true, if successfull; otherwise - false</response>
        /// <response code="400">If the object is null</response>
        [HttpPost, Route("removeusers")]
        [ProducesResponseType(typeof(SessionDto), 201)]
        [ProducesResponseType(typeof(SessionDto), 400)]
        public async Task<IActionResult> RemoveUsersFromSession([FromBody, Required] SessionDto sessionDto)
        {
            try
            {
                if (sessionDto == null)
                    return BadRequest();

                await _service.RemoveUsersFromSession(sessionDto.Id);

                return CreatedAtRoute("default", true);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
