/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using System.ComponentModel;
using Windows.Storage;

namespace FaceAPITrainer
{
    class SettingsHelper : INotifyPropertyChanged
    {
        public event EventHandler SettingsChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        private static readonly SettingsHelper instance;

        static SettingsHelper()
        {
            instance = new SettingsHelper();
        }

        public void Initialize()
        {
            LoadRoamingSettings();
            ApplicationData.Current.DataChanged += RoamingDataChanged;
        }

        private void RoamingDataChanged(ApplicationData sender, object args)
        {
            LoadRoamingSettings();
            instance.OnSettingsChanged();
        }

        private void OnSettingsChanged()
        {
            instance.SettingsChanged?.Invoke(instance, EventArgs.Empty);
        }

        private void OnSettingChanged(string propertyName, object value)
        {
            ApplicationData.Current.RoamingSettings.Values[propertyName] = value;

            instance.OnSettingsChanged();
            instance.OnPropertyChanged(propertyName);


        }

        private void OnPropertyChanged(string propertyName)
        {
            instance.PropertyChanged?.Invoke(instance, new PropertyChangedEventArgs(propertyName));
        }

        public static SettingsHelper Instance => instance;

        private void LoadRoamingSettings()
        {
            object value = ApplicationData.Current.RoamingSettings.Values["TeamId"];
            if (value != null)
            {
                this.TeamId = (int)value;
            }


            value = ApplicationData.Current.RoamingSettings.Values["FaceApiKey"];
            if (value != null)
            {
                this.FaceApiKey = value.ToString();
            }

            value = ApplicationData.Current.RoamingSettings.Values["BingApiKey"];
            if (value != null)
            {
                this.BingApiKey = value.ToString();
            }

            value = ApplicationData.Current.RoamingSettings.Values["WorkspaceKey"];
            if (value != null)
            {
                this.WorkspaceKey = value.ToString();
            }

            value = ApplicationData.Current.RoamingSettings.Values["EmotionApiKey"];
            if (value != null)
            {
                this.EmotionApiKey = value.ToString();
            }

            value = ApplicationData.Current.RoamingSettings.Values["CameraName"];
            if (value != null)
            {
                this.CameraName = value.ToString();
            }

            value = ApplicationData.Current.RoamingSettings.Values["MinDetectableFaceCoveragePercentage"];
            if (value != null)
            {
                this.MinDetectableFaceCoveragePercentage = (int)value;
            }


            value = ApplicationData.Current.RoamingSettings.Values["Location"];
            if (value != null)
            {
                this.Location = value.ToString();
            }

        }

        public void RestoreAllSettings()
        {
            ApplicationData.Current.RoamingSettings.Values.Clear();
        }
        public int Id { get; set; }

        private int _teamId;
        public int TeamId
        {
            get => this._teamId;
            set
            {
                this._teamId = value;
                this.OnSettingChanged("TeamId", value);
            }
        }


        private string _faceApiKey = string.Empty;
        public string FaceApiKey
        {
            get => this._faceApiKey;
            set
            {
                this._faceApiKey = value;
                this.OnSettingChanged("FaceApiKey", value);
            }
        }

        private string _bingApiKey = string.Empty;
        public string BingApiKey
        {
            get => this._bingApiKey;
            set
            {
                this._bingApiKey = value;
                this.OnSettingChanged("BingApiKey", value);
            }
        }


        private string _emotionApiKey = string.Empty;
        public string EmotionApiKey
        {
            get => this._emotionApiKey;
            set
            {
                this._emotionApiKey = value;
                this.OnSettingChanged("EmotionApiKey", value);
            }
        }



        private string _workspaceKey;
        public string WorkspaceKey
        {
            get => _workspaceKey;
            set
            {
                this._workspaceKey = value;
                this.OnSettingChanged("WorkspaceKey", value);
            }
        }

        private string _cameraName = string.Empty;
        public string CameraName
        {
            get => _cameraName;
            set
            {
                this._cameraName = value;
                this.OnSettingChanged("CameraName", value);
            }
        }


        private int _minDetectableFaceCoveragePercentage;

        public int MinDetectableFaceCoveragePercentage
        {
            get => _minDetectableFaceCoveragePercentage;
            set
            {
                this._minDetectableFaceCoveragePercentage = value;
                this.OnSettingChanged("MinDetectableFaceCoveragePercentage", value);
            }
        }


        private string _cognitiveServicesDcLocation = string.Empty;
        public string Location
        {
            get => _cognitiveServicesDcLocation;
            set
            {
                this._cognitiveServicesDcLocation = value;
                this.OnSettingChanged("Location", value);
            }
        }


    }
}
