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
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Newtonsoft.Json;

namespace MicrosoftSportsScience.Services
{
    public class ApiClientService : IHttpClientService
    {
        private readonly ILogService logService;
        private readonly IApiAuthService authService;

        public ApiClientService(ILogService logService, IApiAuthService authService)
        {
            this.logService = logService;
            this.authService = authService;
        }


        public async Task<T> GetItemAsync<T>(string apiEndpoint, string query = null)
        {
            var hndlr = new HttpClientHandler { UseDefaultCredentials = true };

            //Construct URL
            var uri = BuildUrl(apiEndpoint, query);

            try
            {
                using (var client = new HttpClient(hndlr))
                {
                    client.BaseAddress = uri;
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
                    else if (response.StatusCode != HttpStatusCode.NoContent)
                    {
                        this.logService.Warning($"Endpoint {uri} returned status code: {response.StatusCode}", this);
                    }
                }
            }
            catch (Exception ex)
            {
                this.logService.Error(string.Format("Unable to GET data from endpoint: {0}, error: {1}", uri, ex.ToString()), this);
                await new MessageDialog(ex.Message, "Network Error").ShowAsync();
            }

            return default(T);
        }

        public async Task<bool> SetItemAsync<T>(string apiEndpoint, T item, string query = null)
        {
            var hndlr = new HttpClientHandler { UseDefaultCredentials = true };

            //Construct URL
            var uri = BuildUrl(apiEndpoint, query);

            try
            {
                using (var client = new HttpClient(hndlr))
                {
                    client.BaseAddress = uri;
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
                this.logService.Error(string.Format("Unable to POST data to endpoint: {0}, error: {1}", uri, ex.ToString()), this);
                await new MessageDialog(ex.Message, "Network Error").ShowAsync();
            }

            return false;
        }

        public async Task<int> SetItemAndReturnIdAsync<T>(string apiEndpoint, T item, string query = null)
        {
            var hndlr = new HttpClientHandler { UseDefaultCredentials = true };

            //Construct URL
            var uri = BuildUrl(apiEndpoint, query);

            try
            {
                using (var client = new HttpClient(hndlr))
                {
                    client.BaseAddress = uri;
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var token = await this.authService.GetAuthToken();
                    if (token != null)
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    var jsonContent = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(uri, jsonContent);

                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        int r;
                        return int.TryParse(result, out r) ? r : -1;
                    }
                }
            }
            catch (Exception ex)
            {
                this.logService.Error(string.Format("Unable to POST data to endpoint: {0}, error: {1}", uri, ex.ToString()), this);
                await new MessageDialog(ex.Message, "Network Error").ShowAsync();
            }

            return -1;
        }

        public async Task<bool> PutItemAsync<T>(string apiEndpoint, T item, string query = null)
        {
            var hndlr = new HttpClientHandler { UseDefaultCredentials = true };


            //Construct URL
            var uri = BuildUrl(apiEndpoint, query);

            try
            {
                using (var client = new HttpClient(hndlr))
                {
                    client.BaseAddress = uri;
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
                this.logService.Error(string.Format("Unable to PUT data to endpoint: {0}, error: {1}", uri, ex.ToString()), this);
                await new MessageDialog(ex.Message, "Network Error").ShowAsync();
            }

            return false;
        }




        public async Task<TResponse> PostAsync<TBody, TResponse>(string apiEndpoint, TBody body, string query = null)
        {
            var hndlr = new HttpClientHandler { UseDefaultCredentials = true };

            //Construct URL
            var uri = BuildUrl(apiEndpoint, query);

            try
            {
                using (var client = new HttpClient(hndlr))
                {
                    client.BaseAddress = uri;
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
                    else if (response.StatusCode != HttpStatusCode.NoContent)
                    {
                        this.logService.Warning($"Endpoint {uri} returned status code: {response.StatusCode}", this);
                    }
                }
            }
            catch (Exception ex)
            {
                this.logService.Error(string.Format("Unable to PUT data to endpoint: {0}, error: {1}", uri, ex.ToString()), this);
                await new MessageDialog(ex.Message, "Network Error").ShowAsync();
            }

            return default(TResponse);
        }



        private Uri BuildUrl(string endpoint, string query)
        {
            var builder = new UriBuilder(new Uri(Defines.API_BASE_URL));
            builder.Path = endpoint;
            builder.Query = query ?? "";

            return builder.Uri;
        }
    }
}
