
using PPR.Lite.Shared.General;
using PPR.Lite.Shared.Master;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PPR.Lite.Repository.IRepository.Master
{
   public interface ILocationRepository
    {
        Task<Location> GetList(UserBase user, string TabIndex);
        Task<Location> GetLocationDetails(UserBase user, Guid LocationGI, Guid LogGI, String Type);
        Task<ResponseEntity> Save(Location input);
        Task<ResponseEntity> ApproveOrReject(Location model);
        Task<ResponseEntity> Delete(UserBase user, Location model);
    }
}
