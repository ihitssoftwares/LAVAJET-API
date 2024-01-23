using PPR.Lite.Shared;
using PPR.Lite.Shared.General;
using PPR.Lite.Shared.PMS;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PPR.Lite.Repository.IRepository.Master
{
    public interface IAppraisalEmployeeAuthoritySettingsRepository
    {
        public Task<PMS_Appraisal_Employee> GetEmployeeList(int JobRoleCode, int AuthorityLevelCode, UserBase user, Filter Filter);
        public Task<AppraisalEmployeeAuthoritySettings> GetEmployeeAuthoritySettingsDetails(Guid EmployeeGI, int JobRoleCode, UserBase User);
        public Task<ResponseEntity> Edit(AppraisalEmployeeAuthoritySettings model);
        public Task<ResponseEntity> BatchUpdate(AppraisalEmployeeAuthoritySettings model);

    }
}
