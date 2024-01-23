using PPR.Lite.Infrastructure.ISecurity;
using PPR.Lite.Infrastructure.ISecurity.Abstract;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace PPR.Lite.Infrastructure.Security
{
   public class Hash:IHash
    {
        public void CreatePasswordHash(string password, out string passwordHash, out string passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                var salt = hmac.Key;
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                passwordSalt = Convert.ToBase64String(salt);
                passwordHash = Convert.ToBase64String(hash);
            }
        }

        public bool ComparePasswordHash(string password, string storedHash, string storedSalt)
        {
            var hash = Convert.FromBase64String(storedHash);
            var salt = Convert.FromBase64String(storedSalt);
            using (var hmac = new HMACSHA512(salt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != hash[i]) return false;
                }
            }
            return true;
        }
    }
}
