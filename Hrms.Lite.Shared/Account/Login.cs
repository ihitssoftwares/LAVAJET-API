using PPR.Lite.Shared.DataBank.DTO;
using System;
using System.Collections.Generic;
using System.Text;
//using PPR.Lite.Shared.Mobile;
using PPR.Lite.Shared.General;

namespace PPR.Lite.Shared.Account
{
    public class Login
    {
        public AuthUser authUser { get; set; }
        public string CompanyCode { get; set; }
        public string UserCode { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public string CompanyName { get; set; }
        public string MACAddress { get; set; }
        public string INOUT { get; set; }
        public string software { get; set; }   
        //#Mobile 
        //public MobDeviceData DeviceData {get; set;}
        public int GroupCompanyCode { get; set; }
        public string GroupCompanyName { get; set; }
        public string LoginStatus { get; set; }
        public bool LoginSuccess { get; set; }

    }
    public class CustomSoftware
    {
        public File SoftwareLogo { get; set; }
        public File SoftwareName { get; set; } //ESSP SoftwareName
        public File HRMS_SoftwareName { get; set; }
    }
}
