using PPR.Lite.Shared.General;
using PPR.Lite.Shared.Master;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PPR.Lite.Repository.IRepository.Master
{
   public interface IJobRoleVsFactorsRepository
    {
        Task<RoleVsFactorShedule> typeList(UserBase user, int JobRoleCode);
        Task<RoleVsFactorShedule> FactorList(UserBase user, int FactorTypeCode, int JobRole);

        Task<ResponseEntity> Save(RoleVsFactorShedule input);

        Task<ResponseEntity> MapFactorListCreate(RoleVsFactorShedule input);
        Task<RoleVsFactorShedule> GetMapFactorList(UserBase user, int JobRole);
    }
}
