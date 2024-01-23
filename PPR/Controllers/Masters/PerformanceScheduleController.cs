using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PPR.Lite.Api.Helper;
using PPR.Lite.Repository.IRepository.Master;
using PPR.Lite.Shared.Master;
using PPR.Lite.Shared.PMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPR.Lite.Api.Controllers.Masters
{
    [Route("api/[controller]")]
    [ApiController]
    public class PerformanceScheduleController : ControllerBase
    {
        private readonly IPerformanceScheduleRepository _repo;
        public PerformanceScheduleController(IPerformanceScheduleRepository repo)
        {
            _repo = repo;
        }
        [HttpPost("EmployeeList")]
        public async Task<IActionResult> GetList(int ReviewPeriodCode, int Schedule, Filter Filter)
        {

            return Ok(await _repo.GetList(UserHelper.GetUser(User), ReviewPeriodCode, Schedule, Filter));
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] PerformanceSchedule input)
        {
            input.User = UserHelper.GetUser(User);
            return Ok(await _repo.Save(input));
        }
        [HttpGet("ResetAppraisal")]
        public async Task<IActionResult> ResetAppraisal(int ReviewPeriodCode,  Guid EmployeeGI)
        {
            return Ok(await _repo.ResetAppraisal(UserHelper.GetUser(User), ReviewPeriodCode,  EmployeeGI));
        }
        [HttpPost("BatchUpdate")]
        public async Task<IActionResult> BatchUpdate([FromBody] PerformanceSchedule input)
        {
            input.User = UserHelper.GetUser(User);
            return Ok(await _repo.BatchUpdate(input));
        }


    }
}
