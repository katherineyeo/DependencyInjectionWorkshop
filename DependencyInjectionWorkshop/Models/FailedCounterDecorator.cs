namespace DependencyInjectionWorkshop.Models
{
    public class FailedCounterDecorator : BaseAuthenticationDecorator
    {
        private IFailCounter _FailCounter;

        public FailedCounterDecorator(IAuthentication authentication, IFailCounter failCounter) : base(authentication)
        {
            _FailCounter = failCounter;
        }

        public override bool Verify(string accountId, string password, string otp)
        {
            var isValid = base.Verify(accountId, password, otp);
            if (isValid)
            {
                ResetFailedCount(accountId);
            }
            return isValid;
        }

        public void ResetFailedCount(string accountId)
        {
            _FailCounter.ResetFailedCount(accountId);
        }
    }
}