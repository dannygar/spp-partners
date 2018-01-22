/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using System.Threading.Tasks;

namespace Spp.Presentation.User.Client.Services
{
    public interface IHttpClientService
    {
        Task<T> GetItemAsync<T>(string apiEndpoint, string query = null);

        Task<bool> SetItemAsync<T>(string Uri, T item, string query = null);

        Task<bool> PutItemAsync<T>(string uri, T item, string query = null);

        Task<int> SetItemAndReturnIdAsync<T>(string uri, T item, string query = null);

        Task<TResponse> PostAsync<TBody, TResponse>(string uri, TBody body, string query = null);
    }
}
