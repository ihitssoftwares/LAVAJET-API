using PPR.Lite.Shared.General;
using PPR.Lite.Shared.Master;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PPR.Lite.Repository.IRepository.Master
{
   public  interface IFactorGroupRepository
    {
        Task<FactorGroup> GetList(UserBase user, string TabIndex);
        Task<ResponseEntity> Save(FactorGroup input);
        Task<FactorGroup> GetDetails(UserBase UserBase, Guid FactorGroupGI, Guid LogGI, string Type);
        Task<ResponseEntity> Delete(UserBase user, FactorGroup model);
        Task<ResponseEntity> ApproveOrReject(FactorGroup model);


    }
}
