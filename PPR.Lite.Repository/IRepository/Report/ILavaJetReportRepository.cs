using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PPR.Lite.Repository.IRepository.Report
{
    public interface ILavaJetReportRepository
    {
        public Task<JObject> GetCustomerInvoice(int CustCode, string InvPassNo);
        public Task<JObject> GetCustomerReceipt(int CustCode, string InvPassNo,int ReceiptType);
        public Task<JObject> GetCustomerClearenceCertificate(int ClearanceCode);
        public Task<JObject> GetVehiclePermit(int QRCode);
    }
}
