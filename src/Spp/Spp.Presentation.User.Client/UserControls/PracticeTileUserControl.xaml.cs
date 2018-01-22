/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using Spp.Presentation.User.Client.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Spp.Presentation.User.Client.UserControls
{
    public sealed partial class PracticeTileUserControl : UserControl
    {
        private bool flipped = false;
        SplitView rootPage = Shell.Current;

        public PracticeTileUserControl()
        {
            this.InitializeComponent();
        }

        public string CategoryColor
        {
            get { return (string)GetValue(CategoryColorProperty); }
            set { SetValue(CategoryColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CategoryColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CategoryColorProperty =
            DependencyProperty.Register("CategoryColor", typeof(string), typeof(PracticeTileUserControl), new PropertyMetadata(null));

        public string Category
        {
            get { return (string)GetValue(CategoryProperty); }
            set { SetValue(CategoryProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Category.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CategoryProperty =
            DependencyProperty.Register("Category", typeof(string), typeof(PracticeTileUserControl), new PropertyMetadata(null));

        public string PracticeName
        {
            get { return (string)GetValue(PracticeNameProperty); }
            set { SetValue(PracticeNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for WorkouName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PracticeNameProperty =
            DependencyProperty.Register("PracticeName", typeof(string), typeof(PracticeTileUserControl), new PropertyMetadata(null));


        public string PracticeTopic
        {
            get { return (string)GetValue(PracticeTopicProperty); }
            set { SetValue(PracticeTopicProperty, value); }
        }

        // Using a DependencyProperty as the backing store for WorkouName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PracticeTopicProperty =
            DependencyProperty.Register("PracticeTopic", typeof(string), typeof(PracticeTileUserControl), new PropertyMetadata(null));


        private void grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (!flipped)
                VisualStateManager.GoToState(this, "Flip", true);
            else
                VisualStateManager.GoToState(this, "FlipBack", true);

            flipped = !flipped;
        }

        private void EditTapped(object sender, TappedRoutedEventArgs e)
        {
            var vm = ((Grid)sender).DataContext as AthletePracticeViewModel;
            (rootPage.Content as Frame).Navigate(typeof(SessionPlanner), vm);

            e.Handled = true;
        }

        private void ScheduleTapped(object sender, TappedRoutedEventArgs e)
        {
            (rootPage.Content as Frame).Navigate(typeof(PracticeSessionPlannerAddPlayerGroups), ((Grid)sender).DataContext);
            e.Handled = true;
        }
    }
}
