using DependencyInjectionWorkshop.Models;
using NSubstitute;
using NUnit.Framework;

namespace DependencyInjectionWorkshopTests
{
    [TestFixture]
    public class AuthenticationServiceTests
    {
        private const string _DefaultHashedPassword = "my hashed password";
        private const string _DefaultAccount = "joey";
        private const string _DefaultOtp = "123";
        private const string _DefaultPassword = "abc";
        private IProfileDao _ProfileDao;
        private IOtp _Otp;
        private IHash _Hash;
        private IFailCounter _FailCounter;
        private INotification _Notification;
        private ILogger _Logger;
        private AuthenticationService _AuthenticationService;

        [SetUp]
        public void Setup()
        {
            _ProfileDao = Substitute.For<IProfileDao>();
            _Otp = Substitute.For<IOtp>();
            _Hash = Substitute.For<IHash>();
            _FailCounter = Substitute.For<IFailCounter>();
            _Notification = Substitute.For<INotification>();
            _Logger = Substitute.For<ILogger>();
            _AuthenticationService = new AuthenticationService(_ProfileDao, _Hash, _Otp, _Notification, _FailCounter, _Logger);
        }

        [Test]
        public void is_valid()
        {
            GivenPassword(_DefaultAccount, _DefaultHashedPassword);

            GivenHash(_DefaultPassword, _DefaultHashedPassword);

            GivenOtp(_DefaultAccount, _DefaultOtp);
            
            var isValid = _AuthenticationService.Verify(_DefaultAccount, _DefaultPassword, _DefaultOtp);

            Assert.IsTrue(isValid);
        }

        private void GivenHash(string password, string hashedPassword)
        {
            _Hash.ComputeHash(password).Returns(hashedPassword);
        }

        private void GivenOtp(string account, string otp)
        {
            _Otp.GetOtp(account).Returns(otp);
        }

        private void GivenPassword(string account, string hashedPassword)
        {
            _ProfileDao.GetPassword(account).Returns(hashedPassword);
        }
    }
}