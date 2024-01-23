using PPR.Lite.Shared.DataBank.Generic.Master;
using PPR.Lite.Shared.General;
using System;
using System.Collections.Generic;
using System.Text;
namespace PPR.Lite.Shared.Master
{
    public class Location : MasterBase
    {
        public string Area { get; set; }
        public int ActiveCount { get; set; }
        public int InactiveCount { get; set; }
        public int PendingCount { get; set; }
        public string DoorNumAndBuilding { get; set; }
        public string Street { get; set; }
        public State State { get; set; }
        public Country Country { get; set; }
        public District District { get; set; }
        public List<Location> LocationList { get; set; }
        public LocationType LocationType { get; set; }
        public Region Region { get; set; }
        public string LandLineNum { get; set; }
        public string EmailId { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public PTGroup PTgroup { get; set; }
        public ESIGroup ESIgroup { get; set; }
        public string WebAdderss { get; set; }
        public string GeoAdderss { get; set; }
        public UserBase User { get; set; }
    }
    public class LocationType : MasterBase
    {
    }

    
}

    

