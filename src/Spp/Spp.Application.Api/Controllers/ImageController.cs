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

    [Route(ApiRoutes.ImageRoute)]
    public class ImageController : SppBaseController
    {
        private readonly SettingsService _service;

        public ImageController(SppDbContext context) : base(context)
        {
            this._service = new SettingsService(context);
        }


        /// <summary>
        /// Returns the Images.
        /// </summary>
        /// <returns></returns>s
        [HttpGet]
        public async Task<IActionResult> GetImages()
        {
            try
            {
                var result = await this._service.GetImages();

                return this.Ok(result);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return this.BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Creates a new image
        /// </summary>
        /// <param name="imageDto"></param>
        /// <returns>true, if successfull; otherwise - false</returns>
        /// <response code="201">true, if successfull; otherwise - false</response>
        /// <response code="400">If the object is null</response>
        [Authorize]
        [HttpPost, Route("create/image")]
        [ProducesResponseType(typeof(ImageDto), 201)]
        [ProducesResponseType(typeof(ImageDto), 400)]
        public async Task<IActionResult> CreateImage([FromBody, Required] ImageDto imageDto)
        {
            try
            {
                if (imageDto == null)
                    return BadRequest();

                var result = await _service.CreateImage(imageDto);

                return CreatedAtRoute("default", result);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Updates the existing image
        /// </summary>
        /// <param name="imageDto"></param>
        /// <returns>true, if successfull; otherwise - false</returns>
        /// <response code="201">true, if successfull; otherwise - false</response>
        /// <response code="400">If the object is null</response>
        [Authorize]
        [HttpPost, Route("update/image")]
        [ProducesResponseType(typeof(ImageDto), 201)]
        [ProducesResponseType(typeof(ImageDto), 400)]
        public async Task<IActionResult> UpdateImage([FromBody, Required] ImageDto imageDto)
        {
            try
            {
                if (imageDto == null)
                    return BadRequest();

                if (await _service.UpdateImage(imageDto))
                    return CreatedAtRoute("default", true);

                return BadRequest("Failed to update the database");

            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Deletes the image using the specified  Id
        /// </summary>
        /// <param name="imageId"></param>
        /// <returns>true, if successfull; otherwise - false</returns>
        /// <response code="201">true, if successfull; otherwise - false</response>
        /// <response code="400">If the object is null</response>
        [Authorize]
        [HttpPost, Route("delete/image/{imageId:int}")]
        [ProducesResponseType(typeof(ImageDto), 201)]
        [ProducesResponseType(typeof(ImageDto), 400)]
        public async Task<IActionResult> DeleteImage(int imageId)
        {
            try
            {
                if (imageId <= 0)
                    return BadRequest();

                if (await _service.DeleteImage(imageId))
                    return CreatedAtRoute("default", true);

                return BadRequest("Failed to update the database");
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
