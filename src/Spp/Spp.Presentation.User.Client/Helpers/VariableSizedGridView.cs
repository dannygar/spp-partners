/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MicrosoftSportsScience.Helpers
{
    public class VariableSizedGridView : GridView
    {
        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            try
            {
                dynamic localItem = item;
                element.SetValue(VariableSizedWrapGrid.RowSpanProperty, localItem.RowSpan);
                element.SetValue(VariableSizedWrapGrid.ColumnSpanProperty, 1);
                //element.SetValue(VariableSizedWrapGrid.ColumnSpanProperty, localItem.ColSpan);
            }
            catch
            {
                element.SetValue(VariableSizedWrapGrid.RowSpanProperty, 1);
                element.SetValue(VariableSizedWrapGrid.ColumnSpanProperty, 1);
            }

            base.PrepareContainerForItemOverride(element, item);
        }
    }
}
