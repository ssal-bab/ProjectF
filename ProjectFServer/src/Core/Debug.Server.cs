using System;
using Microsoft.Extensions.Logging;

namespace H00N
{
    public static partial class Debug
    {
        private static ILogger logger = null;

        public static void SetLogger(ILogger logger)
        {
            Debug.logger = logger;
        }

        public static partial void Log(string message)
        {
            logger.LogInformation(message);
        }

        public static partial void LogError(string message)
        {
            logger.LogError(message);
        }

        public static partial void LogWarning(string message)
        {
            logger.LogWarning(message);
        }

        public static partial void Log(object message)
        {
            logger.LogInformation(message.ToString());
        }

        public static partial void LogError(object message)
        {
            logger.LogError(message.ToString());
        }

        public static partial void LogWarning(object message)
        {
            logger.LogWarning(message.ToString());
        }
    }
}