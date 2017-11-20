/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System.Linq;

namespace FaceAPITrainer
{
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;

    class BingSearchHelper
    {
        private static string ImageSearchEndPoint;
        private static string AutoSuggestionEndPoint;

        static BingSearchHelper()
        {
            SettingsHelper.Instance.SettingsChanged += (e, args) =>
            {
                InitializeApiEndPoints();
            };

            InitializeApiEndPoints();
        }

        private static void InitializeApiEndPoints()
        {
            ImageSearchEndPoint = "https://api.cognitive.microsoft.com/bing/v5.0/images/search";
            //ImageSearchEndPoint = "https://www.bing.com/api/v4/images/search?appid=" + SettingsHelper.Instance.BingApiKey;
            AutoSuggestionEndPoint = "https://www.bing.com/as/api/v4/Suggestions/images?appid=" + SettingsHelper.Instance.BingApiKey;
        }

        public static bool BingIsConfigured => !string.IsNullOrWhiteSpace(SettingsHelper.Instance.BingApiKey);

        public static async Task<IEnumerable<string>> GetImageSearchResults(string query, string imageContent = "Face", int count = 20, int offset = 0)
        {
            List<string> urls = new List<string>();

            using (var client = new HttpClient())
            {
                var url = $"{ImageSearchEndPoint}?q={query}&count={count}&safeSearch=Strict";
                client.BaseAddress = new Uri(url);
                client.Timeout = TimeSpan.FromSeconds(5);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", $"{SettingsHelper.Instance.BingApiKey}");
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                // new bing api
                var result = await client.GetAsync(url);
                result.EnsureSuccessStatusCode();
                var json = await result.Content.ReadAsStringAsync();
                dynamic data = JObject.Parse(json);
                var valueTokenEnumerable =
                    ((Newtonsoft.Json.Linq.JContainer) data).Where(
                        x => ((Newtonsoft.Json.Linq.JProperty) x).Name == "value");
                var valueToken = valueTokenEnumerable.FirstOrDefault();
                foreach (var imageCol in ((Newtonsoft.Json.Linq.JArray)((Newtonsoft.Json.Linq.JProperty)valueToken).Value))
                {
                    var contentUrlProperty =
                        ((Newtonsoft.Json.Linq.JContainer) imageCol).Where(
                            property => ((Newtonsoft.Json.Linq.JProperty) property).Name == "contentUrl")
                                .Select(x => ((Newtonsoft.Json.Linq.JValue)((Newtonsoft.Json.Linq.JProperty)x).Value))
                                .FirstOrDefault();
                    if (contentUrlProperty != null)
                    {
                        var contentUrl = contentUrlProperty.Value;
                        urls.Add(contentUrl.ToString());
                    }
                }
            }

            return urls;
        }

        public static async Task<IEnumerable<string>> GetAutoSuggestResults(string query)
        {         
            List<string> suggestions = new List<string>();

            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(5);
                client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("FaceAPITrainer", "1"));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var result = await client.GetAsync(string.Format("{0}&q={1}", AutoSuggestionEndPoint, WebUtility.UrlEncode(query)));
                result.EnsureSuccessStatusCode();
                var json = await result.Content.ReadAsStringAsync();
                dynamic data = JObject.Parse(json);
                if (data.suggestionGroups != null && data.suggestionGroups.Count > 0 &&
                    data.suggestionGroups[0].suggestionContainers != null)
                {
                    for (int i = 0; i < data.suggestionGroups[0].suggestionContainers.Count; i++)
                    {
                        suggestions.Add(data.suggestionGroups[0].suggestionContainers[i].suggestion.name.Value);
                    }
                }
            }

            return suggestions;
        }
    }
}
