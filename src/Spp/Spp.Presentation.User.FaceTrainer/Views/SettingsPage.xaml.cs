/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FaceAPITrainer.Services;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace FaceAPITrainer.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        private ITypedDataService _dataService;

        private CoreDispatcher _dispatcher;

        private static Dictionary<string, string> _locations = new Dictionary<string, string>
        {
            { "West US", "westus" },
            { "East US 2", "eastus2" },
            { "West Central US", "eastus2" },
            { "West Europe", "westeurope" },
            { "Southeast Asia", "southeastasia" }
        };

        public bool IsNewWorkspace { get; set; }

        public SettingsPage()
        {
            this.InitializeComponent();
            this.DataContext = SettingsHelper.Instance;
            this._dataService = new HttpClientService(new AzureADApiAuthService());
            this._dispatcher = Windows.UI.Core.CoreWindow.GetForCurrentThread().Dispatcher;
            IsNewWorkspace = false;
            SettingsHelper.Instance.PropertyChanged += Settings_PropertyChanged;
        }

        private void Settings_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "WorkspaceKey")
            {
                IsNewWorkspace = true;
            }
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            this.cameraSourceComboBox.ItemsSource = await Util.GetAvailableCameraNamesAsync();
            this.cameraSourceComboBox.SelectedItem = SettingsHelper.Instance.CameraName;

            this.cognitiveServicesDCComboBox.ItemsSource = _locations.Keys;
            base.OnNavigatedFrom(e);
        }

        private void OnGenerateNewKeyClicked(object sender, RoutedEventArgs e)
        {
            SettingsHelper.Instance.WorkspaceKey = Guid.NewGuid().ToString();
        }

        private async void OnResetSettingsClick(object sender, RoutedEventArgs e)
        {
            await Util.ConfirmActionAndExecute("This will reset all the settings and erase your changes. Confirm?",
                async () =>
                {
                    await Task.Run(() => SettingsHelper.Instance.RestoreAllSettings());
                    await new MessageDialog("Settings restored. Please restart the application to load the default settings.").ShowAsync();
                });
        }

        private void OnCameraSourceSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.cameraSourceComboBox.SelectedItem != null)
            {
                SettingsHelper.Instance.CameraName = this.cameraSourceComboBox.SelectedItem.ToString();
            }
        }


        private void OnLocationSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.cognitiveServicesDCComboBox.SelectedItem != null)
            {
                SettingsHelper.Instance.Location = this.cognitiveServicesDCComboBox.SelectedItem.ToString();
            }
        }



        private async void OnSaveChangesClicked(object sender, RoutedEventArgs e)
        {
            await Util.ConfirmActionAndExecute("This will store all the changes in the team's database. Confirm?",
                async () =>
                {
                    await Task.Run(StoreChangesToDB);
                });
        }

        private async Task StoreChangesToDB()
        {
            var url = $"{Defines.API_BASE_URL}{Defines.API_COGNITIVESERVICES_ENDPOINT}" + ((IsNewWorkspace) ? "" : "/update");

            var csKeys = new CognitiveServiceKeys()
            {
                Id = SettingsHelper.Instance.Id,
                TeamId = SettingsHelper.Instance.TeamId,
                WorkspaceKey = SettingsHelper.Instance.WorkspaceKey,
                FaceApiKey = SettingsHelper.Instance.FaceApiKey,
                EmotionApiKey = SettingsHelper.Instance.EmotionApiKey,
                BingApiKey = SettingsHelper.Instance.BingApiKey,
                CameraName = SettingsHelper.Instance.CameraName,
                Location = SettingsHelper.Instance.Location,
                MinDetectableFaceCoveragePercentage = (uint)SettingsHelper.Instance.MinDetectableFaceCoveragePercentage
            };

            var result = await this._dataService.PostAsync<CognitiveServiceKeys, bool>(url, csKeys);

            await this._dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                if (result)
                {
                    var msg = new MessageDialog("The new settings have been successfully stored.", "Success");
                    await msg.ShowAsync();
                }
                else
                {
                    await Util.GenericWarningHandler("Cognitive Services Update",
                        "Failed to update the Cognitive Services with the new settings. Please, try again later.");
                }
            });
        }

    }


    public class CognitiveServiceKeys
    {
        public int Id { get; set; }
        public string WorkspaceKey { get; set; }
        public string FaceApiKey { get; set; }
        public string EmotionApiKey { get; set; }
        public string BingApiKey { get; set; }
        public string CameraName { get; set; }
        public uint MinDetectableFaceCoveragePercentage { get; set; }

        public string Location { get; set; }

        //Navigation keys
        public int TeamId { get; set; }

    }

}
