using System;
using System.Xml.Serialization;

namespace Library.NCIPServer.Models
{
    [XmlRoot]
    public class CheckInItem
    {
        [XmlElement]
        public InitiationHeader InitiationHeader { get; set; }
        [XmlElement]
        public ItemId ItemId { get; set; }
        [XmlElement]
        public DateTime ReturnedDate { get; set; }
    }
    [XmlRoot]
    public class CheckInItemResponse
    {
        [XmlElement]
        public ResponseHeader ResponseHeader { get; set; }
        [XmlElement]
        public ItemId ItemId { get; set; }
        [XmlElement]
        public UserId UserId { get; set; }
    }
    [XmlRoot]
    public class CheckOutItem
    {
        [XmlElement]
        public InitiationHeader InitiationHeader { get; set; } 
        [XmlElement]       
        public ItemId ItemId { get; set; }
        [XmlElement]
        public UserId UserId { get; set; }
    }
    public class CheckOutItemResponse 
    {
        [XmlElement]
        public ResponseHeader ResponseHeader { get; set; }
        [XmlElement]
        public ItemId ItemId { get; set; }
        [XmlElement]
        public UserId UserId { get; set; }
        [XmlElement]
        public DateTime DateDue { get; set; }
    }
}