/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿// ------------------------------------------------------
// <copyright file="TypedJsonService.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>
// ------------------------------------------------------

using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FaceAPITrainer.Services
{
    public class HttpClientService : ITypedDataService
    {
        private readonly IApiAuthService authService;

        public HttpClientService(IApiAuthService authService)
        {
            this.authService = authService;
        }

        public async Task<T> GetItemAsync<T>(string uri)
        {
            var hndlr = new HttpClientHandler { UseDefaultCredentials = true };

            try
            {
                using (var client = new HttpClient(hndlr))
                {
                    client.BaseAddress = new Uri(uri);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var token = await this.authService.GetAuthToken();
                    if (token != null)
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    HttpResponseMessage response = await client.GetAsync(uri);

                    if (response.IsSuccessStatusCode && response.StatusCode != HttpStatusCode.BadRequest
                        && response.StatusCode != HttpStatusCode.BadGateway &&
                        response.StatusCode != HttpStatusCode.NoContent)
                    {
                        using (HttpContent content = response.Content)
                        {
                            string contents = await content.ReadAsStringAsync();

                            if (!string.IsNullOrEmpty(contents))
                            {
                                var responseInfo = JsonConvert.DeserializeObject<T>(contents);
                                return responseInfo;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return default(T);
        }

        public async Task<bool> SetItemAsync<T>(string uri, T item)
        {
            var hndlr = new HttpClientHandler { UseDefaultCredentials = true };

            try
            {
                using (var client = new HttpClient(hndlr))
                {
                    client.BaseAddress = new Uri(uri);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var token = await this.authService.GetAuthToken();
                    if (token != null)
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    var jsonContent = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(uri, jsonContent);

                    return response.IsSuccessStatusCode;
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return false;
        }

        public async Task<bool> PutItemAsync<T>(string uri, T item)
        {
            var hndlr = new HttpClientHandler { UseDefaultCredentials = true };

            try
            {
                using (var client = new HttpClient(hndlr))
                {
                    client.BaseAddress = new Uri(uri);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var token = await this.authService.GetAuthToken();
                    if (token != null)
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    var jsonContent = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json");
                    var response = await client.PutAsync(uri, jsonContent);

                    return response.IsSuccessStatusCode;
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return false;
        }

        public async Task<TResponse> PostAsync<TBody, TResponse>(string uri, TBody body)
        {
            var hndlr = new HttpClientHandler { UseDefaultCredentials = true };

            try
            {
                using (var client = new HttpClient(hndlr))
                {
                    client.BaseAddress = new Uri(uri);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var token = await this.authService.GetAuthToken();
                    if (token != null)
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    var jsonContent = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(uri, jsonContent);

                    if (response.IsSuccessStatusCode && response.StatusCode != HttpStatusCode.NoContent)
                    {
                        using (var content = response.Content)
                        {
                            var contents = await content.ReadAsStringAsync();

                            if (!string.IsNullOrEmpty(contents))
                            {
                                return JsonConvert.DeserializeObject<TResponse>(contents);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return default(TResponse);
        }
    }
}
