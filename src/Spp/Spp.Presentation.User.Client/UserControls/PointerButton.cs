/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Spp.Presentation.User.Client.UserControls
{
    public class PointerButton : Button
    {
        private static readonly CoreCursor HandCursor = new CoreCursor(CoreCursorType.Hand, 1);
        private static readonly CoreCursor ArrowCursor = new CoreCursor(CoreCursorType.Arrow, 1);

        public PointerButton() : base()
        {
            PointerEntered += (sender, e) =>
            {
                Window.Current.CoreWindow.PointerCursor = HandCursor;
            };
            PointerExited += (sender, e) =>
            {
                Window.Current.CoreWindow.PointerCursor = ArrowCursor;
            };
        }
    }
}
