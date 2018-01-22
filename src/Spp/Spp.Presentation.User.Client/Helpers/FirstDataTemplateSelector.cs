/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Spp.Presentation.User.Client.Helpers
{
    public class FirstDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate DefaultTemplate { get; set; }
        public DataTemplate FirstItemTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            var lv = GetListView(container);
            if (lv != null)
            {
                var i = lv.Items.IndexOf(item);
                if (i == 0)
                {
                    return FirstItemTemplate;
                }
            }
            return DefaultTemplate;
        }


        public static GridView GetListView(DependencyObject element)
        {
            var parent = VisualTreeHelper.GetParent(element);
            if (parent == null)
            {
                return null;
            }
            var parentListView = parent as GridView;
            return parentListView ?? GetListView(parent);
        }
    }
}
