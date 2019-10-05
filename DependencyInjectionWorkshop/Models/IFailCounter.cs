namespace DependencyInjectionWorkshop.Models
{
    public interface IFailCounter
    {
        void ResetFailedCount(string accountId);
        void AddFailedCount(string accountId);
        void LogFailedCount(string accountId);
        bool IsAccountLocked(string accountId);
        int GetFailedCount(string accountId);
    }
}