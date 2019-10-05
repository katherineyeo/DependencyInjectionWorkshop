namespace DependencyInjectionWorkshop.Models
{
    public class NLoggerAdapter : ILogger
    {
        public NLoggerAdapter()
        {
        }

        public void LogInfo(string accountId, int failedCount)
        {
            var logger = NLog.LogManager.GetCurrentClassLogger();
            logger.Info($"accountId:{accountId} failed times:{failedCount}");
        }
    }
}