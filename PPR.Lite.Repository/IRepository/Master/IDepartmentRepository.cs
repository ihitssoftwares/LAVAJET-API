using PPR.Lite.Shared.General;
using PPR.Lite.Shared.Master;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PPR.Lite.Repository.IRepository.Master
{
    public interface IDepartmentRepository
    {
        Task<Department> GetList(UserBase user, string TabIndex);
        Task<ResponseEntity> Save(Department input);
        Task<Department> GetDetails(UserBase UserBase, Guid DepartmentGI, Guid LogGI, string Type);
        Task<ResponseEntity> Delete(UserBase user,Department model);
        Task<ResponseEntity> ApproveOrReject(Department model);

    }
}
