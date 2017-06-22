using System.Xml.Serialization;

namespace Library.NCIPServer.Models
{
    public class InitiationHeader
    {
        [XmlElement]
        public FromSystemId FromSystemId { get; set; }
        [XmlElement]
        public FromAgencyId FromAgencyId { get; set; }
        [XmlElement]
        public ToSystemId ToSystemId { get; set; }
        [XmlElement]
        public ToAgencyId ToAgencyId { get; set; }
    }

    
}