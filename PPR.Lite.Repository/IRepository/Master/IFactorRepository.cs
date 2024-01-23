using PPR.Lite.Shared.General;
using PPR.Lite.Shared.Master;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PPR.Lite.Repository.IRepository.Master
{
   public  interface IFactorRepository
    {
        Task<Factor> GetList(UserBase user, string TabIndex);
        Task<ResponseEntity> Save(Factor input);
        Task<Factor> GetDetails(UserBase UserBase, Guid FactorGI, Guid LogGI, string Type);
        Task<ResponseEntity> Delete(UserBase user, Factor model);
        Task<ResponseEntity> ApproveOrReject(Factor model);

    }
}
