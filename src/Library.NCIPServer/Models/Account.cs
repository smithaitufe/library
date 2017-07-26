using System.Xml.Serialization;

namespace Library.NCIPServer.Models
{
    public class LoanedItemsCount
    {
        [XmlElement]
        public int LoanedItemCountValue { get; set; }
        [XmlElement]
        public CirculationStatus CirculationStatus { get; set; }
        
    }

    public class CirculationStatus
    {
        [XmlAttribute]
        public string Scheme { get; set; }
        [XmlText]
        public string Value { get; set; }

    }
    public class CurrencyCode 
    {
        [XmlAttribute]
        public string Scheme { get; set; }
    }
    public class AccountBalance 
    {
        [XmlElement]
        public CurrencyCode CurrencyCode { get; set; }
        [XmlElement]
        public int MonetaryValue { get; set; }
    }

}