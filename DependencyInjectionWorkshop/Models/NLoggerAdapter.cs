﻿namespace DependencyInjectionWorkshop.Models
{
    public class NLoggerAdapter : ILogger
    {
        public NLoggerAdapter()
        {
        }

        public void LogInfo(string message)
        {
            var logger = NLog.LogManager.GetCurrentClassLogger();
            logger.Info(message);
        }
    }
}