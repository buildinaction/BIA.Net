using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace BIA.Net.Common.Helpers
{
    /// <summary>
    /// Wrapper for .NET Stopwatch class to provide an easy way to manipulate timer and counter for performance analysis purpose.
    /// </summary>
    public static class StopWatchHelper
    {
        #region Fields

        /// <summary>
        /// Store the list of current counters associated with their hashcode.
        /// </summary>
        private static readonly Dictionary<int, Stopwatch> Counters = new Dictionary<int, Stopwatch>();

        /// <summary>
        /// Store a stack of available counters.
        /// </summary>
        private static readonly Stack<Stopwatch> OrphanedCounters = new Stack<Stopwatch>();

        /// <summary>
        /// Store the datetime value of the last cleaning.
        /// </summary>
        private static DateTime? lastCleanTime = null;

        #endregion Fields

        #region Methods

        /// <summary>
        /// Start a counter using a new one or a free existing one.
        /// </summary>
        /// <returns>Identifier to retrieve the counter.</returns>
        public static int Start()
        {
            // First execute the cleaning process
            CleanCounters();

            Stopwatch stopwatch;

            lock (OrphanedCounters)
            {
                if (OrphanedCounters.Count > 0)
                {
                    // There is some available counters, use an existing one
                    stopwatch = OrphanedCounters.Pop();
                }
                else
                {
                    // Create a new counter and add it in the store.
                    stopwatch = new Stopwatch();
                    Counters.Add(stopwatch.GetHashCode(), stopwatch);
                }

                stopwatch.Start();

                // Use hash code to identify counter
                return stopwatch.GetHashCode();
            }
        }

        /// <summary>
        /// Resumes an existing counter. If the counter is already counting, nothing will happened.
        /// </summary>
        /// <exception cref="CounterNotFoundException">Thrown if no existing counter with the provided identifier.</exception>
        /// <exception cref="NullCounterException">Thrown if counter linked to identifier is null.</exception>
        /// <param name="counterId">Identifier of the counter to resume.</param>
        public static void Start(int counterId)
        {
            lock (OrphanedCounters)
            {
                Stopwatch stopwatch;
                if (!Counters.TryGetValue(counterId, out stopwatch))
                {
                    throw new CounterNotFoundException("Unable to find counter with identifier " + counterId);
                }

                if (stopwatch == null)
                {
                    throw new NullCounterException("Provided idenditifier seems to be ok but linked counter is null.");
                }

                stopwatch.Start();
            }
        }

        /// <summary>
        /// Stops the counter and returns the elapsed time in millisecond since started.
        /// </summary>
        /// <param name="counterId">Counter Id</param>
        /// <exception cref="CounterNotFoundException">Thrown if no existing counter with the provided identifier.</exception>
        /// <exception cref="NullCounterException">Thrown if counter linked to identifier is null.</exception>
        /// <returns>Elapsed time in milliseconds</returns>
        public static long Stop(int counterId)
        {
            lock (OrphanedCounters)
            {
                Stopwatch stopwatch;
                long elapsedTime = -1;
                if (!Counters.TryGetValue(counterId, out stopwatch))
                {
                    throw new CounterNotFoundException("Unable to find counter with identifier " + counterId);
                }

                if (stopwatch == null)
                {
                    throw new NullCounterException("Provided idenditifier seems to be ok but linked counter is null.");
                }

                stopwatch.Stop();
                elapsedTime = stopwatch.ElapsedMilliseconds;
                stopwatch.Reset();
                OrphanedCounters.Push(stopwatch);

                return elapsedTime;
            }
        }

        /// <summary>
        /// Cleaning process of existing counters. Once per hour, clean any counter started for more than one hour,
        /// that must be an error from the developper, forgetting calling the Stop method on it.
        /// </summary>
        private static void CleanCounters()
        {
            if (!lastCleanTime.HasValue || lastCleanTime.Value.AddHours(1).CompareTo(DateTime.Now) > 0)
            {
                lock (OrphanedCounters)
                {
                    foreach (Stopwatch item in Counters.Values)
                    {
                        if (item.ElapsedMilliseconds > 3600 * 1000)
                        {
                            // Reset the counter and put it in the list of available counters
                            item.Reset();
                            OrphanedCounters.Push(item);
                        }
                    }
                }

                lastCleanTime = DateTime.Now;
            }
        }

        #endregion Methods
    }

    #region Specific exceptions

    /// <summary>
    /// Specific class if expected counter is not found.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single class", Justification = "Keep specific exception next to sender.")]
    public class CounterNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CounterNotFoundException"/> class with a specified error message..
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public CounterNotFoundException(string message)
            : base(message)
        {
        }
    }

    /// <summary>
    /// Specific class if expected counter is null.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single class", Justification = "Keep specific exception next to sender.")]
    public class NullCounterException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NullCounterException"/> class with a specified error message..
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public NullCounterException(string message)
            : base(message)
        {
        }
    }

    #endregion Specific exceptions
}
