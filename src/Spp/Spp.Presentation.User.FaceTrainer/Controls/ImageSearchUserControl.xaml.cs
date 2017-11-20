/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace FaceAPITrainer.Controls
{
    public sealed partial class ImageSearchUserControl : UserControl
    {
        public static readonly DependencyProperty ClearStateWhenClosedProperty =
            DependencyProperty.Register(
            "ClearStateWhenClosed",
            typeof(bool),
            typeof(ImageSearchUserControl),
            new PropertyMetadata(true)
            );

        public static readonly DependencyProperty DetectFacesOnLoadProperty =
            DependencyProperty.Register(
            "DetectFacesOnLoad",
            typeof(bool),
            typeof(ImageSearchUserControl),
            new PropertyMetadata(false)
            );

        public static readonly DependencyProperty DefaultSearchQueryProperty =
            DependencyProperty.Register(
            "DefaultSearchQuery",
            typeof(string),
            typeof(ImageSearchUserControl),
            new PropertyMetadata(string.Empty, OnDefaultSearchQueryChanged)
            );

        public static readonly DependencyProperty ImageContentTypeProperty =
            DependencyProperty.Register(
            "ImageContentType",
            typeof(string),
            typeof(ImageSearchUserControl),
            new PropertyMetadata("Face")
            );

        private static void OnDefaultSearchQueryChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ImageSearchUserControl).autoSuggestBox.Text = (string) e.NewValue;
        }

        public event EventHandler<IEnumerable<ImageWithFace>> OnImageSearchCompleted;
        public event EventHandler OnImageSearchCanceled;
        public event EventHandler OnImageSearchLocalFilesProvided;

        public bool DetectFacesOnLoad
        {
            get { return (bool)GetValue(DetectFacesOnLoadProperty); }
            set { SetValue(DetectFacesOnLoadProperty, (bool)value); }
        }

        public bool ClearStateWhenClosed
        {
            get { return (bool)GetValue(ClearStateWhenClosedProperty); }
            set { SetValue(ClearStateWhenClosedProperty, (bool)value); }
        }

        public string DefaultSearchQuery
        {
            get { return (string)GetValue(DefaultSearchQueryProperty); }
            set { SetValue(DefaultSearchQueryProperty, (string)value); }
        }

        public string ImageContentType
        {
            get { return (string)GetValue(ImageContentTypeProperty); }
            set { SetValue(ImageContentTypeProperty, (string)value); }
        }

        public ImageSearchUserControl()
        {
            this.InitializeComponent();
        }

        private async void onQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            await QueryBingImages(args.QueryText);
        }

        private async Task QueryBingImages(string query)
        {
            this.progressRing.IsActive = true;

            try
            {
                if (BingSearchHelper.BingIsConfigured)
                {
                    IEnumerable<string> imageUrls =
                        await
                        BingSearchHelper.GetImageSearchResults(query, imageContent: this.ImageContentType, count: 30);

                    this.imageResultsGrid.ItemsSource = imageUrls.Select(url => new ImageWithFace(url));
                }
            }
            catch (Exception ex)
            {
                this.imageResultsGrid.ItemsSource = null; 
                await Util.GenericApiCallExceptionHandler(ex, "Failure querying Bing Images");
            }

            this.progressRing.IsActive = false;
        }

        private void ClearFlyoutState()
        {
            if (this.ClearStateWhenClosed)
            {
                this.autoSuggestBox.Text = this.DefaultSearchQuery;
                this.imageResultsGrid.ItemsSource = Enumerable.Empty<string>();
            }
            else
            {
                this.imageResultsGrid.DeselectRange(new ItemIndexRange(0, (uint)this.imageResultsGrid.Items.Count));
            }
        }

        private async void onTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                try
                {
                    if (BingSearchHelper.BingIsConfigured)
                    {
                        this.autoSuggestBox.ItemsSource = await BingSearchHelper.GetAutoSuggestResults(this.autoSuggestBox.Text);
                    }
                }
                catch (Exception)
                {
                    // Default to no suggestions
                    this.autoSuggestBox.ItemsSource = null;
                }

            }
            else if (args.Reason == AutoSuggestionBoxTextChangeReason.ProgrammaticChange && !string.IsNullOrEmpty(this.autoSuggestBox.Text))
            {
                await this.QueryBingImages(this.autoSuggestBox.Text);
            }
        }

        private void OnAcceptButtonClicked(object sender, RoutedEventArgs e)
        {
            if (this.OnImageSearchCompleted != null)
            {
                this.OnImageSearchCompleted(this, this.imageResultsGrid.SelectedItems.Cast<ImageWithFace>().ToArray());
            }

            this.ClearFlyoutState();
        }

        private void OnCancelButtonClicked(object sender, RoutedEventArgs e)
        {
            if (this.OnImageSearchCanceled != null)
            {
                this.OnImageSearchCanceled(this, EventArgs.Empty);
            }

            this.ClearFlyoutState();
        }

        private void OnImageResultSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.addSelectedPhotosButton.IsEnabled = this.imageResultsGrid.SelectedItems.Any();
        }

        private async void LoadImagesFromFileClicked(object sender, RoutedEventArgs e)
        {
            this.progressRing.IsActive = true;

            try
            {
                FileOpenPicker fileOpenPicker = new FileOpenPicker { SuggestedStartLocation = PickerLocationId.PicturesLibrary, ViewMode = PickerViewMode.Thumbnail };
                fileOpenPicker.FileTypeFilter.Add(".jpg");
                fileOpenPicker.FileTypeFilter.Add(".jpeg");
                fileOpenPicker.FileTypeFilter.Add(".png");
                fileOpenPicker.FileTypeFilter.Add(".bmp");
                IReadOnlyList<StorageFile> selectedFiles = await fileOpenPicker.PickMultipleFilesAsync();

                if (selectedFiles != null)
                {
                    this.imageResultsGrid.ItemsSource = selectedFiles.Select(file => new ImageWithFace(file));
                }

                if (this.OnImageSearchLocalFilesProvided != null)
                {
                    this.OnImageSearchLocalFilesProvided(this, EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {
                this.imageResultsGrid.ItemsSource = null;
                await Util.GenericApiCallExceptionHandler(ex, "Failure processing local images");
            }

            this.progressRing.IsActive = false;
        }
    }

    public class IdentifiedPerson
    {
        public double Confidence
        {
            get; set;
        }

        public Person Person
        {
            get; set;
        }

        public Guid FaceId
        {
            get; set;
        }
    }
}
