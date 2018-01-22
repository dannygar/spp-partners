/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using System.Collections.ObjectModel;
using Spp.Presentation.User.Client.ViewModels;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Spp.Presentation.User.Client.UserControls
{
    public sealed partial class CalendarUserControl : UserControl
    {
        double cellWidth;
        double cellHeight;
        Rectangle rect = null;
        Canvas workoutCanvas = null;
        TextBlock name;

        public CalendarUserControl()
        {
            this.InitializeComponent();
            CalendarSections = new ObservableCollection<CalendarSectionViewModel>();
            for (int i = 0; i < 147; i++)
            {
                CalendarSectionViewModel section = new CalendarSectionViewModel();
                CalendarSections.Add(section);
            }
        }

        public ObservableCollection<CalendarSectionViewModel> CalendarSections
        {
            get { return (ObservableCollection<CalendarSectionViewModel>)GetValue(CalendarSectionsProperty); }
            set { SetValue(CalendarSectionsProperty, value); }
        }

        public static readonly DependencyProperty CalendarSectionsProperty =
            DependencyProperty.Register("CalendarSections", typeof(ObservableCollection<CalendarSectionViewModel>), typeof(CalendarUserControl), null);

        private void Calendar_Loaded(object sender, RoutedEventArgs e)
        {
            cellWidth = WeekCanvas.ActualWidth / 7;
            cellHeight = WeekCanvas.ActualHeight / 20;
        }

        private void WeekCanvas_DragOver(object sender, DragEventArgs e)
        {
            Point p = e.GetPosition(WeekCanvas);
            double offsetXIncrement = 0;
            double offsetYIncrement = 0;

            for (int x = 0; x <= 7; x++)
            {
                if (p.X > (x + 1) * cellWidth)
                {
                    offsetXIncrement++;
                }
            }

            for (int y = 0; y <= 20; y++)
            {
                if (p.Y > (y + 1) * cellHeight)
                {
                    offsetYIncrement++;
                }
            }

            Canvas.SetLeft(workoutCanvas, offsetXIncrement * cellWidth);
            Canvas.SetTop(workoutCanvas, offsetYIncrement * cellHeight);
        }

        private void WeekCanvas_DropCompleted(UIElement sender, DropCompletedEventArgs args)
        {
            rect = null;
            workoutCanvas = null;
        }

        private void WeekCanvas_DragEnter(object sender, DragEventArgs e)
        {
            SolidColorBrush mySolidColorBrush = new SolidColorBrush();
            rect = new Rectangle();
            workoutCanvas = new Canvas();
            rect = new Rectangle();
            name = new TextBlock
            {
                Text = "Power\nMovements",
                Margin = new Thickness(5, 5, 5, 5)
            };
            rect.Width = cellWidth;
            rect.Height = cellHeight * 5;
            mySolidColorBrush.Color = Windows.UI.Color.FromArgb(255, 255, 0, 114);
            rect.Fill = mySolidColorBrush;
            workoutCanvas.Children.Add(rect);
            workoutCanvas.Children.Add(name);
            WeekCanvas.Children.Add(workoutCanvas);
        }
    }
}
