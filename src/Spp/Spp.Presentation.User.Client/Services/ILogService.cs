/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace Spp.Presentation.User.Client.Services
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
