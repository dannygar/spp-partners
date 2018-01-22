/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Spp.Presentation.User.Client.UserControls
{
    public sealed partial class WellnessTileUserControl : UserControl
    {
        public WellnessTileUserControl()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        public string WellnessScore
        {
            get { return (string)GetValue(WellnessScoreProperty); }
            set { SetValue(WellnessScoreProperty, value); }
        }

        // Using a DependencyProperty as the backing store for WellnessScore.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WellnessScoreProperty =
            DependencyProperty.Register("WellnessScore", typeof(string), typeof(WellnessTileUserControl), null);


    }
}
