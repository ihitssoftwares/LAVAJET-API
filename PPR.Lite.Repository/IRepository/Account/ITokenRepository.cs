using PPR.Lite.Shared.General;
using System;
using System.Collections.Generic;
using System.Text;

namespace PPR.Lite.Repository.IRepository.Account
{
  public  interface ITokenRepository
    {
         string GenerateToken<T>(AppTokenSettings<T> tokenSettings);
    }
}
