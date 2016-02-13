using System;

namespace Ubik.Infra.Contracts
{
    /// <summary>
    /// Interface for classes that support standard logging behaviors
    /// </summary>
    public interface ILoggingService
    {
        void LogMessage(string message);

        void LogMessage(string message, LogEntryType entryType);

        void LogException(Exception ex);

        void LogException(Exception ex, LogEntryType entryType);
    }
}