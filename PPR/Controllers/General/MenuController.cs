using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PPR.Lite.Api.Helper;
using PPR.Lite.Repository.IRepository.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPR.Lite.Api.Controllers.General
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : Controller
    {
        #region Private properties
        private readonly IMenuRepository _repo;
        #endregion
        #region Constructor        
        public MenuController(IMenuRepository repo)
        {
            _repo = repo;
        }
        #endregion
        [HttpGet("GetMenu")]
        public async Task<IActionResult> GetMenu(string LoginFrom)
        {
            return Ok(await _repo.GetMenu(UserHelper.GetUser(User), LoginFrom));
        }
        [HttpGet("GetModuleList")]
        public async Task<IActionResult> GetModuleList(string LoginFrom)
        {
            return Ok(await _repo.GetModuleList(UserHelper.GetUser(User), LoginFrom));
        }
        [HttpGet("GetModuleVsMenuList")]
        public async Task<IActionResult> GetModuleVsMenuList(int ModuleCode)
        {
            return Ok(await _repo.GetModuleVsMenuList(UserHelper.GetUser(User), ModuleCode));
        }
        [HttpGet("GetMenuDetails")]
        public async Task<IActionResult> GetMenuDetails(int MenuCode)
        {
            return Ok(await _repo.GetMenuDetails(UserHelper.GetUser(User), MenuCode));
        }


    }
}
