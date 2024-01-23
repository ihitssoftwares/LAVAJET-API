//using Hrms.Lite.Api.Helper;
//using Hrms.Lite.Repository.IRepository;
//using Hrms.Lite.Shared.Account;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
using PPR.Lite.Api.Helper;
using PPR.Lite.Repository.IRepository;
using PPR.Lite.Shared.Account;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
//using PPR.Lite.Shared.Mobile;
using PPR.Lite.Shared.General;

namespace PPR.Lite.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IAccountRepository _AccountRepository;

        public AccountController(IAccountRepository AccountRepository)
        {
            _AccountRepository = AccountRepository;
        }


        [HttpGet("GetSoftwareLogo")]
        public async Task<IActionResult> GetSoftwareLogo()
        {
            return Ok(await _AccountRepository.GetSoftwareLogo());
        }
      
        [HttpPost("Authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] Login input)
        {
            //Login credentials = new Login { CompanyCode = "IHS", Password = "Ihits", UserCode = "Ihits" };
            return Ok(await  _AccountRepository.Authenticate(input));
        }[HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] Login input)
        {
            //Login credentials = new Login { CompanyCode = "IHS", Password = "Ihits", UserCode = "Ihits" };
            return Ok(await  _AccountRepository.Login(input));
        }
     

        [HttpPost("LogHistory")]
        public async Task<IActionResult> LogHistory(Login model)
        {
            //Software ="EESP/HRMS ; InOutStatus="IN/OUT" ; MacAddress=(IF MACADDRESS KNOWS THEN PASS ,OTHERWISE PASS NULL)"
            if (model.INOUT == "OUT")
                return Ok(await _AccountRepository.LogHistory((UserHelper.GetUser(User)),model.CompanyCode,model.UserCode,model.LoginSuccess ,model.software, model.INOUT, model.MACAddress));
            else
            {
                UserBase userBase = new UserBase();
                userBase.CompanyGuid = Guid.Parse("00000000-0000-0000-0000-000000000000");
                userBase.UserGuid = Guid.Parse("00000000-0000-0000-0000-000000000000");
                return Ok(await _AccountRepository.LogHistory(userBase, model.CompanyCode, model.UserCode, model.LoginSuccess,model.software, model.INOUT, model.MACAddress));
            }
        }
        //[HttpGet("GetLogOutUserRoleDetails")]
        //public async Task<IActionResult> GetLogOutUserRoleDetails(int UserRoleCode)
        //{
        //    return Ok(await _AccountRepository.GetLogOutUserRoleDetails(UserHelper.GetUser(User), UserRoleCode));
        //}

        //#region Mobile App Services
        //[HttpPost("MobileLogin")]
        
        //public async Task<IActionResult> MobileLogin([FromBody] Login input)
        //{ 
        //    return Ok(await _AccountRepository.Mobile_Authenticate(input));
        //}
        //[HttpPost("MobileLogHistory")]
        //public async Task<IActionResult> MobileLogHistory(Login model)
        //{
        //    //Software ="EESP/HRMS ; InOutStatus="IN/OUT" ; MacAddress=(IF MACADDRESS KNOWS THEN PASS ,OTHERWISE PASS NULL)"
        //    return Ok(await _AccountRepository.MobileLogHistory((UserHelper.GetUser(User)),model));
        //}
        //#endregion

        [HttpPost("GetGroupCompanyDropDown")]
        public async Task<IActionResult> GetGroupCompanyDropDown([FromBody] Login input)
        {
            return Ok(await _AccountRepository.GetGroupCompanyDropDown(input));
        }
    }
}

