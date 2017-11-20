/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿namespace MicrosoftSportsScience.Helpers
{
    using Windows.ApplicationModel.Resources;
    using Windows.UI.Xaml;

    public class ResourceHelper
    {
        private static readonly ResourceLoader ResourceLoader = new ResourceLoader();

        public static class PerformanceRadar
        {
            public static string Player { get; } = GetString("PerformanceRadarPlayer");

            public static string Skill { get; } = GetString("PerformanceRadarSkill");

            public static string Team { get; } = GetString("PerformanceRadarTeam");
        }

        public static T GetApplicationResource<T>(string key)
            where T : class
        {
            return Application.Current.Resources.ContainsKey(key)
                ? Application.Current.Resources[key] as T
                : null;
        }

        public static string GetString(string key)
        {
            return ResourceLoader.GetString(key);
        }
    }
}
