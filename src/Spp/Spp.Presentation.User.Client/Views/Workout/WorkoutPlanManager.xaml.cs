/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace Spp.Presentation.User.Client
{
    public sealed partial class WorkoutPlanManager : Page
    {
        SplitView rootPage = Shell.Current;

        public class Item
        {
            public int Index { get; set; }
            public int ColSpan { get; set; }
            public int RowSpan { get; set; }
        }

        public List<Item> DataList = new List<Item>();

        public WorkoutPlanManager()
        {
            this.InitializeComponent();
            rootPage.CompactPaneLength = 50;
        }

        public IEnumerable<Item> GetItems()
        {
            var list = new List<Item>();

            for (int i = 0; i <= 10; i++)
            {
                if (i == 0)
                {
                    list.Add(new Item
                    {
                        Index = i,
                        ColSpan = 2,
                        RowSpan = 1,
                    });
                }
                else
                {
                    list.Add(new Item
                    {
                        Index = i,
                        ColSpan = 1,
                        RowSpan = 1,
                    });
                }
            }

            return list;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var DataList = this.GetItems();

            NoBorderGrid.ItemsSource = DataList;

            base.OnNavigatedTo(e);
        }

        private void AddNewWorkoutPlan(object sender, TappedRoutedEventArgs e)
        {
            //(this.rootPage.Content as Frame).Navigate(typeof(FunctionalityNotImplemented));
            //Modal.OpenModal();
        }
    }
}
