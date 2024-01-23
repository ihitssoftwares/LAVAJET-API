using PPR.Lite.Shared.General;
using PPR.Lite.Shared.Master;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PPR.Lite.Repository.IRepository.Master
{
   public interface IJobRoleRepository
    {
        Task<JobRole> GetList(UserBase User, string Tabindex);
        Task<ResponseEntity> Save(JobRole input);
        Task<ResponseEntity> Delete(UserBase user, JobRole model);
        Task<ResponseEntity> ApproveOrReject(JobRole model);

        Task<JobRole> GetDetails(UserBase UserBase, Guid JobRoleGI, Guid LogGI, string Type);
    }
}
