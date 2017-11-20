/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ninject;
using SportsDataLoader.FileProcessing.Extensions;
using SportsDataLoader.FileProcessing.Interfaces;
using SportsDataLoader.Model;

namespace SportsDataLoader.FileProcessing.Factories
{
    public class FileProcessingServiceFactory : IFileProcessingServiceFactory
    {
        private readonly Dictionary<string, Func<IFileProcessingService>> serviceDictionary;

        public FileProcessingServiceFactory(IKernel kernel)
        {
            serviceDictionary = new Dictionary<string, Func<IFileProcessingService>>
            {
                ["csv"] = () => kernel.Get<IDelimitedTextFileProcessingService<ICsvFileParser>>(),
                ["tsv"] = () => kernel.Get<IDelimitedTextFileProcessingService<ITsvFileParser>>(),
                ["txt"] = () => kernel.Get<IDelimitedTextFileProcessingService<ITsvFileParser>>(),
                ["xlsx"] = () => kernel.Get<IXlsxFileProcessingService>(),
                ["zip"] = () => kernel.Get<IZipFileProcessingService>()
            };
        }

        public Task<IFileProcessingService> GetProcessingService(FileMetadata fileMetadata)
        {
            if (fileMetadata == null)
                throw new ArgumentNullException(nameof(fileMetadata));

            var fileExtension = fileMetadata.FileName.GetFileExtension();

            if ((fileExtension != null) && serviceDictionary.ContainsKey(fileExtension))
                return Task.FromResult(serviceDictionary[fileExtension]());

            return Task.FromResult(null as IFileProcessingService);
        }
    }
}