using System;
using System.Diagnostics;

namespace GTASessionBot.Utilities
{
    /// <summary>
    /// Defines a class responsible for writing logs to the Windows Event Log.
    /// </summary>
    public class EventLogLogger
    {

        private static readonly string SourceName = "Session Management Bot";
        private static readonly string LogName = "Gaming";


        /// <summary>
        /// Ensures that the event source exists, and creates it if it does not exist.
        /// </summary>
        public static void EnsureExists()
        {
            if (!EventLog.SourceExists(SourceName))
            {
                EventLog.CreateEventSource(SourceName, LogName);
            }
        }


        /// <summary>
        /// Logs the given information message to the Windows Event Log.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public static void LogInformation(string message)
        {

            using (var log = new EventLog(LogName))
            {
                log.Source = SourceName;
                log.WriteEntry(message, EventLogEntryType.Information);
            }
        }


        /// <summary>
        /// Logs the given warning to the Windows Event Log.
        /// </summary>
        /// <param name="warning">The warning to log.</param>
        public static void LogWarning(string warning)
        {
            using (var log = new EventLog(LogName))
            {
                log.Source = SourceName;
                log.WriteEntry(warning, EventLogEntryType.Warning);
            }
        }


        /// <summary>
        /// Logs the given error to the Windows Event Log.
        /// </summary>
        /// <param name="error">The error to log.</param>
        public static void LogError(string error)
        {
            using (var log = new EventLog(LogName))
            {
                log.Source = SourceName;
                log.WriteEntry(error, EventLogEntryType.Error);
            }
        }


        /// <summary>
        /// Logs an error with the given exception to the Windows Event Log.
        /// </summary>
        /// <param name="e">The exception to get details from.</param>
        public static void LogError(Exception e)
        {
            using (var log = new EventLog(LogName))
            {
                log.Source = SourceName;
                log.WriteEntry($"An unexpected error occurred. {e.Message}", EventLogEntryType.Error);
            }
        }


        public static void LogError(Exception e, string commandMessage)
        {
            using (var log = new EventLog(LogName))
            {
                log.Source = SourceName;
                log.WriteEntry(
                    $"An unexpected error occurred when processing the command: '{commandMessage}'. {e.Message}",
                    EventLogEntryType.Error
                );
            }
        }

    }
}
