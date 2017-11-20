/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using MicrosoftSportsScience.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace MicrosoftSportsScience.UserControls
{
    public sealed partial class SignInOptions : UserControl
    {
        readonly SplitView rootPage = Shell.Current;

        public SignInOptions()
        {
            this.InitializeComponent();
        }
        
        public SignInOptionsMethod CurrentMethod
        {
            get { return (SignInOptionsMethod)GetValue(CurrentMethodProperty); }
            set { SetValue(CurrentMethodProperty, value); OnMethodChanged(); }
        }

        // Using a DependencyProperty as the backing store for CurrentMethod.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentMethodProperty =
            DependencyProperty.Register("CurrentMethod", typeof(SignInOptionsMethod), typeof(SignInOptions), new PropertyMetadata(SignInOptionsMethod.Camera));

     
        private void OnMethodChanged()
        {
            if (CurrentMethod == SignInOptionsMethod.Camera)
            {
                cameraButton.BorderBrush = new SolidColorBrush(Colors.White);
                keyboardButton.BorderBrush = new SolidColorBrush(Colors.Transparent);
            }
            else
            {
                cameraButton.BorderBrush = new SolidColorBrush(Colors.Transparent);
                keyboardButton.BorderBrush = new SolidColorBrush(Colors.White);
            }
        }

        private void keyboardButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentMethod == SignInOptionsMethod.Camera)
            {
                (rootPage.Content as Frame).Navigate(typeof(SignIn));
            }
        }

        private void cameraButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentMethod == SignInOptionsMethod.Keyboard)
            {
                (rootPage.Content as Frame).Navigate(typeof(GreetingPage));
            }
        }
    }

    public enum SignInOptionsMethod { Camera, Keyboard };
}
