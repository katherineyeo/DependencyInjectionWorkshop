namespace DependencyInjectionWorkshop.Models
{
    public class BaseAuthenticationDecorator : IAuthentication
    {
        private readonly IAuthentication _Authentication;

        public BaseAuthenticationDecorator(IAuthentication authentication)
        {
            _Authentication = authentication;
        }

        public virtual bool Verify(string accountId, string password, string otp)
        {
            return _Authentication.Verify(accountId, password, otp);
        }
    }
}