using PPR.Lite.Api.Helper;
using PPR.Lite.Repository.IRepository.Master;
using PPR.Lite.Shared.Master;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPR.Lite.Api.Controllers.Masters
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly ILocationRepository _repo;


        public LocationController(ILocationRepository repo)
        {
            _repo = repo;
        }

        [HttpGet("List")]
        public async Task<IActionResult> GetList(string TabIndex)
        {
            return Ok(await _repo.GetList(UserHelper.GetUser(User), TabIndex));
        }

        [HttpGet("GetLocationDetails")]
        public async Task<IActionResult> GetLocationDetails(Guid LocationGI, Guid LogGI, string Type)
        {
            return Ok(await _repo.GetLocationDetails(UserHelper.GetUser(User), LocationGI, LogGI, Type));
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(Location input)
        {
            input.User = UserHelper.GetUser(User);
            return Ok(await _repo.Save(input));
        }

        [HttpPost("ApproveOrReject")]
        public async Task<IActionResult> ApproveOrReject(Location model)
        {
            model.User = UserHelper.GetUser(User);
            return Ok(await _repo.ApproveOrReject(model));
        }

        [HttpPost("Delete")]
        public async Task<IActionResult> Delete(Location model)
        {
            
            return Ok(await _repo.Delete(UserHelper.GetUser(User), model));

        }
    }
}
