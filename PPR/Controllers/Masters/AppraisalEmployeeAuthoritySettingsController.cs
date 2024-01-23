using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PPR.Lite.Api.Helper;
using PPR.Lite.Repository.IRepository.Master;
using PPR.Lite.Shared;
using PPR.Lite.Shared.PMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPR.Lite.Api.Controllers.Masters
{

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AppraisalEmployeeAuthoritySettingsController : Controller
    {

        #region Private properties
        private readonly IAppraisalEmployeeAuthoritySettingsRepository _repo;
        #endregion
        #region Constructor        
        public AppraisalEmployeeAuthoritySettingsController(IAppraisalEmployeeAuthoritySettingsRepository repo)
        {
            _repo = repo;
        }
        #endregion


        #region Get
        [HttpGet("GetEmployeeAuthoritySettingsDetails")]
        public async Task<IActionResult> GetEmployeeAuthoritySettingsDetails(Guid EmployeeGI, int JobRoleCode)
        {
            return Ok(await _repo.GetEmployeeAuthoritySettingsDetails(EmployeeGI, JobRoleCode, UserHelper.GetUser(User)));
        }

        #endregion
        #region Post
        [HttpPost("Edit")]
        public async Task<IActionResult> Edit(AppraisalEmployeeAuthoritySettings model)
        {
            model.UserBase = UserHelper.GetUser(User);
            return Ok(await _repo.Edit(model));
        }
        [HttpPost("BatchUpdate")]
        public async Task<IActionResult> BatchUpdate(AppraisalEmployeeAuthoritySettings model)
        {
            model.UserBase = UserHelper.GetUser(User);
            return Ok(await _repo.BatchUpdate(model));
        }
        [HttpPost("EmployeeList")]
        public async Task<IActionResult> GetEmployeeList(int JobRoleCode, int AuthorityLevelCode, Filter Filter)
        {
            return Ok(await _repo.GetEmployeeList(JobRoleCode, AuthorityLevelCode, UserHelper.GetUser(User), Filter));
        }
        #endregion
    }
}
