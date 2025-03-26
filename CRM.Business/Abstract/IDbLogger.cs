using CRM.Core.Entity.Concrete;
using Microsoft.Extensions.Logging;

namespace CRM.Business.Abstract {
    public interface IDbLogger : ILogBusiness, ILogger<LogBase> {
        void LogToDbByMessage(string message, LogLevel logLevel, Exception? exception = null);
    }
}