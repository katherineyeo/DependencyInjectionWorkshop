namespace DependencyInjectionWorkshop.Models
{
    public class AuthenticationService : IAuthentication
    {
        private readonly IProfileDao _ProfileDao;
        private readonly IHash _Hash;
        private readonly IOtp _Otp;

        public AuthenticationService(IProfileDao profileDao, IHash hash, IOtp otp)
        {
            _ProfileDao = profileDao;
            _Hash = hash;
            _Otp = otp;
        }

        public AuthenticationService()
        {
            _ProfileDao = new ProfileDao();
            _Hash = new Sha256Adapter();
            _Otp = new Otp();
        }

        public bool Verify(string accountId, string password, string otp)
        {
            var passwordFromDb = _ProfileDao.GetPassword(accountId);

            var hashedPassword = _Hash.ComputeHash(password);

            var currentOtp = _Otp.GetOtp(accountId);

            if (passwordFromDb == hashedPassword && otp == currentOtp)
            {
                return true;
            }

            return false;
        }
    }
}