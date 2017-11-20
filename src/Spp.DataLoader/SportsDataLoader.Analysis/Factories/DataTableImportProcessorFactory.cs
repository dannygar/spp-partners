/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using System.Threading.Tasks;
using Ninject;
using SportsDataLoader.FileProcessing.Interfaces;
using SportsDataLoader.FileProcessing.Models;
using SportsDataLoader.Model;

namespace SportsDataLoader.FileProcessing.Factories
{
    public class DataTableImportProcessorFactory : IDataTableImportProcessorFactory
    {
        private readonly IKernel kernel;

        public DataTableImportProcessorFactory(IKernel kernel)
        {
            this.kernel = kernel;
        }

        public Task<IDataTableImportProcessor> GetImportProcessor(DataTableImportMetadata importMetadata)
        {
            if (importMetadata == null)
                throw new ArgumentNullException(nameof(importMetadata));

            return Task.FromResult(kernel.Get<IDataTableImportProcessor>(importMetadata.Schema.SchemaType));
        }
    }
}