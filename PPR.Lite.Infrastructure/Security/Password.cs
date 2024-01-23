using PPR.Lite.Infrastructure.ISecurity.Abstract;
using PPR.Lite.Shared.Account;
using PPR.Lite.Shared.General;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace PPR.Lite.Infrastructure.Security
{
    public class Password : IPassword
    {
        private readonly PasswordSettings _passwordSettings;
        private readonly OTPSettings _otpSettings;
        public Password(IOptions<AppSettings> settings)
        {
            _passwordSettings = settings.Value?.PasswordSettings;
            _otpSettings = settings.Value?.OTPSettings;
        }
        public string GenerateRandomPassword(PasswordSettings settings)
        {
            PasswordSettings PasswordSettings = new PasswordSettings();
            settings = settings == null ? PasswordSettings : settings;
            var pwd = new PasswordGenerator.Password(
                includeLowercase: settings.IncludeLowercase,
                includeUppercase: settings.IncludeUppercase,
                includeNumeric: settings.IncludeNumeric,
                includeSpecial: settings.IncludeSpecial,
                passwordLength: settings.Length
                );
            return pwd.Next();
        }

        public string GenerateOTP(int? length)
        {

            length = length == null ? _otpSettings.Length : length;
            var pwd = new PasswordGenerator.Password(length.Value).IncludeNumeric();
            return pwd.Next();
        }
    }
}

