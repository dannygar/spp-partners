/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
// 
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license.
// 
// Microsoft Cognitive Services: http://www.microsoft.com/cognitive
// 
// Copyright (c) Microsoft Corporation
// All rights reserved.
// 
// MIT License:
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED ""AS IS"", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 

using System;
using System.ComponentModel;
using Newtonsoft.Json;
using Windows.Storage;

namespace MicrosoftSportsScience.Helpers
{
    internal class CSSettingsHelper : INotifyPropertyChanged
    {
        public event EventHandler SettingsChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        private static CSSettingsHelper instance;

        static CSSettingsHelper()
        {
            instance = new CSSettingsHelper();
        }

        public void Initialize()
        {
            LoadRoamingSettings();
            Windows.Storage.ApplicationData.Current.DataChanged += RoamingDataChanged;
        }

        private void RoamingDataChanged(ApplicationData sender, object args)
        {
            LoadRoamingSettings();
            instance.OnSettingsChanged();
        }

        private void OnSettingsChanged()
        {
            if (instance.SettingsChanged != null)
            {
                instance.SettingsChanged(instance, EventArgs.Empty);
            }
        }

        private void OnSettingChanged(string propertyName, object value)
        {
            ApplicationData.Current.RoamingSettings.Values[propertyName] = value;

            instance.OnSettingsChanged();
            instance.OnPropertyChanged(propertyName);
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (instance.PropertyChanged != null)
            {
                instance.PropertyChanged(instance, new PropertyChangedEventArgs(propertyName));
            }
        }

        public static CSSettingsHelper Instance
        {
            get
            {
                return instance;
            }
        }

        private void LoadRoamingSettings()
        {
            object value = ApplicationData.Current.RoamingSettings.Values["FaceApiKey"];
            if (value != null)
            {
                this.FaceApiKey = value.ToString();
            }

            value = ApplicationData.Current.RoamingSettings.Values["Location"];
            if (value != null)
            {
                this.Location = value.ToString();
            }


            value = ApplicationData.Current.RoamingSettings.Values["EmotionApiKey"];
            if (value != null)
            {
                this.EmotionApiKey = value.ToString();
            }

            value = ApplicationData.Current.RoamingSettings.Values["WorkspaceKey"];
            if (value != null)
            {
                this.WorkspaceKey = value.ToString();
            }

            value = ApplicationData.Current.RoamingSettings.Values["CameraName"];
            if (value != null)
            {
                this.CameraName = value.ToString();
            }

            value = ApplicationData.Current.RoamingSettings.Values["MinDetectableFaceCoveragePercentage"];
            if (value != null)
            {
                uint size;
                if (uint.TryParse(value.ToString(), out size))
                {
                    this.MinDetectableFaceCoveragePercentage = size;
                }
            }

            value = ApplicationData.Current.RoamingSettings.Values["EnableFaceRecognition"];
            if (value != null)
            {
                bool booleanValue;
                if (bool.TryParse(value.ToString(), out booleanValue))
                {
                    this.EnableFaceRecognition = booleanValue;
                }
            }

        }

        public void RestoreAllSettings()
        {
            ApplicationData.Current.RoamingSettings.Values.Clear();
        }

        public bool IsValidSettings()
        {
            return !(string.IsNullOrEmpty(WorkspaceKey)
                     || string.IsNullOrEmpty(FaceApiKey)
                     || string.IsNullOrEmpty(Location)
                     || string.IsNullOrEmpty(EmotionApiKey));
        }


        private string faceApiKey = string.Empty;
        public string FaceApiKey
        {
            get { return this.faceApiKey; }
            set
            {
                this.faceApiKey = value;
                this.OnSettingChanged("FaceApiKey", value);
            }
        }


        private string _cognitiveServicesDcLocation = string.Empty;
        [JsonProperty]
        public string Location
        {
            get { return _cognitiveServicesDcLocation; }
            set
            {
                this._cognitiveServicesDcLocation = value;
                this.OnSettingChanged("Location", value);
            }
        }



        private string emotionApiKey = string.Empty;
        public string EmotionApiKey
        {
            get { return this.emotionApiKey; }
            set
            {
                this.emotionApiKey = value;
                this.OnSettingChanged("EmotionApiKey", value);
            }
        }


        private string workspaceKey = string.Empty;
        public string WorkspaceKey
        {
            get { return workspaceKey; }
            set
            {
                this.workspaceKey = value;
                this.OnSettingChanged("WorkspaceKey", value);
            }
        }

        private string cameraName = string.Empty;
        public string CameraName
        {
            get { return cameraName; }
            set
            {
                this.cameraName = value;
                this.OnSettingChanged("CameraName", value);
            }
        }

        private uint minDetectableFaceCoveragePercentage = 7;
        public uint MinDetectableFaceCoveragePercentage
        {
            get { return this.minDetectableFaceCoveragePercentage; }
            set
            {
                this.minDetectableFaceCoveragePercentage = value;
                this.OnSettingChanged("MinDetectableFaceCoveragePercentage", value);
            }
        }

        private bool enableFaceRecognition = false;
        public bool EnableFaceRecognition
        {
            get { return enableFaceRecognition; }
            set
            {
                this.enableFaceRecognition = value;
                this.OnSettingChanged("EnableFaceRecognition", value);
            }
        }


        private bool showDebugInfo = false;
        public bool ShowDebugInfo
        {
            get { return showDebugInfo; }
            set
            {
                this.showDebugInfo = value;
                this.OnSettingChanged("ShowDebugInfo", value);
            }
        }


    }
}
