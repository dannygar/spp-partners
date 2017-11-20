/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using MicrosoftSportsScience.Data;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using MicrosoftSportsScience.Services;
using MicrosoftSportsScience.ViewModels;

namespace MicrosoftSportsScience.Models
{
    class RecommendedLoadModel : BaseModel
    {
        private ILogService _logService;

        public RecommendedLoadModel(ILogService logService)
        {
            _logService = logService;
        }

        public async Task<double> InvokeRequestResponseService()
        {
            var scoreRequest = CreateScoreRequest(
                                DateTime.Today.DayOfWeek.ToString(),
                                "Hayden Evans",
                                ((int)DateTime.Today.DayOfWeek - 6).ToString(),
                                "0",
                                "60",
                                "5",
                                "300",
                                "79",
                                "88",
                                "7",
                                "6",
                                "8",
                                "8",
                                "5",
                                "25",
                                "0",
                                "206.75",
                                "133",
                                "0.64",
                                "-0.498643",
                                "1"
                         );

            return await InvokeRequestResponseService(scoreRequest);
        }

        public async Task<double> InvokeRequestResponseService(ScoreRequest scoreRequest)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    //Load configuration settings
                    await AzureADv2AuthService.LoadConfig();


                    string apiKey = AzureADv2AuthService.AppSettings.AppConfigurationSettings.MLClientKey;
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

                    client.BaseAddress = new Uri(AzureADv2AuthService.AppSettings.AppConfigurationSettings.MLEndpointUrl);

                    var jobject = JObject.FromObject(scoreRequest);
                    var content = new StringContent(jobject.ToString(), Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync("", content);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResult = await response.Content.ReadAsStringAsync();
                        var result = JObject.Parse(jsonResult).ToObject<ScoreRequestResult>();
                        var table = result.Results.output1.value.Values;
                        return Double.Parse(table[table.GetLowerBound(0), table.GetUpperBound(1)]);
                    }
                    else
                    {
                        _logService.Error(string.Format("The request failed with status code: {0}", response.StatusCode), this);

                        // Print the headers - they include the requert ID and the timestamp, which are useful for debugging the failure
                        _logService.Error(response.Headers.ToString(), this);

                        string responseContent = await response.Content.ReadAsStringAsync();
                        _logService.Error(responseContent, this);
                        return 0;
                    }
                }

            }
            catch (Exception e)
            {
                _logService.Error(e.ToString(), this);
                return 0;
            }
        }

        public ScoreRequest CreateScoreRequest(string WeekDay, string PlayerName, string DaysBeforeGame, string PlayerLoad, string TrainingDuration, string RPE, string TrainingRPE, string HRV, string SleepQualityTonight, string Sleep, string Energy, string Soreness, string Stress, string Hydration, string Recoverymin, string WorkourVolume, string PlayerLoadCRoll, string PlayerLoadARoll, string AcuteChronic, string AcuteChronicZ, string GoodPractice)
        {
            ScoreRequest scoreRequest = new ScoreRequest();
            scoreRequest.Inputs.input1 = new Input1
            {
                ColumnNames = new string[]
                {
                            "Weekday",
                            "PlayerName",
                            "DaysBeforeGame",
                            "PlayerLoad",
                            "TrainingDuration",
                            "RPE",
                            "TrainingRPE",
                            "HRV",
                            "SleepQualityTonight",
                            "Sleep",
                            "Energy",
                            "Soreness",
                            "Stress",
                            "Hydration",
                            "Recoverymins",
                            "WorkoutVolume",
                            "PlayerLoadcroll",
                            "PlayerLoadaroll",
                            "acutechronic",
                            "acutechronicz",
                            "GoodPractice",
                },

                Values = new string[,]
                {
                            {
                                WeekDay,
                                PlayerName,
                                DaysBeforeGame,
                                PlayerLoad,
                                TrainingDuration,
                                RPE,
                                TrainingRPE,
                                HRV,
                                SleepQualityTonight,
                                Sleep,
                                Energy,
                                Soreness,
                                Stress,
                                Hydration,
                                Recoverymin,
                                WorkourVolume,
                                PlayerLoadCRoll,
                                PlayerLoadARoll,
                                AcuteChronic,
                                AcuteChronicZ,
                                GoodPractice,
                                },
                    },
            };

            return scoreRequest;
        }
    }
}
