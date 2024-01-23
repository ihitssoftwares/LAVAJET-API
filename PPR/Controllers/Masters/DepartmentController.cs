using PPR.Lite.Api.Helper;
using PPR.Lite.Repository.IRepository;
using PPR.Lite.Repository.IRepository.Master;
using PPR.Lite.Shared.Master;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using PPR.Lite.Shared.General;


namespace PPR.Lite.Api.Controllers.Masters
{

    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentRepository _repo;
       
        public DepartmentController(IDepartmentRepository repo)
        {
            _repo = repo;
        }

        [HttpGet("List")]
        public async Task<IActionResult> GetList(string TabIndex)
        {
            //UserBase user = new UserBase { UserGuid = new Guid("2CAC1F4A-CFB9-4EDC-B824-3C720087DDD2"), CompanyGuid = new Guid("8EA0DCCB-1CF2-46E8-96A0-DD9AED61D439") };
            return Ok(await _repo.GetList(UserHelper.GetUser(User), TabIndex));
        }

        [HttpGet("DepartmentDetails")]
        public async Task<IActionResult> GetDeptDetails(Guid DepartmentGI, Guid LogGI, string Type)
        {
            return Ok(await _repo.GetDetails(UserHelper.GetUser(User), DepartmentGI,LogGI,Type));
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(Department input)
        {
            input.User = UserHelper.GetUser(User);
            return Ok(await _repo.Save(input));
        }
        [HttpPost("Delete")]
        public async Task<IActionResult> Delete(Department model)
        {
            //model.User = UserHelper.GetUser(User);
            return Ok(await _repo.Delete(UserHelper.GetUser(User),model));

        }
        [HttpPost("ApproveOrReject")]
        public async Task<IActionResult> ApproveOrReject(Department model)
        {
            model.User = UserHelper.GetUser(User);
            return Ok(await _repo.ApproveOrReject(model));
        }

    }
}
