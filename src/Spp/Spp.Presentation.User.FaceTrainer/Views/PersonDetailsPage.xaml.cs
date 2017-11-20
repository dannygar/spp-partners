/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using FaceAPITrainer.Models;

namespace FaceAPITrainer.Views
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Threading.Tasks;

    using FaceAPITrainer.Controls;

    using Microsoft.ProjectOxford.Face.Contract;

    using Windows.Storage;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Controls.Primitives;
    using Windows.UI.Xaml.Input;
    using Windows.UI.Xaml.Media.Imaging;
    using Windows.UI.Xaml.Navigation;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PersonDetailsPage
    {
        public PersonDetailsPage()
        {
            this.InitializeComponent();
        }

        public Person CurrentPerson { get; set; }

        public PersonGroup CurrentPersonGroup { get; set; }

        public User[] Users { get; set; }

        public ObservableCollection<PersonFace> PersonFaces { get; set; }

        public string HeaderText { get; set; }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            Tuple<PersonGroup, Person> pageParameter = e.Parameter as Tuple<PersonGroup, Person>;

            this.CurrentPerson = pageParameter.Item2;
            this.CurrentPersonGroup = pageParameter.Item1;
            this.HeaderText = string.Format("{0}/{1}", pageParameter.Item1.Name, pageParameter.Item2.Name);
            this.PersonFaces = new ObservableCollection<PersonFace>();
            this.bingSearchControl.DefaultSearchQuery = this.CurrentPerson.Name;

            this.DataContext = this;

            await this.LoadPersonFacesFromService();

            base.OnNavigatedTo(e);
        }

        private async Task LoadPersonFacesFromService()
        {
            this.progressControl.IsActive = true;

            this.PersonFaces.Clear();

            try
            {
                Person latestVersionOfCurrentPerson = await FaceServiceHelper.GetPersonAsync(this.CurrentPersonGroup.PersonGroupId, this.CurrentPerson.PersonId);
                this.CurrentPerson.PersistedFaceIds = latestVersionOfCurrentPerson.PersistedFaceIds;

                if (this.CurrentPerson.PersistedFaceIds != null)
                {
                    foreach (Guid face in this.CurrentPerson.PersistedFaceIds)
                    {
                        PersonFace personFace = await FaceServiceHelper.GetPersonFaceAsync(this.CurrentPersonGroup.PersonGroupId, this.CurrentPerson.PersonId, face);
                        this.PersonFaces.Add(personFace);
                    }
                }
            }
            catch (Exception e)
            {
                await Util.GenericApiCallExceptionHandler(e, "Failure downloading person faces");
            }

            this.progressControl.IsActive = false;
        }

        private async void OnImageSearchCompleted(object sender, IEnumerable<ImageWithFace> args)
        {
            this.progressControl.IsActive = true;

            this.trainingImageCollectorFlyout.Hide();

            bool foundError = false;
            Exception lastError = null;
            foreach (var item in args)
            {
                try
                {
                    if (item.LocalImageFile != null)
                    {
                        var facePhotosFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync(
                            "facePhotos",
                            CreationCollisionOption.ReplaceExisting);

                        var file = await item.LocalImageFile.CopyAsync(facePhotosFolder);

                        await FaceServiceHelper.AddPersonFaceAsync(
                            this.CurrentPersonGroup.PersonGroupId,
                            this.CurrentPerson.PersonId,
                            imageStream: await file.OpenStreamForReadAsync(),
                            userData: file.Path,
                            targetFace: null);
                    }
                    else
                    {
                        //Face API Bug Workaround to avoid shortening of URL when adding a face
                        //Get the image stream
                        var facePhotosFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync(
                            "facePhotos",
                            CreationCollisionOption.OpenIfExists);
                        //Get file extension
                        var ext = item.ImageUrl.Contains("jpg")
                            ? "jpg" : item.ImageUrl.Contains("png") ? "png" : "img";

                        var imagePath = $"{facePhotosFolder.Path}\\{Guid.NewGuid()}.{ext}";

                        //Download the image from the online to local storage
                        await WebUtils.DownloadAsync(new Uri(item.ImageUrl), imagePath);

                        //Obtain the local image stream
                        var image = await StorageFile.GetFileFromPathAsync(imagePath);

                        //Add image to the store
                        await FaceServiceHelper.AddPersonFaceAsync(
                            this.CurrentPersonGroup.PersonGroupId,
                            this.CurrentPerson.PersonId,
                            imageStream: await image.OpenStreamForReadAsync(),
                            userData: image.Path,
                            targetFace: null);

                        //await FaceServiceHelper.AddPersonFaceAsync(
                        //    this.CurrentPersonGroup.PersonGroupId,
                        //    this.CurrentPerson.PersonId,
                        //    imageUrl: item.ImageUrl,
                        //    userData: item.ImageUrl,
                        //    targetFace: null);

                    }
                }
                catch (Exception e)
                {
                    foundError = true;
                    lastError = e;
                }
            }

            if (foundError)
            {
                await Util.GenericApiCallExceptionHandler(lastError, "Failure adding one or more of the faces");
            }

            await this.LoadPersonFacesFromService();

            this.progressControl.IsActive = false;
        }

        private async void OnDeletePersonClicked(object sender, RoutedEventArgs e)
        {
            await Util.ConfirmActionAndExecute("Delete person?", async () => { await DeletePersonAsync(); });
        }

        private async Task DeletePersonAsync()
        {
            try
            {
                await FaceServiceHelper.DeletePersonAsync(this.CurrentPersonGroup.PersonGroupId, this.CurrentPerson.PersonId);
                this.Frame.GoBack();
            }
            catch (Exception ex)
            {
                await Util.GenericApiCallExceptionHandler(ex, "Failure deleting person");
            }
        }

        private void OnImageSearchCanceled(object sender, EventArgs e)
        {
            this.trainingImageCollectorFlyout.Hide();
        }

        private async void DeleteSelectedImagesClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (var item in this.imagesGridView.SelectedItems)
                {
                    await FaceServiceHelper.DeletePersonFaceAsync(
                        this.CurrentPersonGroup.PersonGroupId,
                        this.CurrentPerson.PersonId,
                        ((PersonFace)item).PersistedFaceId);
                }

                await this.LoadPersonFacesFromService();
            }
            catch (Exception ex)
            {
                await Util.GenericApiCallExceptionHandler(ex, "Failure deleting images");
            }
        }

        private void ImageRightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
        }

        private async void OnImageDataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            PersonFace dataContext = sender.DataContext as PersonFace;

            if (dataContext != null)
            {
                Image image = sender as Image;
                if (image != null)
                {
                    BitmapImage bitmapImage = new BitmapImage();
                    image.Source = bitmapImage;

                    try
                    {
                        if (Path.IsPathRooted(dataContext.UserData))
                        {
                            // local file
                            bitmapImage.SetSource(await(await StorageFile.GetFileFromPathAsync(dataContext.UserData)).OpenReadAsync());
                        }
                        else
                        {
                            // url
                            bitmapImage.UriSource = new Uri(dataContext.UserData);
                        }
                    }
                    catch (Exception)
                    {
                        // reviewed
                    }
                }
            }
        }

        private void OnImageSearchLocalFilesProvided(object sender, EventArgs e)
        {
            this.trainingImageCollectorFlyout.ShowAt(this.AddFacesAppBarButton);
        }
    }
}
