using AppCore.Interfaces;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services
{
    public class AppLogger<T> : IAppLogger<T>
    {
        private ILogger<T> _logger;
        public AppLogger(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<T>();
        }
        public void LogInformation(string message, params object[] args)
        {
            _logger.LogInformation(message, args);
        }

        public void LogWarning(string message, params object[] args)
        {
            _logger.LogWarning(message, args);
        }
    }
}
