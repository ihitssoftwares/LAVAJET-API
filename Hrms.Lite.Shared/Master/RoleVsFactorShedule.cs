using PPR.Lite.Shared.General;
using PPR.Lite.Shared.PMS;
using System;
using System.Collections.Generic;
using System.Text;

namespace PPR.Lite.Shared.Master
{
    public class RoleVsFactorShedule:MasterBase
    {
        public Guid FactorGI { get; set; }
        public Guid JobRoleGI { get; set; }
        public MasterBase AppraisalRole { get; set; }
        public MasterBase JobRole { get; set; }
        public UserBase User { get; set; }
        public PMS_Appraisal Appraisal { get; set; }

        public JobRoleVsFactorType JobRoleVsFactorType { get; set; }
        public JobRoleVsFactorFactors JobRoleVsFactorFactors { get; set; }
        public List<RoleVsFactorShedule> TypeList { get; set; }
        public List<RoleVsFactorShedule> FactorList { get; set; }
        public List<MapFactor> MapFactorList { get; set; }
        public int TypeCode { get; set; }
    }
    public class JobRoleVsFactorType
    {
        public int FactorTypeCode { get; set; }
        public string Name { get; set; }
        public bool Applicability { get; set; }
        public int Weightage { get; set; }


    }
    public class JobRoleVsFactorFactors
    {
        public int FactorCode { get; set; }
        public string FactorName { get; set; }
        public int FactorGroupCode { get; set; }
        public string FactorGroupName { get; set; }
        public bool Applicability { get; set; }
        public int Weightage { get; set; }
        public int Target { get; set; }
        public int UnitCode { get; set; }
        public int RatingType { get; set; }
        public string RatingTypeName { get; set; }
        public string UnitName { get; set; }

    }

    public class MapFactor
    {
        public int AppraisalCode { get; set; }
        public int FactorCode { get; set; }
        public int RoleCode { get; set; }
        public string FactorName { get; set; }
        public int FactorGroupCode { get; set; }
        public string FactorGroupName { get; set; }
        public bool Applicability { get; set; }
        public int Weightage { get; set; }
        public string FactorTypeName { get; set; }
    }


}
