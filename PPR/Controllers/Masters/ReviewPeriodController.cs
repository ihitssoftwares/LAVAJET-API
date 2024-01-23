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

    [Route("api/[controller]")]
    [ApiController]
    public class ReviewPeriodController : Controller
    {
        private readonly IReviewPeriodRepository _repo;

        public ReviewPeriodController(IReviewPeriodRepository repo)
        {
            _repo = repo;
        }
        [HttpGet("List")]
        public async Task<IActionResult> GetList(string TabIndex)
        {
            return Ok(await _repo.GetList(UserHelper.GetUser(User), TabIndex));
        }

        [HttpGet("ReviewPeriodDetails")]
        public async Task<IActionResult> GetDeptDetails(Guid ReviewPeriodGI, Guid LogGI, string Type)
        {
           return Ok(await _repo.GetDetails(UserHelper.GetUser(User), ReviewPeriodGI, LogGI, Type));
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(ReviewPeriod input)
        {
            input.User = UserHelper.GetUser(User);
            return Ok(await _repo.Save(input));
        }
        [HttpPost("Delete")]
        public async Task<IActionResult> Delete(ReviewPeriod model)
        {
            model.User = UserHelper.GetUser(User);
            return Ok(await _repo.Delete(UserHelper.GetUser(User), model));

        }
        [HttpPost("ApproveOrReject")]
        public async Task<IActionResult> ApproveOrReject(ReviewPeriod model)
        {
            model.User = UserHelper.GetUser(User);
            return Ok(await _repo.ApproveOrReject(model));
       }

        [HttpGet("GetLevel")]
        public async Task<IActionResult> GetLevel()
        {
            return Ok(await _repo.GetLevel(UserHelper.GetUser(User)));

        }
    }
}
