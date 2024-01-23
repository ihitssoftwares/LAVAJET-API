using PPR.Lite.Api.Helper;
using PPR.Lite.Repository.IRepository;
using PPR.Lite.Repository.IRepository.Report;
using PPR.Lite.Shared.Master;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using PPR.Lite.Shared.General;

namespace PPR.Lite.Api.Controllers.Report
{
    [Route("api/[controller]")]
    [ApiController]
    public class LavaJetReportController : Controller
    {
        private readonly ILavaJetReportRepository _repo;

        public LavaJetReportController(ILavaJetReportRepository repo)
        {
            _repo = repo;
        }
        [HttpGet("GetCustomerInvoice")]
        public async Task<IActionResult> GetCustomerInvoice(int CustCode, string InvPassNo)
        {


            return Json(await _repo.GetCustomerInvoice(CustCode, InvPassNo));
        }
        [HttpGet("GetCustomerReceipt")]
        public async Task<IActionResult> GetCustomerReceipt(int CustCode, string InvPassNo,int ReceiptType)
        {


            return Json(await _repo.GetCustomerReceipt(CustCode, InvPassNo, ReceiptType));
        }
        [HttpGet("GetCustomerClearenceCertificate")]
        public async Task<IActionResult> GetCustomerClearenceCertificate(int ClearanceCode)
        {


            return Json(await _repo.GetCustomerClearenceCertificate(ClearanceCode));
        }
        [HttpGet("GetVehiclePermit")]
        public async Task<IActionResult> GetVehiclePermit(int QRCode)
        {


            return Json(await _repo.GetVehiclePermit(QRCode));
        }
    }
}
