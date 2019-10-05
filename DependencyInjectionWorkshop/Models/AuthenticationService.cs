namespace DependencyInjectionWorkshop.Models
{
    public interface IAuthentication
    {
        bool Verify(string accountId, string password, string otp);
    }

    public class AuthenticationService : IAuthentication
    {
        private readonly IProfileDao _ProfileDao;
        private readonly IHash _Hash;
        private readonly IOtp _Otp;
        private readonly INotification _Notification;
        private readonly IFailCounter _FailCounter;
        private readonly ILogger _Logger;

        public AuthenticationService(IProfileDao profileDao, IHash hash, IOtp otp, INotification notification, IFailCounter failCounter, ILogger logger)
        {
            _ProfileDao = profileDao;
            _Hash = hash;
            _Otp = otp;
            _Notification = notification;
            _FailCounter = failCounter;
            _Logger = logger;
        }

        public AuthenticationService()
        {
            _ProfileDao = new ProfileDao();
            _Hash = new Sha256Adapter();
            _Otp = new Otp();
            _Notification = new Notification();
            _FailCounter = new FailCounter();
            _Logger = new NLoggerAdapter();
        }

        public bool Verify(string accountId, string password, string otp)
        {
            if (_FailCounter.IsAccountLocked(accountId))
            {
                throw new FailedTooManyTimesException();
            }

            var passwordFromDb = _ProfileDao.GetPassword(accountId);

            var hashedPassword = _Hash.ComputeHash(password);

            var currentOtp = _Otp.GetOtp(accountId);

            if (passwordFromDb == hashedPassword && otp == currentOtp)
            {
                _FailCounter.ResetFailedCount(accountId);
                return true;
            }
            else
            {
                _FailCounter.AddFailedCount(accountId);

                LogFailedCount(accountId);

                _Notification.Notify($"{accountId} try to login, failed");

                return false;
            }
        }

        private void LogFailedCount(string accountId)
        {
            var failedCount = _FailCounter.GetFailedCount(accountId);
            _Logger.LogInfo($"accountId:{accountId} failed times:{failedCount}");
        }
    }
}