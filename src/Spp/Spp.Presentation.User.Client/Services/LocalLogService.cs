/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Foundation.Diagnostics;
using Windows.Storage;

namespace Spp.Presentation.User.Client.Services
{
    public class LocalLogService : ILogService
    {
        private enum LoggingType
        {
            Info,
            Warning,
            Error
        }

        private static LoggingSession loggingSession;
        private static LoggingChannel loggingChannel;

        public void Info(string message, object sender)
        {
            this.EnsureLogIsOpen();

            var logMessage = this.FormatLogMessage(message, sender, LoggingType.Info.ToString());
            loggingChannel.LogMessage(logMessage, LoggingLevel.Information);
            Debug.WriteLine(logMessage);
        }

        public void Warning(string message, object sender)
        {
            this.EnsureLogIsOpen();

            var logMessage = this.FormatLogMessage(message, sender, LoggingType.Warning.ToString());
            loggingChannel.LogMessage(logMessage, LoggingLevel.Warning);
            Debug.WriteLine(logMessage);
        }

        public void Error(string message, object sender)
        {
            this.EnsureLogIsOpen();

            var logMessage = this.FormatLogMessage(message, sender, LoggingType.Error.ToString());
            loggingChannel.LogMessage(logMessage, LoggingLevel.Error);
            Debug.WriteLine(logMessage);
        }

        public void Error(Exception e, object sender)
        {
            this.EnsureLogIsOpen();

            var errorMessage = string.Format("Message: {0} \n StackTrace: {1} \n InnerException: {2}", e.Message, e.StackTrace, e.InnerException);

            var logMessage = this.FormatLogMessage(errorMessage, sender, LoggingType.Error.ToString());
            loggingChannel.LogMessage(logMessage, LoggingLevel.Error);
            Debug.WriteLine(logMessage);
        }

        public void FlushLogs()
        {
            if (loggingSession == null || loggingChannel == null)
                return;

            loggingChannel.Dispose();
            loggingSession.Dispose();

            loggingSession = null;
            loggingChannel = null;
        }

        public async Task<StorageFile> SaveLogs()
        {
            StorageFolder appDefinedLogFolder =
                       await ApplicationData.Current.LocalFolder.CreateFolderAsync("Logs", CreationCollisionOption.OpenIfExists);

            string newLogFileName = "Log-" + this.GetTimeStamp() + ".etl";
            return await loggingSession.SaveToFileAsync(appDefinedLogFolder, newLogFileName);
        }

        private string FormatLogMessage(string message, object sender, string logType)
        {
            return string.Format("[{0}] {1} [{2}] {3}", logType, DateTime.Now, sender.GetType(), message);
        }

        private void EnsureLogIsOpen()
        {
            if (loggingSession == null)
            {
                loggingSession = new LoggingSession("session");
#pragma warning disable CS0618 // Type or member is obsolete
                loggingChannel = new LoggingChannel("channel");
#pragma warning restore CS0618 // Type or member is obsolete
                loggingSession.AddLoggingChannel(loggingChannel);
            }
        }

        private string GetTimeStamp()
        {
            DateTime now = DateTime.Now;
            return string.Format(
                System.Globalization.CultureInfo.InvariantCulture,
                "{0:D2}{1:D2}{2:D2}-{3:D2}{4:D2}{5:D2}{6:D3}",
                now.Year - 2000,
                now.Month,
                now.Day,
                now.Hour,
                now.Minute,
                now.Second,
                now.Millisecond);
        }
    }
}
