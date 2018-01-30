using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Spp.Application.Core.Contracts;
using Spp.Application.Core.Models;
using Spp.Application.Services;
using Spp.Data;

namespace Spp.Application.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    //[Authorize]
    [Route(ApiRoutes.WellnessRoute)]
    public class WellnessController : SppBaseController
    {
        private readonly IWellnessService _service;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public WellnessController(SppDbContext context) : base(context)
        {
            _service = new WellnessService(context);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("player/{playerId:int}")]
        public async Task<IActionResult> Get(int playerId)
        {
            try
            {
                var result = await _service.GetWellnessesByPlayerId(playerId);

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
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(WellnessDto), 201)]
        [ProducesResponseType(typeof(WellnessDto), 400)]
        public async Task<IActionResult> Post([FromBody, Required] WellnessDto dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest();

                var result = await _service.SubmitWellness(dto);
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
