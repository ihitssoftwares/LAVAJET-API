using PPR.Lite.Shared.General;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PPR.Lite.Api.Helper
{
    public static  class UserHelper
    {
       
            public static UserBase GetUser(ClaimsPrincipal user)
            {
                var userData = JsonConvert.DeserializeObject<UserBase>(user.FindFirstValue(ClaimTypes.UserData));
                return userData;
            }
        }
    
}
