using System.Xml.Serialization;

namespace Library.NCIPServer.Models
{
    [XmlRoot]
    public class OrganizationNameInformation
    {
        [XmlElement]
        public string OrganizationNameType { get; set; }
        [XmlElement]
        public string OrganizationName { get; set; }
    }
}