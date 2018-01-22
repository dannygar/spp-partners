/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using Spp.Presentation.User.Client.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Spp.Presentation.User.Client.UserControls
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
