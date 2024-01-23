using PPR.Lite.Shared.General;
using System;
using System.Collections.Generic;
using System.Text;

namespace PPR.Lite.Shared.Master
{
   public class Category:MasterBase
    {

        public int ActiveCount { get; set; }
        public int PendingCount { get; set; }
        public int InActiveCount { get; set; }
        public List<Category> CategoryList { get; set; }
       // public CategoryType CategoryType { get; set; }
        public UserBase User { get; set; }
        public int? ExpiryPeriod { get; set; }
    }
}
