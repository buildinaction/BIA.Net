// <copyright file="TraceManager.cs" company="BIA.NET">
// Copyright (c) BIA.NET. All rights reserved.
// </copyright>

namespace BIA.Net.Common
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Reflection;
    using BIA.Net.Common.Helpers;
    using log4net;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Class to for trace management.
    /// </summary>
    public static class TraceManager
    {
        /// <summary>
        /// Store the log4net logger.
        /// </summary>
        private static readonly ILog Log = LogManager.GetLogger("File");

        /// <summary>
        /// Last time measure for this thread.
        /// </summary>
        [ThreadStatic]
        private static long? lastElapsedTime = null;

        /// <summary>
        /// Last counter id used for this thread
        /// </summary>
        [ThreadStatic]
        private static int lastTimerId;

        /// <summary>
        /// Enum to list available action on time measure
        /// </summary>
        public enum TimeMeasureAction
        {
            /// <summary>
            /// Do nothing about time measure
            /// </summary>
            DoNothing = 0,

            /// <summary>
            /// Start time measure
            /// </summary>
            Start = 1,

            /// <summary>
            /// Stop time measure
            /// </summary>
            Stop = 2
        }

        /// <summary>
        /// Automatically configures the log system based on the application's configuration settings.
        /// </summary>
        /// <param name="pEventLog">Event logger</param>
        public static void Configure()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        /// <summary>
        /// Log a message object with the Debug level.
        /// </summary>
        /// <param name="className">Name of the current class to use for log.</param>
        /// <param name="methodName">Name of the current method to use for log.</param>
        /// <param name="message">Message to use in log entry.</param>
        public static void Debug(string className, string methodName, string message)
        {
            Debug(className, methodName, message, TimeMeasureAction.DoNothing);
        }

        /// <summary>
        /// Log a message object with the Debug level (automaticaly set the class and methode.)
        /// </summary>
        /// <param name="message">message to log</param>
        /// <param name="linenumber">linenumber Automaticaly field</param>
        /// <param name="filepath">filepath Automaticaly field</param>
        /// <param name="memberName">memberName Automaticaly field</param>
        public static void Debug(string message, [CallerLineNumber] int linenumber = 0, [CallerFilePathAttribute] string filepath = "", [CallerMemberName]string memberName = "")
        {
            string className, methodName;
            PrepareClassAndMethodeName(linenumber, filepath, memberName, out className, out methodName);

            Debug(className, methodName, message, TimeMeasureAction.DoNothing);
        }

        private static void PrepareClassAndMethodeName(int linenumber, string filepath, string memberName, out string className, out string methodName)
        {
            className = string.Format("{0}.{1}", filepath.Substring(filepath.LastIndexOf('\\') + 1).Split('.')[0], linenumber);
            methodName = string.Format("{0}", memberName);
        }

        /// <summary>
        /// Log a message object with the Debug level. Before this, a time measure action will be done according to providing value.
        /// </summary>
        /// <param name="className">Name of the current class to use for log.</param>
        /// <param name="methodName">Name of the current method to use for log.</param>
        /// <param name="message">Message to use in log entry.</param>
        /// <param name="timeMeasureAction">Time measure action to proceed.</param>
        public static void Debug(string className, string methodName, string message, TimeMeasureAction timeMeasureAction)
        {
            if (Log.IsDebugEnabled)
            {
                switch (timeMeasureAction)
                {
                    case TimeMeasureAction.Start:
                        StartTimeMeasure();
                        break;
                    case TimeMeasureAction.Stop:
                        StopTimeMeasure();
                        break;
                    default:
                        break;
                }
            }

            Log.Debug(FormatMessage(className, methodName, message));
        }

        /// <summary>
        /// Log a message object with the Debug level including the stack trace of the System.Exception passed as a parameter.
        /// </summary>
        /// <param name="className">Name of the current class to use for log.</param>
        /// <param name="methodName">Name of the current method to use for log.</param>
        /// <param name="message">Message to use in log entry.</param>
        /// <param name="exception">Exception to use in log entry.</param>
        public static void Debug(string className, string methodName, string message, Exception exception)
        {
            Log.Debug(FormatMessage(className, methodName, message), exception);
        }

        /// <summary>
        /// Log a message object with the Info level.
        /// </summary>
        /// <param name="className">Name of the current class to use for log.</param>
        /// <param name="methodName">Name of the current method to use for log.</param>
        /// <param name="message">Message to use in log entry.</param>
        public static void Info(string className, string methodName, string message)
        {
            Log.Info(FormatMessage(className, methodName, message));
        }

        /// <summary>
        /// Log a message object with the Info level(automaticaly set the class and methode.)
        /// </summary>
        /// <param name="message">message to log</param>
        /// <param name="linenumber">linenumber Automaticaly field</param>
        /// <param name="filepath">filepath Automaticaly field</param>
        /// <param name="memberName">memberName Automaticaly field</param>
        public static void Info(string message, [CallerLineNumber] int linenumber = 0, [CallerFilePathAttribute] string filepath = "", [CallerMemberName]string memberName = "")
        {
            string className, methodName;
            PrepareClassAndMethodeName(linenumber, filepath, memberName, out className, out methodName);

            Log.Info(FormatMessage(className, methodName, message));
        }

        /// <summary>
        /// Log a message object with the Info level including the stack trace of the System.Exception passed as a parameter.
        /// </summary>
        /// <param name="className">Name of the current class to use for log.</param>
        /// <param name="methodName">Name of the current method to use for log.</param>
        /// <param name="message">Message to use in log entry.</param>
        /// <param name="exception">Exception to use in log entry.</param>
        public static void Info(string className, string methodName, string message, Exception exception)
        {
            Log.Info(FormatMessage(className, methodName, message));
        }

        /// <summary>
        /// Log a message object with the Warn level.
        /// </summary>
        /// <param name="className">Name of the current class to use for log.</param>
        /// <param name="methodName">Name of the current method to use for log.</param>
        /// <param name="message">Message to use in log entry.</param>
        public static void Warn(string className, string methodName, string message)
        {
            Log.Warn(FormatMessage(className, methodName, message));
        }

        /// <summary>
        /// Log a message object with the Warn level including the stack trace of the System.Exception passed as a parameter.
        /// </summary>
        /// <param name="className">Name of the current class to use for log.</param>
        /// <param name="methodName">Name of the current method to use for log.</param>
        /// <param name="message">Message to use in log entry.</param>
        /// <param name="exception">Exception to use in log entry.</param>
        public static void Warn(string className, string methodName, string message, Exception exception)
        {
            Log.Warn(FormatMessage(className, methodName, message), exception);
        }

        /// <summary>
        /// Log a message object with the Warn level (automaticaly set the class and methode.)
        /// </summary>
        /// <param name="message">message to log</param>
        /// <param name="linenumber">linenumber Automaticaly field</param>
        /// <param name="filepath">filepath Automaticaly field</param>
        /// <param name="memberName">memberName Automaticaly field</param>
        public static void Warn(string message, [CallerLineNumber] int linenumber = 0, [CallerFilePathAttribute] string filepath = "", [CallerMemberName]string memberName = "")
        {
            string className, methodName;
            PrepareClassAndMethodeName(linenumber, filepath, memberName, out className, out methodName);

            Log.Warn(FormatMessage(className, methodName, message));
        }

        /// <summary>
        /// Log a message object with the Warn level including the stack trace of the System.Exception passed as a parameter(automaticaly set the class and methode.)
        /// </summary>
        /// <param name="message">message to log</param>
        /// <param name="exception">Exception to use in log entry.</param>
        /// <param name="linenumber">linenumber Automaticaly field</param>
        /// <param name="filepath">filepath Automaticaly field</param>
        /// <param name="memberName">memberName Automaticaly field</param>
        public static void Warn(string message, Exception exception, [CallerLineNumber] int linenumber = 0, [CallerFilePathAttribute] string filepath = "", [CallerMemberName]string memberName = "")
        {
            string className, methodName;
            PrepareClassAndMethodeName(linenumber, filepath, memberName, out className, out methodName);

            Log.Warn(FormatMessage(className, methodName, message), exception);
        }

        /// <summary>
        /// Log a message object with the Error level.
        /// </summary>
        /// <param name="className">Name of the current class to use for log.</param>
        /// <param name="methodName">Name of the current method to use for log.</param>
        /// <param name="message">Message to use in log entry.</param>
        public static void Error(string className, string methodName, string message)
        {
            Log.Error(FormatMessage(className, methodName, message));
        }

        /// <summary>
        /// Log a message object with the Error level including the stack trace of the System.Exception passed as a parameter.
        /// </summary>
        /// <param name="className">Name of the current class to use for log.</param>
        /// <param name="methodName">Name of the current method to use for log.</param>
        /// <param name="message">Message to use in log entry.</param>
        /// <param name="exception">Exception to use in log entry.</param>
        public static void Error(string className, string methodName, string message, Exception exception)
        {
            Log.Error(FormatMessage(className, methodName, message), exception);
        }

        /// <summary>
        /// Log a message object with the Error level (automaticaly set the class and methode.)
        /// </summary>
        /// <param name="message">message to log</param>
        /// <param name="linenumber">linenumber Automaticaly field</param>
        /// <param name="filepath">filepath Automaticaly field</param>
        /// <param name="memberName">memberName Automaticaly field</param>
        public static void Error(string message, [CallerLineNumber] int linenumber = 0, [CallerFilePathAttribute] string filepath = "", [CallerMemberName]string memberName = "")
        {
            string className, methodName;
            PrepareClassAndMethodeName(linenumber, filepath, memberName, out className, out methodName);

            Log.Warn(FormatMessage(className, methodName, message));
        }

        /// <summary>
        /// Log a message object with the Error level including the stack trace of the System.Exception passed as a parameter(automaticaly set the class and methode.)
        /// </summary>
        /// <param name="message">message to log</param>
        /// <param name="exception">Exception to use in log entry.</param>
        /// <param name="linenumber">linenumber Automaticaly field</param>
        /// <param name="filepath">filepath Automaticaly field</param>
        /// <param name="memberName">memberName Automaticaly field</param>
        public static void Error(string message, Exception exception, [CallerLineNumber] int linenumber = 0, [CallerFilePathAttribute] string filepath = "", [CallerMemberName]string memberName = "")
        {
            string className, methodName;
            PrepareClassAndMethodeName(linenumber, filepath, memberName, out className, out methodName);

            Log.Warn(FormatMessage(className, methodName, message), exception);
        }

        /// <summary>
        /// Starts the stopwatch
        /// </summary>
        public static void StartTimeMeasure()
        {
            ResetLastElapsedTime();
            lastTimerId = StopWatchHelper.Start();
        }

        /// <summary>
        /// Stops the the stopwatch
        /// </summary>
        public static void StopTimeMeasure()
        {
            try
            {
                lastElapsedTime = StopWatchHelper.Stop(lastTimerId);
            }
            catch (Exception counterManagementException)
            {
                MethodBase methodBase = MethodBase.GetCurrentMethod();
                Warn(methodBase.DeclaringType.Name, methodBase.Name, "Unable to stop time measure", counterManagementException);

                // Reset the last elapsed time value to avoid to use it in nexxt log attempt.
                ResetLastElapsedTime();
            }
        }

        /// <summary>
        /// Concatenates the string parameters.
        /// </summary>
        /// <param name="className">The class name to use.</param>
        /// <param name="methodName">The method name to use.</param>
        /// <param name="message">The message to use.</param>
        /// <returns>The concatenation of the provided values.</returns>
        private static string FormatMessage(string className, string methodName, string message)
        {
            // Add time to end of line if needed
            if (lastElapsedTime.HasValue)
            {
                string output = string.Format("{0} : {1} : {2} : {3}", className, methodName, message, lastElapsedTime.Value.ToString("#,##0' ms'", CultureInfo.InvariantCulture));
                ResetLastElapsedTime();
                return output;
            }
            else
            {
                return string.Format("{0} : {1} : {2}", className, methodName, message);
            }
        }

        /// <summary>
        /// Reset the last elapsed time.
        /// </summary>
        private static void ResetLastElapsedTime()
        {
            lastElapsedTime = null;
        }
    }
}
