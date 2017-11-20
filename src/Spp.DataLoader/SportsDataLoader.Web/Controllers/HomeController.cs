/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SportsDataLoader.FileManagement.Interfaces;
using SportsDataLoader.Messaging.Interfaces;
using SportsDataLoader.Model;
using SportsDataLoader.Model.Constants;
using SportsDataLoader.Model.Enumerations;
using SportsDataLoader.Model.Events;
using SportsDataLoader.Model.Interfaces;
using SportsDataLoader.Shared.Extensions;
using SportsDataLoader.Web.Extensions;
using SportsDataLoader.Web.Interfaces;
using SportsDataLoader.Web.Models;

namespace SportsDataLoader.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebConfiguration configuration;
        private readonly IFileMetadataRepository fileMetadataRepository;
        private readonly IFileRepository fileRepository;
        private readonly IMessageSender<FileUploaded> fileUploadedMessageSender;

        public HomeController(IWebConfiguration configuration,
                              IFileMetadataRepository fileMetadataRepository,
                              IFileRepository fileRepository,
                              IMessageSender<FileUploaded> fileUploadedMessageSender)
        {
            this.configuration = configuration;
            this.fileMetadataRepository = fileMetadataRepository;
            this.fileRepository = fileRepository;
            this.fileUploadedMessageSender = fileUploadedMessageSender;
        }

        public async Task<ActionResult> Index(Guid? tenantId = null, string cultureCode = null)
        {
            tenantId = (tenantId ?? configuration.DefaultTenantId);

            var culture = configuration.DefaultCulture;

            if (cultureCode != null)
            {
                culture = cultureCode.TryParseCultureInfo();

                if (culture == null)
                    return BadRequest($"[{cultureCode}] is not a valid culture code.");
            }

            try
            {
                var fileMetadata = await fileMetadataRepository.GetAllTenantFileMetadata(tenantId.ToString());

                var tenantMetadataViewModel = new TenantMetadataViewModel
                {
                    CultureCode = culture.Name,
                    TenantId = tenantId.Value,

                    FileMetadataList = fileMetadata.OrderByDescending(fm => fm.CreatedDateTimeUtc)
                                                   .Select(fm => fm.ToViewModel(culture))
                                                   .ToList()
                };

                return View(tenantMetadataViewModel);
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }


        public async Task<ActionResult> GetAllFileMetadata(Guid? tenantId = null, string cultureCode = null)
        {
            tenantId = (tenantId ?? configuration.DefaultTenantId);

            var culture = configuration.DefaultCulture;

            if (cultureCode != null)
            {
                culture = cultureCode.TryParseCultureInfo();

                if (culture == null)
                    return BadRequest($"[{cultureCode}] is not a valid culture code.");
            }

            try
            {
                var fileMetadata = await fileMetadataRepository.GetAllTenantFileMetadata(tenantId.ToString());

                return Json(fileMetadata.OrderByDescending(fm => fm.CreatedDateTimeUtc)
                                        .Select(fm => fm.ToViewModel(culture)),
                            JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        public async Task<ActionResult> UploadFiles(Guid? tenantId = null, string cultureCode = null)
        {
            tenantId = (tenantId ?? configuration.DefaultTenantId);

            var culture = configuration.DefaultCulture;

            if (cultureCode != null)
            {
                culture = cultureCode.TryParseCultureInfo();

                if (culture == null)
                    return BadRequest($"[{cultureCode}] is not a valid culture code.");
            }

            if (Request.Files.Count == 0)
                return BadRequest("No files uploaded.");

            var fileMetadataList = await ProcessUploadedFiles(culture, tenantId.Value);

            return Json(fileMetadataList.Select(fm => fm.ToViewModel(culture)));
        }

        private async Task<IEnumerable<FileMetadata>> ProcessUploadedFiles(CultureInfo cultureInfo, Guid tenantId)
        {
            var fileMetadataList = new List<FileMetadata>();

            foreach (string fileName in Request.Files)
            {
                var file = Request.Files[fileName];

                if ((file != null) && (file.ContentLength > 0))
                {
                    var fileMetadata = new FileMetadata
                    {
                        FileId = Guid.NewGuid().ToString(),
                        FileName = file.FileName,
                        FileSchemaName = FileDataTypes.Unknown,
                        FileMimeType = file.ContentType,
                        FileCultureCode = cultureInfo.Name,
                        FileSize = file.ContentLength,
                        FileStatus = FileStatus.Uploaded,
                        TenantId = tenantId.ToString()
                    };

                    fileMetadata.CreatedDateTimeUtc = fileMetadata.LastModifiedDateTimeUtc = DateTime.UtcNow;

                    await OnFileUploaded(file, fileMetadata);

                    fileMetadataList.Add(fileMetadata);
                }
            }

            return fileMetadataList;
        }

        private async Task OnFileUploaded(HttpPostedFileBase file, FileMetadata fileMetadata)
        {
            await fileRepository.SaveFileAsync(file.InputStream, fileMetadata.TenantId, fileMetadata.FileId);
            await fileMetadataRepository.UpsertFileMetadata(fileMetadata);
            await fileUploadedMessageSender.SendMessage(new FileUploaded(fileMetadata));
        }

        private HttpStatusCodeResult BadRequest(string message = null)
        {
            return (string.IsNullOrEmpty(message)
                ? new HttpStatusCodeResult(HttpStatusCode.BadRequest)
                : new HttpStatusCodeResult(HttpStatusCode.BadRequest, message));
        }
    }
}