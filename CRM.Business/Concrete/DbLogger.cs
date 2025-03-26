using CRM.Business.Abstract;
using CRM.Core.Entity.Concrete;
using CRM.Core.Extensions;
using CRM.DataAccess.Conctrete.EntityFrameworkCore.Context;
using Microsoft.Extensions.Logging;

namespace CRM.Business.Concrete {
    public class DbLogger : LogBusiness, IDbLogger {

        public DbLogger(CrmDbContext context) : base(context) { }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull {
            return default!;
        }

        public bool IsEnabled(LogLevel logLevel) {
            return logLevel != LogLevel.None;
        }

        public void LogToDbByMessage(string? message, LogLevel logLevel, Exception? exception = null) {

            var log = new LogBase { Type = logLevel, Message = message! };

            if(exception != null) {
                message += $"-Exception : {exception.InnerException?.Message ?? exception.Message}";
            }

            switch(log.Type) {
                case LogLevel.Trace:
                    this.LogTrace(message);
                    break;
                case LogLevel.Debug:
                    this.LogDebug(message);
                    break;
                case LogLevel.Information:
                    this.LogInformation(message);
                    break;
                case LogLevel.Warning:
                    this.LogWarning(message);
                    break;
                case LogLevel.Error:
                    this.LogError(message);
                    break;
                case LogLevel.Critical:
                    this.LogCritical(message);
                    break;
                case LogLevel.None:
                    break;
            }

            if(log.Message.IsNotNullOrEmptyOrWhiteSpace()) {
                this.Add(log);
                this.Save();
            }
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter) {

        }
    }
}
