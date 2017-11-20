/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿// <copyright file="ILogService.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace MicrosoftSportsScience.Services
{
    public interface ILogService
    {
        void Info(string message, object sender);

        void Warning(string message, object sender);

        void Error(string message, object sender);

        void Error(Exception e, object sender);

        void FlushLogs();

        Task<StorageFile> SaveLogs();
    }
}
