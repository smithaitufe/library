using System.Xml.Serialization;

namespace Library.NCIPServer.Models
{
    public class ItemOptionalFields 
    {
        [XmlElement]
        public BibliographicDescription BibliographicDescription { get; set; }
        [XmlElement]
        public string CirculationStatus { get; set; }
        [XmlElement]
        public int HoldQueueLength { get; set; }
        [XmlElement]
        public ItemUseRestrictionType ItemUseRestrictionType { get; set; }
        [XmlElement]
        public Location Location { get; set; }
        [XmlElement]
        public string ItemTransaction { get; set; }

    }
    public class ItemDescription
    {
        [XmlElement]
        public string CallNumber { get; set; }
        [XmlElement]
        public string CopyNumber { get; set; }
    }
    public class ItemUseRestrictionType
    {
        [XmlAttribute]
        public string Scheme { get; set; }
        [XmlText]
        public string Value { get; set; }
    }
    public class ItemId
    {
        [XmlElement]
        public AgencyId AgencyId { get; set; }
        [XmlElement]
        public ItemIdentifierType ItemIdentifierType { get; set; }
        [XmlElement]
        public string ItemIdentifierValue { get; set; }
    }

    public class ItemIdentifierType
    {
        [XmlAttribute]
        public string Scheme { get; set; }
        [XmlText]
        public string Value { get; set; }
    }
    public class ItemElementType {
        [XmlAttribute]
        public string Scheme { get; set; }
        [XmlText]
        public string Value { get; set; }
    }

}