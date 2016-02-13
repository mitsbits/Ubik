using System;

namespace Ubik.Infra.Contracts
{
    public interface ILog
    {
        void Error(Exception exception);

        void Info(string format, params object[] args);

        void Warn(string format, params object[] args);
    }
}