using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PPR.Lite.Shared.General;

namespace PPR.Lite.Repository.IRepository.General
{
    public interface IDropdownRepository
    {
        Task<List<Dropdown>> GetJobRoleApplLocCombo(UserBase user);
        Task<List<Dropdown>> GetDepartmentCombo(UserBase user);
        Task<List<Dropdown>> GetGradeCombo(UserBase user);
        Task<List<Dropdown>> GetDesignationCombo(UserBase user);
        Task<List<Dropdown>> GetCountryDropDown(UserBase user);
        Task<List<Dropdown>> GetDistrictDropdown(int? StateCode, UserBase user);
        Task<List<Dropdown>> GetStateDropDown(int? CountryCode, UserBase user);
        Task<List<Dropdown>> GetLocationTypeDropdown(UserBase user);
        Task<List<Dropdown>> GetRegionDropdown(UserBase user);
        Task<List<Dropdown>> GetPTGroupDropDown(UserBase user);
        Task<List<Dropdown>> GetESIGroupDropDown(UserBase user);
        Task<List<Dropdown>> GetInterviewLevelDropdown(UserBase user);
        Task<List<Dropdown>> GetCalenderYearDropdown(UserBase User);
        Task<List<Dropdown>> GetFactorGroupDropDown(UserBase User);
        Task<List<Dropdown>> GetFactorTypeDropDown(UserBase User);
        
        Task<List<Dropdown>> GetUnitDropDown(UserBase User);
        Task<List<Dropdown>> GetLocationDropDown(UserBase User); 
        Task<List<Dropdown>> GetDesignationDropDown(UserBase User);
        Task<List<Dropdown>> GetDepartmentDropDown(UserBase User);
        Task<List<Dropdown>> GetGradeDropDown(UserBase User);
        Task<List<Dropdown>> GetPMSAppraisalAuthorityLevel(UserBase user);
        Task<List<Dropdown>> GetGenderDropDown(UserBase user);
        Task<List<Dropdown>> GetEmployeeDropDown(UserBase user);
        Task<List<Dropdown>> GetPMSAppraisalCombo(UserBase user);
        Task<List<Dropdown>> GetPMSAppraisalRole(UserBase user);
        Task<List<Dropdown>> GetReviewPeriod(UserBase user);
        Task<List<Dropdown>> GetRatingTypeDropDown(int? GetRatingTypeDropDown,UserBase user);
        
    }
}
