/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using SportsDataLoader.FileManagement.Interfaces;
using SportsDataLoader.Messaging.Interfaces;
using SportsDataLoader.Model;
using SportsDataLoader.Model.Constants;
using SportsDataLoader.Model.Enumerations;
using SportsDataLoader.Model.Events;
using SportsDataLoader.Model.Interfaces;
using SportsDataLoader.WebApi.Extensions;
using SportsDataLoader.WebApi.Models;

namespace SportsDataLoader.WebApi.Controllers
{
    [RoutePrefix("api/file")]
    public class FileController : ApiController
    {
        private readonly IFileMetadataRepository fileMetadataRepository;
        private readonly IFileRepository fileRepository;
        private readonly IMessageSender<FileUploaded> fileUploadedMessageSender;

        public FileController(IFileMetadataRepository fileMetadataRepository,
                              IFileRepository fileRepository,
                              IMessageSender<FileUploaded> fileUploadedMessageSender)
        {
            this.fileRepository = fileRepository;
            this.fileMetadataRepository = fileMetadataRepository;
            this.fileUploadedMessageSender = fileUploadedMessageSender;
        }

        [HttpPost]
        [Route("{tenantId:guid}/{cultureCode?}")]
        public async Task<IEnumerable<FileMetadataModel>> UploadFiles(Guid tenantId, string cultureCode = null)
        {
            var request = Request;
            var tenantIdString = tenantId.ToString();

            var cultureInfo = (cultureCode == null)
                ? CultureInfo.CurrentCulture
                : ParseCultureCode(cultureCode);

            if (request.Content.IsMimeMultipartContent() == false)
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

            var fileMetadataList = new List<FileMetadata>();
            var fileStreams = await request.Content.ReadAsMultipartAsync();

            foreach (var httpContent in fileStreams.Contents)
            {
                var httpContentStream = await httpContent.ReadAsStreamAsync();

                var fileMetadata = new FileMetadata
                {
                    FileId = Guid.NewGuid().ToString(),
                    FileName = httpContent.Headers.ContentDisposition.FileName.Trim('"'),
                    FileSchemaName = FileDataTypes.Unknown,
                    FileMimeType = httpContent.Headers.ContentType.MediaType,
                    FileCultureCode = cultureInfo.Name,
                    FileSize = httpContentStream.Length,
                    TenantId = tenantIdString,
                    FileStatus = FileStatus.Uploaded
                };

                fileMetadata.CreatedDateTimeUtc = fileMetadata.LastModifiedDateTimeUtc = DateTime.UtcNow;

                await fileRepository.SaveFileAsync(httpContentStream, tenantIdString, fileMetadata.FileId);
                await OnFileUploaded(fileMetadata);

                fileMetadataList.Add(fileMetadata);
            }

            return fileMetadataList.Select(m => m.ToFileMetadataModel());
        }

        [HttpGet]
        [Route("{tenantId:guid}/metadata")]
        public async Task<IEnumerable<FileMetadataModel>> GetAllFileMetadata(Guid tenantId)
        {
            var fileMetadata =
                await fileMetadataRepository.GetAllTenantFileMetadata(tenantId.ToString());

            return fileMetadata.Select(m => m.ToFileMetadataModel());
        }

        [HttpGet]
        [Route("{tenantId:guid}/{fileId:guid}/metadata")]
        public async Task<FileMetadataModel> GetFileMetadata(Guid tenantId, Guid fileId)
        {
            var fileMetadata =
                await fileMetadataRepository.GetFileMetadata(tenantId.ToString(), fileId.ToString());

            if (fileMetadata == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return fileMetadata.ToFileMetadataModel();
        }

        private CultureInfo ParseCultureCode(string cultureCode)
        {
            if (string.IsNullOrEmpty(cultureCode))
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            try
            {
                return CultureInfo.GetCultureInfo(cultureCode);
            }
            catch
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        private async Task OnFileUploaded(FileMetadata fileMetadata)
        {
            await fileMetadataRepository.UpsertFileMetadata(fileMetadata);

            await fileUploadedMessageSender.SendMessage(new FileUploaded
            {
                Id = Guid.NewGuid().ToString(),
                FileMetadata = fileMetadata,
                TenantId = fileMetadata.TenantId
            });
        }
    }
}