/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using Spp.Presentation.User.Client.Views;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Spp.Presentation.User.Client.UserControls
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
