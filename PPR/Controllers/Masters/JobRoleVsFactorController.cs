using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PPR.Lite.Api.Helper;
using PPR.Lite.Repository.IRepository.Master;
using PPR.Lite.Shared.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPR.Lite.Api.Controllers.Masters
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class JobRoleVsFactorController : Controller
    {
        private readonly IJobRoleVsFactorsRepository _repo;
        public JobRoleVsFactorController(IJobRoleVsFactorsRepository repo)
        {
            _repo = repo;
        }

        [HttpGet("TypeList")]
        public async Task<IActionResult> typeList(int JobRoleCode)
        {

            return Ok(await _repo.typeList(UserHelper.GetUser(User), JobRoleCode));
        }
        [HttpGet("FactorList")]
        public async Task<IActionResult> FactorList(int FactorTypeCode, int JobRole)
        {

            return Ok(await _repo.FactorList(UserHelper.GetUser(User), FactorTypeCode, JobRole));
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] RoleVsFactorShedule input)
        {
            input.User = UserHelper.GetUser(User);
            return Ok(await _repo.Save(input));
        }
        [HttpGet("GetMapFactorList")]
        public async Task<IActionResult> GetMapFactorList(int JobRole)
        {

            return Ok(await _repo.GetMapFactorList(UserHelper.GetUser(User), JobRole));
        }
        [HttpPost("MapFactorListCreate")]
        public async Task<IActionResult> MapFactorListCreate([FromBody] RoleVsFactorShedule input)
        {
            input.User = UserHelper.GetUser(User);
            return Ok(await _repo.MapFactorListCreate(input));
        }

    }
}
