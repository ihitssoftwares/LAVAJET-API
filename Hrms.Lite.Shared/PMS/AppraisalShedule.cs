using PPR.Lite.Shared;
using PPR.Lite.Shared.General;
using System;
using System.Collections.Generic;
using System.Text;

namespace PPR.Lite.Shared.PMS
{
    public class AppraisalShedule
    {
        public Guid AppraisalGI { get; set; }
        public Guid EmployeeGI { get; set; }
        public Guid AppraisalRoleGI { get; set; }       
        public UserBase User { get; set; }
        public MasterBase AppraisalRole { get; set; }
        public PMS_Appraisal PMS_Appraisal { get; set; }

        public AppraisalVsFactorType AppraisalVsFactorType { get; set; }
        public AppraisalVsFactorFactors AppraisalVsFactorFactors { get; set; }
        public List<AppraisalShedule> TypeList { get; set; }
        public List<AppraisalShedule> FactorList { get; set; }
        public List<ApparisalMapFactor> ApparisalMapFactorList { get; set; }
        public int TypeCode { get; set; }
    }
    public class AppraisalVsFactorType
    {
        public int AppraisalTypeCode { get; set; }
        public string Name { get; set; }
        public bool Applicability { get; set; }
        public int Weightage { get; set; }


    }
    public class AppraisalVsFactorFactors
    {
        public int FactorCode { get; set; }
        public string FactorName { get; set; }
        public int CategoryCode { get; set; }
        public string CategoryName { get; set; }
        public bool Applicability { get; set; }
        public int Weightage { get; set; }


    }

    public class ApparisalMapFactor
    {
        public int AppraisalCode { get; set; }
        public int FactorCode { get; set; }
        public int RoleCode { get; set; }
        public string FactorName { get; set; }
        public int CategoryCode { get; set; }
        public string CategoryName { get; set; }
        public bool Applicability { get; set; }
        public int Weightage { get; set; }
        public string AppraisalType { get; set; }
    }

}
