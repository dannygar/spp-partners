/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System.Globalization;
using SportsDataLoader.Model;
using SportsDataLoader.Model.Enumerations;
using SportsDataLoader.Web.Models;

namespace SportsDataLoader.Web.Extensions
{
    public static class FileMetadataExtensions
    {
        public static FileMetadataViewModel ToViewModel(this FileMetadata fileMetadata,
                                                        CultureInfo cultureInfo)

        {
            var viewModel = new FileMetadataViewModel
            {
                FileId = fileMetadata.FileId,
                FileName = fileMetadata.FileName,
                FileSchemaName = fileMetadata.FileSchemaName,
                FileUploadDateTime = fileMetadata.CreatedDateTimeUtc.ToString("U", cultureInfo)
            };

            switch (fileMetadata.FileStatus)
            {
                case FileStatus.Processed:
                    viewModel.FileStatusDescription = "Processed";
                    viewModel.FileStatusClass = "default";
                    viewModel.FileStatusIcon = "glyphicon glyphicon-ok";
                    break;
                case FileStatus.Processing:
                    viewModel.FileStatusDescription = "Processing";
                    viewModel.FileStatusClass = "info";
                    viewModel.FileStatusIcon = "glyphicon glyphicon-cog";
                    break;
                case FileStatus.ProcessingFailed:
                    viewModel.FileStatusDescription = "Processing Failed";
                    viewModel.FileStatusClass = "danger";
                    viewModel.FileStatusIcon = "glyphicon glyphicon-remove";
                    break;
                case FileStatus.Uploaded:
                    viewModel.FileStatusDescription = "Uploaded";
                    viewModel.FileStatusClass = "info";
                    viewModel.FileStatusIcon = "glyphicon glyphicon-hourglass";
                    break;
            }

            return viewModel;
        }
    }
}