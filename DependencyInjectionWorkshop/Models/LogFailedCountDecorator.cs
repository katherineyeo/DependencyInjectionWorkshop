namespace DependencyInjectionWorkshop.Models
{
    public class LogFailedCountDecorator : BaseAuthenticationDecorator
    {
        private readonly IFailCounter _FailCounter;
        private readonly ILogger _Logger;

        public LogFailedCountDecorator(IAuthentication authenticationService, IFailCounter failCounter, ILogger logger) : base(authenticationService)
        {
            _FailCounter = failCounter;
            _Logger = logger;
        }

        public override bool Verify(string accountId, string password, string otp)
        {
            var isValid = base.Verify(accountId, password, otp);
            if (!isValid)
            {
                LogFailedCount(accountId);
            }
            return isValid;
        }

        public void LogFailedCount(string accountId)
        {
            var failedCount = _FailCounter.GetFailedCount(accountId);
            _Logger.LogInfo($"accountId:{accountId} failed times:{failedCount}");
        }
    }
}