/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using MicrosoftSportsScience.Data;
using MicrosoftSportsScience.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace MicrosoftSportsScience.UserControls
{
    public sealed partial class MessageTileUserControl : UserControl
    {
        public MessageTileUserControl()
        {
            this.InitializeComponent();
        }

        public ObservableCollection<MessageViewModel> MessageList
        {
            get { return (ObservableCollection<MessageViewModel>)GetValue(MessageListProperty); }
            set { SetValue(MessageListProperty, value); }
        }
        
        public static readonly DependencyProperty MessageListProperty =
            DependencyProperty.Register("MessageList", typeof(List<MessageViewModel>), typeof(MessageTileUserControl), null);
        
    }
}
