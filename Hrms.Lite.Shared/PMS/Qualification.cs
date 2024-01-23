using PPR.Lite.Shared.General;
using System;
using System.Collections.Generic;
using System.Text;
namespace PPR.Lite.Shared.PMS
{
    public class Qualification : MasterBase 
    {
        public int ActiveCount { get; set; }
        public int InactiveCount { get; set; }
        public int PendingCount { get; set; }
        public string EntryType { get; set; }
        public List<Qualification> QualificationList { get; set; }
        public UserBase User { get; set; }
        public QualificationType QualificationTypeCode { get; set; }
        public int IsProfessional { get; set; }
    }
    public class QualificationType:MasterBase
    {

    }
}
