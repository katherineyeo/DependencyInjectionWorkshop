namespace DependencyInjectionWorkshop.Models
{
    public class NotificationDecorator : BaseAuthenticationDecorator
    {
        private readonly INotification _Notification;

        public NotificationDecorator(IAuthentication authentication, INotification notification) : base(authentication)
        {
            _Notification = notification;
        }

        public void Notify(string accountId)
        {
            _Notification.Notify($"{accountId} try to login, failed");
        }

        public override bool Verify(string accountId, string password, string otp)
        {
            var isValid = base.Verify(accountId, password, otp);
            if (!isValid)
            {
                Notify(accountId);
            }

            return isValid;
        }
    }
}