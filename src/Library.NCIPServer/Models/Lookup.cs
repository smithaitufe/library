using System;
using System.Xml.Serialization;
using Library.Core.Models;

namespace Library.NCIPServer.Models
{
    [XmlRoot]
    public class LookupItem
    {

        [XmlElement]
        public InitiationHeader InitiationHeader { get; set; }
        [XmlElement]
        public ItemId ItemId { get; set; }
        [XmlElement]
        public int CurrentBorrowerDesired { get; set; }
        [XmlElement]
        public int CurrentRequestersDesired { get; set; }
        [XmlElement]
        public ItemElementType[] ItemElementType { get; set; }
    }

    [XmlRoot]
    public class LookupItemResponse
    {
        [XmlElement]
        public ResponseHeader ResponseHeader { get; set; }
        [XmlElement]
        public ItemId ItemId { get; set; }
        [XmlElement]
        public Nullable<DateTime> DateRecalled { get; set; }
        [XmlElement]
        public Nullable<DateTime> HoldPickupDate { get; set; }
        [XmlElement]
        public ItemOptionalFields ItemOptionalFields { get; set; }

    }
    public class LookupAgency
    {
        [XmlElement]
        public InitiationHeader InitiationHeader { get; set; }
        [XmlElement]
        public AgencyId AgencyId { get; set; }
        [XmlElement]
        public AgencyElementType AgencyElementType { get; set; }
    }

    public class LookupAgencyResponse
    {
        [XmlElement]
        public ResponseHeader ResponseHeader { get; set; }
        [XmlElement]
        public AgencyId AgencyId { get; set; }
        [XmlElement]
        public OrganizationNameInformation OrganizationNameInformation { get; set; }
    }
    [XmlRoot]
    public class LookupUser
    {
        [XmlElement]
        public InitiationHeader InitiationHeader { get; set; }
        [XmlElement]
        public string LoanedItemsDesired { get; set; }
        [XmlElement]
        public string RequestedItemsDesired { get; set; }
        [XmlElement]
        public UserId UserId { get; set; }
        [XmlElement]
        public UserElementType[] UserElementType { get; set; }
    }

    [XmlRoot]
    public class LookupUserResponse 
    {
        [XmlElement]
        public ResponseHeader ResponseHeader { get; set; }
        [XmlElement]
        public UserId UserId { get; set; }
        [XmlElement]
        public UserOptionalFields UserOptionalFields { get; set; }
        [XmlElement]
        public UserFiscalAccount UserFiscalAccount { get; set; }
    }
}
