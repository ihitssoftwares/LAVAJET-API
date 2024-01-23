using Microsoft.AspNetCore.Mvc;
using PPR.Lite.Api.Helper;
using PPR.Lite.Repository.IRepository.Master;
using PPR.Lite.Shared.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPR.Lite.Api.Controllers.Masters
{
  
    [Route("api/[controller]")]
    [ApiController]
    public class PPRSelfController : Controller
    {

        #region Private properties
        private readonly IPPRSelfRepository _repo;
        #endregion

        #region Constructor        
        public PPRSelfController(IPPRSelfRepository repo)
        {
            _repo = repo;
        }
        #endregion
        #region Self Appraisal
        #region "GET"

        [HttpGet("List")]
        public async Task<IActionResult> GetList()
        {

            return Ok(await _repo.GetList(UserHelper.GetUser(User)));
        }
        [HttpGet("GetAppraisalEmpDetails")]
        public async Task<IActionResult> GetAppraisalEmpDetails(int EmployeeCode, int AppraisalCode, int CombinationCode)
        {
            return Ok(await _repo.GetAppraisalEmpDetails(UserHelper.GetUser(User), EmployeeCode, AppraisalCode, CombinationCode));
        }
        [HttpGet("GetTypeList")]
        public async Task<IActionResult> GetTypeList(int HeadCode, int level)
        {
            return Ok(await _repo.GetTypeList(UserHelper.GetUser(User), HeadCode, level));
        }
        [HttpGet("GetLevelList")]
        public async Task<IActionResult> GetLevelList(int HeadCode, int level)
        {
            return Ok(await _repo.GetLevelList(UserHelper.GetUser(User), HeadCode, level));
        }
        [HttpGet("GetAppraisalCatList")]
        public async Task<IActionResult> GetAppraisalCatList(int HeadCode, int level, int ApprisalTypeCode)
        {
            return Ok(await _repo.GetAppraisalCatList(UserHelper.GetUser(User), HeadCode, level, ApprisalTypeCode));
        }
        [HttpGet("GetAppraisalCommentList")]
        public async Task<IActionResult> GetAppraisalCommentList(int HeadCode, int level)
        {
            return Ok(await _repo.GetAppraisalCommentList(UserHelper.GetUser(User), HeadCode, level));
        }
        [HttpGet("GetHistory")]
        public async Task<IActionResult> GetHistory(int HeadCode, int FactorCode)
        {
            return Ok(await _repo.GetHistory(UserHelper.GetUser(User), HeadCode, FactorCode));
        }


        [HttpGet("ApprisalStart")]
        public async Task<IActionResult> ApprisalStart(int EmployeeCode, int AppraisalCode, int LevelCode, int CombinationCode)
        {

            return Ok(await _repo.ApprisalStart(UserHelper.GetUser(User), EmployeeCode, AppraisalCode, LevelCode, CombinationCode));
        }
        [HttpGet("CheckFinalSubmit")]
        public async Task<IActionResult> CheckFinalSubmit(int HeadCode, int level)
        {
            return Ok(await _repo.CheckFinalSubmit(UserHelper.GetUser(User), HeadCode, level));
        }
        [HttpGet("ValidateFinalSubmit")]
        public async Task<IActionResult> ValidateFinalSubmit(int LevelCode, int ApprCode, int EmpCodeFinalCheck)
        {
            return Ok(await _repo.ValidateFinalSubmit(UserHelper.GetUser(User), LevelCode, ApprCode, EmpCodeFinalCheck));
        }
        [HttpGet("GetFactorHistorySummary")]
        public async Task<IActionResult> GetFactorHistorySummary(int EmployeeCode, int AppraisalCode, int CombinationCode)
        {
            return Ok(await _repo.GetFactorHistorySummary(UserHelper.GetUser(User), EmployeeCode, AppraisalCode, CombinationCode));
        }
        #endregion
        #region POST
        [HttpPost("SaveSelfAppraisal")]
        public async Task<IActionResult> SaveSelfAppraisal(PPRSelf input)
        {

            input.User = UserHelper.GetUser(User);
            return Ok(await _repo.Save(input));
        }
        #endregion
        #endregion
        #region Approval
        [HttpGet("GetApprovalList")]
        public async Task<IActionResult> GetApprovalList(int EmployeeCode)
        {

            return Ok(await _repo.GetApprovalList(UserHelper.GetUser(User), EmployeeCode));
        }
        [HttpPost("SaveReReviewDetails")]
        public async Task<IActionResult> SaveReReviewDetails(PPRSelf input)
        {

            input.User = UserHelper.GetUser(User);
            return Ok(await _repo.Save(input));
        }
        [HttpGet("GetRatingValue")]
        public async Task<IActionResult> GetRatingValue(int Targetval, int AchieveValue, int TypeCode)
        {

            return Ok(await _repo.GetRatingValue(UserHelper.GetUser(User), Targetval, AchieveValue, TypeCode));
        }
        #endregion
    }
}
