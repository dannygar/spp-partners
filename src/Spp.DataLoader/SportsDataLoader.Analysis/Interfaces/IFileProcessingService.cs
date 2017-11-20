/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System.IO;
using System.Threading.Tasks;
using SportsDataLoader.Model;

namespace SportsDataLoader.FileProcessing.Interfaces
{
    public interface IFileProcessingService
    {
        Task Process(FileMetadata fileMetadata, Stream fileStream);
    }
}