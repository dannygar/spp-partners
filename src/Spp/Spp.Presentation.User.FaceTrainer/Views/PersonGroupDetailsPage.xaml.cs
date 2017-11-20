/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using FaceAPITrainer.Models;
using FaceAPITrainer.Services;

namespace FaceAPITrainer.Views
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Microsoft.ProjectOxford.Face.Contract;

    using Windows.UI.Popups;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Navigation;
    using Windows.ApplicationModel.DataTransfer;
    using Windows.UI.Xaml.Media.Imaging;
    using System.IO;
    using Windows.Storage;
    using Controls;/// <summary>
                   /// An empty page that can be used on its own or navigated to within a Frame.
                   /// </summary>
    public sealed partial class PersonGroupDetailsPage
    {
        private ITypedDataService _dataService;

        public PersonGroupDetailsPage()
        {
            this.InitializeComponent();
            this._dataService = new HttpClientService(new AzureADApiAuthService());
        }

        public PersonGroup CurrentPersonGroup { get; set; }

        public ObservableCollection<Person> PersonsInCurrentGroup { get; set; }

        public User[] Users { get; set; }

        private User CurrentUser { get; set; }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            this.CurrentPersonGroup = e.Parameter as PersonGroup;

            this.PersonsInCurrentGroup = new ObservableCollection<Person>();

            await this.LoadPersonsInCurrentGroup();
            await this.LoadUsersFromTeamPerformanceService();

            this.DataContext = this;

            base.OnNavigatedTo(e);
        }

        private async Task LoadUsersFromTeamPerformanceService()
        {
            this.Users = new User[] { };

            try
            {
                this.Users = await GetUsersAsync(SettingsHelper.Instance.TeamId);
            }
            catch (HttpRequestException e)
            {
                await Util.GenericErrorHandler("Receiving list of users from service failed.", e.Message);
            }
            catch (Exception e)
            {
                await Util.GenericApiCallExceptionHandler(e, "Receiving list of users from service failed.");
            }
        }

        private async Task LoadPersonsInCurrentGroup()
        {
            this.PersonsInCurrentGroup.Clear();

            try
            {
                Person[] personsInGroup = await FaceServiceHelper.GetPersonsAsync(this.CurrentPersonGroup.PersonGroupId);
                foreach (Person person in personsInGroup.OrderBy(p => p.Name))
                {
                    this.PersonsInCurrentGroup.Add(person);
                }
            }
            catch (Exception e)
            {
                await Util.GenericApiCallExceptionHandler(e, "Failure loading people in the group");
            }
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Frame.Navigate(
                typeof(PersonDetailsPage),
                new Tuple<PersonGroup, Person>(this.CurrentPersonGroup, e.ClickedItem as Person),
                new Windows.UI.Xaml.Media.Animation.DrillInNavigationTransitionInfo());
        }

        private async void OnAddPersonButtonClicked(object sender, RoutedEventArgs e)
        {
            await this.CreatePersonAsync(this.personNameTextBox.Text, this.CurrentUser?.Id.ToString());

            this.CurrentUser = null;
        }

        private void OnCancelAddPersonButtonClicked(object sender, RoutedEventArgs e)
        {
            this.DismissFlyout();
        }

        private void OnPersonNameTextBoxChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var matchingUsers = this.GetMatchingUsers(sender.Text);
                sender.ItemsSource = matchingUsers.ToList();
            }
        }

        private void OnPersonNameSuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            sender.Text = args.SelectedItem.ToString();
        }

        private void OnPersonNameQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion != null)
            {
                SelectUser(args.ChosenSuggestion as User);
            }
            else
            {
                var matchingUsers = this.GetMatchingUsers(args.QueryText);
                if (matchingUsers.Any())
                {
                    SelectUser(matchingUsers.FirstOrDefault());
                }

                this.CurrentUser = (User)args.ChosenSuggestion;
            }
        }

        private async Task<CreatePersonResult> CreatePersonAsync(string name, string userData)
        {
            try
            {
                var result = await FaceServiceHelper.CreatePersonAsync(
                    this.CurrentPersonGroup.PersonGroupId,
                    Util.CapitalizeString(name),
                    userData);

                await this.LoadPersonsInCurrentGroup();

                this.DismissFlyout();

                return result;
            }
            catch (Exception e)
            {
                await Util.GenericApiCallExceptionHandler(e, "Failure creating person");

                return null;
            }
        }

        private void DismissFlyout()
        {
            this.addPersonFlyout.Hide();
            this.personNameTextBox.Text = string.Empty;
        }



        private async Task<User[]> GetUsersAsync(int teamId)
        {
            var url = $"{Defines.API_BASE_URL}{Defines.API_TEAM_ENDPOINT}/{teamId}";

            var team = await this._dataService.GetItemAsync<Team>(url);

            var users = (team?.Users).ToArray();

            return users;
        }

        private ICollection<User> GetMatchingUsers(string queryText)
        {
            return this.Users
                .Where(x => (x.FullName)
                    .ToUpper()
                    .Trim(' ')
                    .Contains(queryText.Trim(' ').ToUpper()))
                .ToList();
        }

        private void SelectUser(User user)
        {
            if (user != null)
            {
                personNameTextBox.Text = $"{user.FullName}";
                this.CurrentUser = user;
            }
        }

        private async void OnDeletePersonGroupClicked(object sender, RoutedEventArgs e)
        {
            await Util.ConfirmActionAndExecute("Delete person group?", async () => { await DeletePersonGroupAsync(); });
        }

        private void OnCopyPersonGroupIdClicked(object sender, RoutedEventArgs e)
        {
            var dataPackage = new DataPackage();
            dataPackage.RequestedOperation = DataPackageOperation.Copy;
            dataPackage.SetText(this.CurrentPersonGroup.PersonGroupId);

            Clipboard.SetContent(dataPackage);
        }

        private async Task DeletePersonGroupAsync()
        {
            try
            {
                await FaceServiceHelper.DeletePersonGroupAsync(this.CurrentPersonGroup.PersonGroupId);
                this.Frame.GoBack();
            }
            catch (Exception ex)
            {
                await Util.GenericApiCallExceptionHandler(ex, "Failure deleting person group");
            }
        }

        private async void OnStartTrainingClicked(object sender, RoutedEventArgs e)
        {
            this.progressControl.IsActive = true;

            TrainingStatus trainingStatus = null;
            try
            {
                await FaceServiceHelper.TrainPersonGroupAsync(this.CurrentPersonGroup.PersonGroupId);

                while (true)
                {
                    trainingStatus = await FaceServiceHelper.GetPersonGroupTrainingStatusAsync(this.CurrentPersonGroup.PersonGroupId);

                    if (trainingStatus.Status != Status.Running)
                    {
                        break;
                    }

                    await Task.Delay(1000);
                }
            }
            catch (Exception ex)
            {
                await Util.GenericApiCallExceptionHandler(ex, "Failure requesting training");
            }

            this.progressControl.IsActive = false;

            if (trainingStatus.Status != Status.Succeeded)
            {
                await new MessageDialog("Training finished with failure.").ShowAsync();
            }
        }

        private async void OnImportUsers(object sender, RoutedEventArgs e)
        {
            if (!this.Users.Any())
            {
                await Util.GenericWarningHandler("Unable to Import.", "There is no any users had been loaded. Please, try to repeat a bit later.");
                return;
            }

            var persons = await FaceServiceHelper.GetPersonsAsync(this.CurrentPersonGroup.PersonGroupId);

            var existedUserIds = persons.Select(x => x.UserData);

            var newUsers = this.Users.Where(x => !existedUserIds.Contains(x.Id.ToString()));

            foreach (var user in newUsers)
            {
                try
                {
                    var person = await this.CreatePersonAsync($"{user.FullName}", user?.Id.ToString());

                    var remotePhotoStream = await Util.GetPhoto(user.PathtoPhoto);

                    var facePhotosFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync("facePhotos", CreationCollisionOption.OpenIfExists);

                    var photoFile = await facePhotosFolder.CreateFileAsync(Guid.NewGuid().ToString());

                    using (var localPhotoStream = await photoFile.OpenStreamForWriteAsync())
                    {
                        await remotePhotoStream.CopyToAsync(localPhotoStream);
                    }

                    await Task.Delay(100);

                    await FaceServiceHelper.AddPersonFaceAsync(
                        this.CurrentPersonGroup.PersonGroupId,
                        person.PersonId,
                        imageStream: await photoFile.OpenStreamForReadAsync(),
                        userData: photoFile.Path,
                        targetFace: null);

                    await Task.Delay(100);
                }
                catch (Exception)
                {
                    continue;
                }
            }
        }

        private async void OnDeleteUsers(object sender, RoutedEventArgs e)
        {
            var persons = await FaceServiceHelper.GetPersonsAsync(this.CurrentPersonGroup.PersonGroupId);

            foreach (var person in persons)
            {
                try
                {
                    await FaceServiceHelper.DeletePersonAsync(this.CurrentPersonGroup.PersonGroupId, person.PersonId);

                    await Task.Delay(100);
                }
                catch (Exception)
                {
                    continue;
                }
            }
        }
    }
}
