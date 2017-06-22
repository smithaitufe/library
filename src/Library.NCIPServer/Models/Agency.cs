using System.Xml.Serialization;

namespace Library.NCIPServer.Models
{
    public class AgencyId
    {
        [XmlAttribute]
        public string Scheme { get; set; }
        [XmlText]
        public string Value { get; set; }
    }
    
    public class AgencyElementType
    {
        [XmlAttribute]
        public string Scheme { get; set; }
        [XmlText]
        public string Value { get; set; }
    }
    public class ToAgencyId
    {
        [XmlElement]
        public AgencyId AgencyId { get; set; }
    }
    public class FromAgencyId
    {
        [XmlElement]
        public AgencyId AgencyId { get; set; }
    }
}