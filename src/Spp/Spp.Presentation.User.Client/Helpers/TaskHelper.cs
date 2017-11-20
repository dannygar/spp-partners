/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿namespace MicrosoftSportsScience.Helpers
{
    using System;
    using System.Threading.Tasks;
    using Windows.ApplicationModel.Core;
    using Windows.UI.Core;

    public class TaskHelper
    {
        public void RunPromiseWithUiContinuation(Task promise, DispatchedHandler continuation)
        {
            var task = new Task(promise.Wait); // Wrap promise (awaitable task) into normal task.
            var continuationAction = new Action<Task>(t => this.ExecuteWithUiDispatcher(continuation));
            task.ContinueWith(continuationAction);
            task.Start();
        }

        private async void ExecuteWithUiDispatcher(DispatchedHandler continuation)
        {
            var appUiDispatcher = CoreApplication.MainView.CoreWindow.Dispatcher;
            await appUiDispatcher.RunAsync(CoreDispatcherPriority.Normal, continuation);
        }
    }
}
