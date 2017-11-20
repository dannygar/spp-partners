/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System.Threading.Tasks;

namespace FaceAPITrainer.Services
{
    public interface ITypedDataService
    {
        Task<T> GetItemAsync<T>(string Uri);

        Task<bool> SetItemAsync<T>(string Uri, T item);

        Task<bool> PutItemAsync<T>(string uri, T item);

        Task<TResponse> PostAsync<TBody, TResponse>(string uri, TBody body);
    }
}
