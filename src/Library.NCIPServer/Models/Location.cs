using System.Collections.Generic;
using System.Xml.Serialization;

namespace Library.NCIPServer.Models
{
    // [XmlRoot]
    public class Location 
    {
        [XmlElement]
        public string LocationType { get; set; }
        [XmlElement]
        public LocationName LocationName { get; set; }

    }
    public class LocationName 
    {
        [XmlElement]
        public LocationNameInstance LocationNameInstance { get; set; }
        [XmlElement]
        public List<LocationNameInstance> LocationNameInstances { get; set; } = new List<LocationNameInstance>();
    }
    public class LocationNameInstance
    {
        [XmlElement]
        public string LocationNameLevel { get; set; }
        [XmlElement]
        public string LocationNameValue { get; set; }        
    }

}