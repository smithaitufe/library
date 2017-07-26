using System;
using System.Xml.Serialization;

namespace Library.NCIPServer.Models
{
    public class UserId
    {
        [XmlElement]
        public AgencyId AgencyId { get; set; }
        [XmlElement]
        public UserIdentifierType UserIdentifierType { get; set; }
        [XmlElement]
        public string UserIdentifierValue { get; set; }
    }

    public class UserIdentifierType 
    {
        [XmlAttribute]
        public string Scheme { get; set; }
        [XmlText]
        public string Value { get; set; }
    }
    public class UserElementType 
    {
        [XmlAttribute]
        public string Scheme { get; set; }
    }
    public class PersonalNameInformation
    {
        [XmlElement]
        public string  UnstructuredPersonalUserName { get; set; }
    }
    public class NameInformation
    {
        [XmlElement]
        public PersonalNameInformation PersonalNameInformation { get; set; }
    }

    public class UnstructuredAddressType
    {
        [XmlAttribute]
        public string Scheme { get; set; }
        [XmlText]
        public string Value { get; set; }
    }
    public class UnstructuredAddress
    {
        [XmlElement]
        public UnstructuredAddressType UnstructuredAddressType { get; set; }
        [XmlElement]
        public string UnstructuredAddressData { get; set; }
    }
    public class PhysicalAddress 
    {
        [XmlElement]
        public UnstructuredAddress UnstructuredAddress { get; set; }
        [XmlElement]
        public string PhysicalAddressType { get; set; }
    }
    public class ElectronicAddress
    {
        [XmlElement]
        public string ElectronicAddressType { get; set; }
        [XmlElement]
        public string ElectronicAddressData { get; set; }
    }
    public class UserAddressInformation
    {
        public string UserAddressRoleType { get; set; }
        [XmlElement]
        public PhysicalAddress PhysicalAddress { get; set; }
        [XmlElement]
        public ElectronicAddress ElectronicAddress { get; set; }
    }

    public class UserPrivilege
    {
        [XmlElement]
        public string AgencyUserPrivilegeType { get; set; }
        [XmlElement]
        public DateTime ValidFromDate { get; set; }
        [XmlElement]
        public DateTime ValidToDate { get; set; }
    }
    public class Ext 
    {
        [XmlElement]
        public byte[] UserPhoto { get; set; }
        [XmlElement]
        public int Limit { get; set; }
    }

    public class UserOptionalFields
    {
        [XmlElement]
        public NameInformation NameInformation { get; set; }
        [XmlElement]
        public Nullable<DateTime> DateOfBirth { get; set; }
        [XmlElement]
        public UserAddressInformation[] UserAddressInformation { get; set; }
        [XmlElement]
        public UserPrivilege UserPrivilege { get; set; }
        [XmlElement]
        public Ext Ext { get; set; }
    }
    
    public class UserFiscalAccount 
    {
        [XmlElement]
        public AccountBalance AccountBalance { get; set; }
    }
    
}




