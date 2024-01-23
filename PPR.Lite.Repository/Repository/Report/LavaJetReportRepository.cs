using PPR.Lite.Repository.IRepository;

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using PPR.Lite.Repository.IRepository.Report;

namespace PPR.Lite.Repository.Repository.Report
{
    public class LavaJetReportRepository: ILavaJetReportRepository
    {
        private readonly ISqlConnectionProvider _sql;
        public LavaJetReportRepository(ISqlConnectionProvider sql)
        {
            _sql = sql;
        }
        public async Task<JObject> GetCustomerInvoice(int CustCode, string InvPassNo)
        {
            IDbDataParameter[] sqlparameters =
        {
                 new SqlParameter("@CustCode",CustCode),
                new SqlParameter("@InvPassNo",InvPassNo)

            };

            var ds = await _sql.ExecuteJsonAsync("[dbo].[USP_WEB_CUSTOMER_INVOICE_JSON]", sqlparameters);
            return ds;
            
        }
        public async Task<JObject> GetCustomerReceipt(int CustCode, string InvPassNo,int ReceiptType)
        {
            IDbDataParameter[] sqlparameters =
        {
                 new SqlParameter("@CustCode",CustCode),
                new SqlParameter("@InvPassNo",InvPassNo),
                new SqlParameter("@ReceiptType",ReceiptType)

            };

            var ds = await _sql.ExecuteJsonAsync("[dbo].[USP_WEB_CUSTOMER_RECEIPT_JSON]", sqlparameters);
            return ds;

        }
        public async Task<JObject> GetCustomerClearenceCertificate(int ClearanceCode)
        {
            IDbDataParameter[] sqlparameters =
        {
                 new SqlParameter("@ClearanceCode",ClearanceCode),
               

            };

            var ds = await _sql.ExecuteJsonAsync("[dbo].[USP_WEB_LICENSE_COMPLETION_CERTIFICATE_DETAILS_JSON]", sqlparameters);
            return ds;

        }

        public async Task<JObject> GetVehiclePermit(int QRCode)
        {
            IDbDataParameter[] sqlparameters =
        {
                 new SqlParameter("@QRCode",QRCode),


            };

            var ds = await _sql.ExecuteJsonAsync("[dbo].[USP_WEB_GET_VEHICLE_ENTRY_PERMIT_DATA_JSON]", sqlparameters);
            return ds;

        }
    }
}
