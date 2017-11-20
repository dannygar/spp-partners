/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿namespace FaceAPITrainer.Views
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.ProjectOxford.Face.Contract;

    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Controls.Primitives;
    using Windows.ApplicationModel.DataTransfer;

    public sealed partial class PersonGroupsPage : Page
    {
        public PersonGroupsPage()
        {
            this.InitializeComponent();
        }

        private async Task LoadPersonGroupsFromService()
        {
            this.progressControl.IsActive = true;

            IEnumerable<PersonGroup> personGroups = new List<PersonGroup>();

            try
            {
                var key = string.IsNullOrWhiteSpace(SettingsHelper.Instance.WorkspaceKey)
                      ? null
                      : SettingsHelper.Instance.WorkspaceKey;

                personGroups = await FaceServiceHelper.GetPersonGroupsAsync(key);

                this.DataContext = personGroups.OrderBy(pg => pg.Name);
            }
            catch (Exception ex)
            {
                await Util.GenericApiCallExceptionHandler(ex, "Failure downloading groups");
            }

            this.progressControl.IsActive = false;

            if (!personGroups.Any())
            {
                await Util.GenericWarningHandler("There is no people groups found.", "Please, check that WorkspaceKey on Settings page fulfilled correctly.");
            }
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Frame.Navigate(
                typeof(PersonGroupDetailsPage),
                e.ClickedItem,
                new Windows.UI.Xaml.Media.Animation.DrillInNavigationTransitionInfo());
        }

        private async void OnAddPersonGroupButtonClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(SettingsHelper.Instance.WorkspaceKey))
                {
                    throw new InvalidOperationException("Before you can create groups you need to define a Workspace Key in the Settings Page.");
                }

                await FaceServiceHelper.CreatePersonGroupAsync(Guid.NewGuid().ToString(), this.personGroupNameTextBox.Text, SettingsHelper.Instance.WorkspaceKey);
                await this.LoadPersonGroupsFromService();
                this.personGroupNameTextBox.Text = "";
                this.addPersonGroupFlyout.Hide();
            }
            catch (Exception ex)
            {
                await Util.GenericApiCallExceptionHandler(ex, "Failure creating group");
            }

        }

        private void OnCancelAddPersonGroupButtonClicked(object sender, RoutedEventArgs e)
        {
            this.personGroupNameTextBox.Text = "";
            this.addPersonGroupFlyout.Hide();
        }

        private async void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            await this.LoadPersonGroupsFromService();
        }

        private void CopyPersonGroupId_Click(object sender, RoutedEventArgs e)
        {
            var datacontext = (PersonGroup)(e.OriginalSource as FrameworkElement).DataContext;

            var dataPackage = new DataPackage();
            dataPackage.RequestedOperation = DataPackageOperation.Copy;
            dataPackage.SetText(datacontext.PersonGroupId);

            Clipboard.SetContent(dataPackage);
        }

        private void StackPanel_RightTapped(object sender, Windows.UI.Xaml.Input.RightTappedRoutedEventArgs e)
        {
            var senderElement = sender as FrameworkElement;
            var flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement);
            flyoutBase.ShowAt(senderElement);
        }
    }
}
