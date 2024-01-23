using PPR.Lite.Shared.General;
using PPR.Lite.Shared.Master;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PPR.Lite.Repository.IRepository.Master
{
   public interface IRatingPaternRepository
    {

        Task<RatingPatern> GetList(UserBase user, string TabIndex);
        Task<ResponseEntity> Save(RatingPatern input);
        Task<RatingPatern> GetDetails(UserBase UserBase, Guid RatingPaternGI, Guid LogGI, string Type);
        Task<ResponseEntity> Delete(UserBase user, RatingPatern model);
        Task<ResponseEntity> ApproveOrReject(RatingPatern model);
    }
}
