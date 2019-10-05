namespace DependencyInjectionWorkshop.Models
{
    public class AuthenticationService : IAuthentication
    {
        private readonly IProfileDao _ProfileDao;
        private readonly IHash _Hash;
        private readonly IOtp _Otp;
        private readonly IFailCounter _FailCounter;
        private readonly ILogger _Logger;
        private readonly IAuthentication _NotificationDecorator;
        private readonly FailedCounterDecorator _FailedCounterDecorator;

        public AuthenticationService(IProfileDao profileDao, IHash hash, IOtp otp, IFailCounter failCounter, ILogger logger)
        {
            //_FailedCounterDecorator = new FailedCounterDecorator(this);
            _ProfileDao = profileDao;
            _Hash = hash;
            _Otp = otp;
            _FailCounter = failCounter;
            _Logger = logger;
        }

        public AuthenticationService()
        {
            //_FailedCounterDecorator = new FailedCounterDecorator(this);
            _ProfileDao = new ProfileDao();
            _Hash = new Sha256Adapter();
            _Otp = new Otp();
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
                //_FailedCounterDecorator.ResetFailedCount(accountId);
                return true;
            }
            else
            {
                _FailCounter.AddFailedCount(accountId);

                LogFailedCount(accountId);

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