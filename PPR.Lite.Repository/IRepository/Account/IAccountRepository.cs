using PPR.Lite.Shared.Account;
using PPR.Lite.Shared.General;
//using PPR.Lite.Shared.Master;
//using PPR.Lite.Shared.Mobile;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace PPR.Lite.Repository.IRepository
{
  public  interface IAccountRepository
    {
        Task<CustomSoftware> GetSoftwareLogo();
        public Task<ResponseEntity<string>>Authenticate(Login credentials);
        public Task<ResponseEntity<string>> Login(Login credentials);
        public Task<ResponseEntity> LogHistory(UserBase UserBase, string CompanyCode, string UserCode,bool LoginSuccess, string Software, string InOutStatus, string MacAddress);
        //Task<UserRoleDetails> GetLogOutUserRoleDetails(UserBase user, int UserRoleCode);

        //#region Mobile       
        //public Task<ResponseEntity<JObject>> Mobile_Authenticate(Login credentials);
        //public Task<ResponseEntity> MobileLogHistory(UserBase UserBase,Login model);

        //#endregion
        Task<List<Dropdown>> GetGroupCompanyDropDown(Login credentials);
    }
}
