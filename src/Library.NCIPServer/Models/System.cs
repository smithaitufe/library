using System.Xml.Serialization;

namespace Library.NCIPServer.Models
{
    [XmlRoot]
    public class FromSystemId
    {
        [XmlAttribute]
        public string Scheme { get; set; }
        [XmlText]
        public string Value { get; set; }
    }
    public class ToSystemId
    {
        [XmlAttribute]
        public string Scheme { get; set; }
        [XmlText]
        public string Value { get; set; }
    }
}