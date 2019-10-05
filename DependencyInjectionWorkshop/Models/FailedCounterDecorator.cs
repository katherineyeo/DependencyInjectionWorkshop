namespace DependencyInjectionWorkshop.Models
{
    public class FailedCounterDecorator : BaseAuthenticationDecorator
    {
        private readonly IFailCounter _FailCounter;

        public FailedCounterDecorator(IAuthentication authentication, IFailCounter failCounter) : base(authentication)
        {
            _FailCounter = failCounter;
        }

        public override bool Verify(string accountId, string password, string otp)
        {
            CheckIsAccountLocked(accountId);
            var isValid = base.Verify(accountId, password, otp);
            if (isValid)
            {
                ResetFailedCount(accountId);
            }
            else
            {
                AddFailedCount(accountId);
            }
            return isValid;
        }

        private void ResetFailedCount(string accountId)
        {
            _FailCounter.ResetFailedCount(accountId);
        }

        private void AddFailedCount(string accountId)
        {
            _FailCounter.AddFailedCount(accountId);
        }

        private void CheckIsAccountLocked(string accountId)
        {
            if (_FailCounter.IsAccountLocked(accountId))
            {
                throw new FailedTooManyTimesException();
            }
        }
    }
}