namespace DependencyInjectionWorkshop.Models
{
    public interface ILogger
    {
        void LogInfo(string accountId, int failedCount);
    }
}