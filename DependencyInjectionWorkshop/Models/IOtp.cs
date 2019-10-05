namespace DependencyInjectionWorkshop.Models
{
    public interface IOtp
    {
        string GetOtp(string accountId);
    }
}