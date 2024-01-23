using PPR.Lite.Shared.Account;
using System;
using System.Collections.Generic;
using System.Text;

namespace PPR.Lite.Infrastructure.ISecurity.Abstract
{
    public interface IPassword
    {
        public string GenerateRandomPassword(PasswordSettings settings);
        public string GenerateOTP(int? length);
    }
}
