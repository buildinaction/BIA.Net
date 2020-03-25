// <copyright file="CustomRollingFileAppender.cs" company="BIA.NET">
// Copyright (c) BIA.NET. All rights reserved.
// </copyright>

namespace BIA.Net.Common
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;

    /// <summary>
    /// Custom log4net.Appender.RollingFileAppender
    /// </summary>
    public class CustomRollingFileAppender : log4net.Appender.RollingFileAppender
    {
        /// <summary>
        /// Gets or sets date of the last log file cleanup.
        /// </summary>
        protected DateTime LastDeleteOldFile { get; set; }

        // /// <inheritdoc/>
        // protected override void AdjustFileBeforeAppend()
        // {
        //     base.AdjustFileBeforeAppend();
        //     this.DeleteOldFile();
        // }

        /// <inheritdoc/>
        protected override void OpenFile(string fileName, bool append)
        {
            base.OpenFile(fileName, append);
            this.DeleteOldFile();
        }

        /// <summary>
        /// Delete files that were changed more than N days ago. (N = this.MaxSizeRollBackups)
        /// </summary>
        protected virtual void DeleteOldFile()
        {
            if (this.MaxSizeRollBackups > 0 && (this.RollingStyle == RollingMode.Composite || this.RollingStyle == RollingMode.Date) && (DateTime.Today - this.LastDeleteOldFile).TotalDays > 1d)
            {
                string directoryName = Path.GetDirectoryName(this.File);

                try
                {
                    string[] files = Directory.GetFiles(directoryName);

                    if (files != null && files.Length > 0)
                    {
                        foreach (string file in files)
                        {
                            if ((DateTime.Today - System.IO.File.GetLastWriteTime(file)).TotalDays > this.MaxSizeRollBackups)
                            {
                                this.DeleteFile(file);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    using (EventLog eventLog = new EventLog("Application"))
                    {
                        eventLog.Source = string.Format("{0} {1} {2}", "Log4Net", typeof(CustomRollingFileAppender).Name, MethodBase.GetCurrentMethod().Name);
                        eventLog.WriteEntry(string.Format("Directory: {0}\n\n{1}", directoryName, ex.Message), EventLogEntryType.Warning);
                    }
                }
                finally
                {
                    // This treatment is only done once a day. This parameter allows to check it.
                    this.LastDeleteOldFile = DateTime.Now;
                }
            }
        }
    }
}