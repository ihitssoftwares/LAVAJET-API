using PPR.Lite.Api.Helper;
using PPR.Lite.Repository.IRepository.General;
using PPR.Lite.Shared.General;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPR.Lite.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]   
    public class DropdownController : ControllerBase
    {
        private readonly IDropdownRepository _dropdownRepository;

        public DropdownController(IDropdownRepository dropdownRepository)
        {
            _dropdownRepository = dropdownRepository;
        }

        [HttpGet("GetJobRoleApplLocCombo")]
        public async Task<IActionResult> GetJobRoleApplLocCombo()
        {
            return Ok(await _dropdownRepository.GetJobRoleApplLocCombo(UserHelper.GetUser(User)));
        }
        [HttpGet("GetDepartmentCombo")]
        public async Task<IActionResult> GetDepartmentCombo()
        {
            return Ok(await _dropdownRepository.GetDepartmentCombo(UserHelper.GetUser(User)));
        }
        [HttpGet("GetGradeCombo")]
        public async Task<IActionResult> GetGradeCombo()
        { 
            return Ok(await _dropdownRepository.GetGradeCombo(UserHelper.GetUser(User)));
        }
        [HttpGet("GetDesignationCombo")]
        public async Task<IActionResult> GetDesignationCombo()
        {
            return Ok(await _dropdownRepository.GetDesignationCombo(UserHelper.GetUser(User)));
        }
        [HttpGet("CountryDropDown")]
        public async Task<IActionResult> GetCountryDropDown()
        {
            return Ok(await _dropdownRepository.GetCountryDropDown(UserHelper.GetUser(User)));
        }
        [HttpGet("StateDropDown")]
        public async Task<IActionResult> GetStateDropDown(int? CountryCode)
        {
            return Ok(await _dropdownRepository.GetStateDropDown(CountryCode, UserHelper.GetUser(User)));
        }
        [HttpGet("DistrictDropdown")]
        public async Task<IActionResult> GetDistrictDropdown(int? StateCode)
        {
            return Ok(await _dropdownRepository.GetDistrictDropdown(StateCode, UserHelper.GetUser(User)));
        }
        [HttpGet("RegionDropdown")]
        public async Task<IActionResult> RegionDropdown()
        {
            return Ok(await _dropdownRepository.GetRegionDropdown(UserHelper.GetUser(User)));
        }
        [HttpGet("LocationTypeDropdown")]
        public async Task<IActionResult> LocationTypeDropdown()
        {
            return Ok(await _dropdownRepository.GetLocationTypeDropdown(UserHelper.GetUser(User)));
        }

        [HttpGet("GetPTGroupDropDown")]
        public async Task<IActionResult> GetPTGroupDropDown()
        {
            return Ok(await _dropdownRepository.GetPTGroupDropDown(UserHelper.GetUser(User)));
        }


        [HttpGet("GetESIGroupDropDown")]
        public async Task<IActionResult> GetESIGroupDropDown()
        {
            return Ok(await _dropdownRepository.GetESIGroupDropDown(UserHelper.GetUser(User)));
        }
        [HttpGet("GetInterviewLevelDropdown")]
        public async Task<IActionResult> GetInterviewLevelDropdown()
        {
            return Ok(await _dropdownRepository.GetInterviewLevelDropdown(UserHelper.GetUser(User)));
        }
        [HttpGet("GetCalenderYearDropdown")]
        public async Task<IActionResult> GetCalenderYearDropdown()
        {
            return Ok(await _dropdownRepository.GetCalenderYearDropdown(UserHelper.GetUser(User)));
        }

        [HttpGet("GetFactorGroupDropDown")]
        public async Task<IActionResult> GetFactorGroupDropDown()
        {
            return Ok(await _dropdownRepository.GetFactorGroupDropDown(UserHelper.GetUser(User)));
        }


        [HttpGet("GetFactorTypeDropDown")]
        public async Task<IActionResult> GetFactorTypeDropDown()
        {
            return Ok(await _dropdownRepository.GetFactorTypeDropDown(UserHelper.GetUser(User)));
        }
        [HttpGet("GetUnitDropDown")]
        public async Task<IActionResult> GetUnitDropDown()
        {
            return Ok(await _dropdownRepository.GetUnitDropDown(UserHelper.GetUser(User)));
        }


        [HttpGet("GetLocationDropDown")]
        public async Task<IActionResult> GetLocationDropDown()
        {
            return Ok(await _dropdownRepository.GetLocationDropDown(UserHelper.GetUser(User)));
        }
        [HttpGet("GetDesignationDropDown")]
        public async Task<IActionResult> GetDesignationDropDown()
        {
            return Ok(await _dropdownRepository.GetDesignationDropDown(UserHelper.GetUser(User)));
        }
        [HttpGet("GetDepartmentDropDown")]
        public async Task<IActionResult> GetDepartmentDropDown()
        {
            return Ok(await _dropdownRepository.GetDepartmentDropDown(UserHelper.GetUser(User)));
        }
        [HttpGet("GetGradeDropDown")]
        public async Task<IActionResult> GetGradeDropDown()
        {
            return Ok(await _dropdownRepository.GetGradeDropDown(UserHelper.GetUser(User)));
        }

        [HttpGet("GetPMSAppraisalAuthorityLevel")]
        public async Task<ActionResult> GetPMSAppraisalAuthorityLevel()
        {
            return Ok(await _dropdownRepository.GetPMSAppraisalAuthorityLevel(UserHelper.GetUser(User)));
        }

        [HttpGet("GenderDropDown")]
        public async Task<IActionResult> GetGenderDropDown()
        {
            return Ok(await _dropdownRepository.GetGenderDropDown(UserHelper.GetUser(User)));
        }

        [HttpGet("GetEmployeeDropDown")]
        public async Task<IActionResult> GetEmployeeDropDown()
        {
            return Ok(await _dropdownRepository.GetEmployeeDropDown(UserHelper.GetUser(User)));
        }
        [HttpGet("GetPMSAppraisalCombo")]
        public async Task<ActionResult> GetPMSAppraisalCombo()
        {
            return Ok(await _dropdownRepository.GetPMSAppraisalCombo(UserHelper.GetUser(User)));
        }
        [HttpGet("GetPMSAppraisalRole")]
        public async Task<ActionResult> GetPMSAppraisalRole()
        {
            return Ok(await _dropdownRepository.GetPMSAppraisalRole(UserHelper.GetUser(User)));
        }
        
        [HttpGet("GetReviewPeriod")]
        public async Task<IActionResult> GetReviewPeriod()
        {
            return Ok(await _dropdownRepository.GetReviewPeriod(UserHelper.GetUser(User)));
        } [HttpGet("GetRatingTypeDropDown")]
        public async Task<IActionResult> GetRatingTypeDropDown(int? FactorTypeCode)
        {
            return Ok(await _dropdownRepository.GetRatingTypeDropDown(FactorTypeCode,UserHelper.GetUser(User)));
        }

    }
}
