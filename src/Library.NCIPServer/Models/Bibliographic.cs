using System.Xml.Serialization;

namespace Library.NCIPServer.Models
{
    public class BibliographicDescription 
    {
        [XmlElement]
        public string Author { get; set; }
        [XmlElement]
        public BibliographicItemId BibliographicItemId { get; set; }
        [XmlElement]
        public string Edition { get; set; }
        [XmlElement]
        public string Publisher { get; set; }
        [XmlElement]
        public string  Title { get; set; }
        [XmlElement]
        public BibliographicLevel BibliographicLevel { get; set; }
        [XmlElement]
        public string MediumType { get; set; }
        

    }
    public class BibliographicItemId
    {
        [XmlElement]
        public string BibliographicItemIdentifier { get; set; }
        [XmlElement]
        public BibliographicItemIdentifierCode BibliographicItemIdentifierCode { get; set; }

    }
    public class BibliographicItemIdentifierCode 
    {
        [XmlAttribute]
        public string Scheme { get; set; }
    }
    public class BibliographicLevel
    {
        [XmlAttribute]
        public string Scheme { get; set; }
        [XmlText]
        public string Value { get; set; }
    }
}