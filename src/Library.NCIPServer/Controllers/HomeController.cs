using Library.Core.Models;
using Library.NCIPServer.Models;
using Library.Repo;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Text.RegularExpressions;
using Library.NCIPServer.Constants;
using System.Collections.Generic;
using Library.NCIPServer.Helpers;
using Newtonsoft.Json;


namespace Library.NCIPServer.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/xml")]
    public class HomeController : Controller
    {
        LibraryDbContext _context;
        UserManager<User> _userManager;
        RoleManager<Role> _roleManager;

        public HomeController(LibraryDbContext context, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        [HttpPost]
        public async Task<IActionResult> Index()
        {

            var xml = await ReadXmlFromRequestAsync(Request.Body);
            var filter = @"<!DOCTYPE.+?>";
            xml = Regex.Replace(xml, filter, "");
            var document = new XmlDocument();
            document.LoadXml(xml);

            var request = document.DocumentElement.ChildNodes[0];
            var requestType = request.Name.ToUpper();
            var requestBody = request.OuterXml;
            var nav = document.CreateNavigator();
            var path = string.Empty;

            switch (requestType)
            {
                case RequestConstants.LOOKUPITEM:
                    path = "/NCIPMessage/LookupItem/ItemId/ItemIdentifierValue/text()";
                    var itemIdentifierValue = nav.SelectSingleNode(path).Value;

                    var item = await _context.VariantCopies
                    .Include(variantCopy => variantCopy.Variant).ThenInclude(variant => variant.Book).ThenInclude(book => book.BookAuthors).ThenInclude(bookAuthors => bookAuthors.Author)
                    .Include(variantCopy => variantCopy.Variant).ThenInclude(variant => variant.Book).ThenInclude(book => book.Publisher)
                    .Include(variantCopy => variantCopy.Variant).ThenInclude(variant => variant.Format)
                    .Include(variantCopy => variantCopy.Variant).ThenInclude(variant => variant.VariantCopies)
                    .Include(variantCopy => variantCopy.Availability)
                    .Include(variantCopy => variantCopy.Shelf)
                    .Include(variantCopy => variantCopy.Location)
                    .Where(variantCopy => variantCopy.SerialNo.ToLower().Equals(itemIdentifierValue.ToLower()))
                    .SingleOrDefaultAsync();

                    var response = new LookupItemResponse
                    {
                        ItemOptionalFields = new ItemOptionalFields
                        {
                            BibliographicDescription = new BibliographicDescription
                            {
                                Author = String.Join(", ", item.Variant.Book.BookAuthors.Select(ba => $"{ba.Author.LastName}, {ba.Author.FirstName}")),
                                Edition = item.Variant.Edition,
                                Publisher = item.Variant.Book.Publisher.Name,
                                Title = item.Variant.Book.Title,
                                BibliographicLevel = new BibliographicLevel
                                {
                                    Scheme = "",
                                    Value = item.Variant.Book.Series ? "Serial" : "Monograph"
                                },
                                MediumType = item.Variant.Format.Name,
                            },
                            CirculationStatus = item.Availability.Name,
                            ItemDescription = new ItemDescription
                            {
                                CallNumber = item.Variant.CallNumber ?? "N/A",
                                CopyNumber = (item.Variant.VariantCopies.OrderBy(vc => vc.InsertedAt).ToList().FindIndex(vc => vc.Id == item.Id) + 1).ToString()
                            },
                            HoldQueueLength = 0,
                        }

                    };
                    xml = XmlObjectConverter.GetXMLFromObject(response);
                    return Ok(xml);

                case RequestConstants.LOOKUPUSER:
                
                    var member = (LookupUser)XmlObjectConverter.GetObjectFromXML(requestBody, typeof(LookupUser));
                    var userId = member.UserId.UserIdentifierValue.ToLower();

                    var user = await _context.Users
                    .Include(u=>u.Roles)
                    .Include(u => u.UserAddresses)
                    .ThenInclude(ua => ua.Address)
                    .Where(u => u.LibraryNo.ToLower().Equals(userId) || u.UserName.ToLower().Equals(userId))
                    .SingleOrDefaultAsync();

                    var roles = await _userManager.GetRolesAsync(user);

                    var userAddress = user.UserAddresses.Where(ua => ua.Primary).FirstOrDefault();
                    PhysicalAddress physicalAddress;
                    if (userAddress != null)
                    {
                        physicalAddress = new PhysicalAddress
                        {
                            UnstructuredAddress = new UnstructuredAddress
                            {
                                UnstructuredAddressType = new UnstructuredAddressType { Value = null },
                                UnstructuredAddressData = $"{userAddress.Address.Line}, {userAddress.Address.City}"
                            }
                        };
                    }
                    else
                    {
                        physicalAddress = new PhysicalAddress();
                    }

                    var lookupUserResponse = new LookupUserResponse
                    {
                        
                        ResponseHeader = new ResponseHeader { },
                        UserId = member.UserId,
                        UserOptionalFields = new UserOptionalFields
                        {
                            NameInformation = new NameInformation
                            {
                                PersonalNameInformation = new PersonalNameInformation
                                {
                                    UnstructuredPersonalUserName = user.FullName
                                }
                            },
                            DateOfBirth = user.BirthDate,
                            UserAddressInformation = new UserAddressInformation[]{
                                new UserAddressInformation {
                                    UserAddressRoleType = (userAddress != null ? (userAddress.Primary && userAddress.Mailing ? "Mailing Address" : null) : null),
                                    PhysicalAddress = physicalAddress
                                },
                                new UserAddressInformation {
                                    UserAddressRoleType = "Phone",
                                    ElectronicAddress = new ElectronicAddress {
                                        ElectronicAddressType = "Phone",
                                        ElectronicAddressData = user.PhoneNumber
                                    }
                                },
                                new UserAddressInformation {
                                    UserAddressRoleType = "Email",
                                    ElectronicAddress = new ElectronicAddress {
                                        ElectronicAddressType = "Email",
                                        ElectronicAddressData = user.Email
                                    }
                                }
                            },
                            UserPrivilege = new UserPrivilege {
                                AgencyUserPrivilegeType = roles.Aggregate((p,c)=>string.Join(",", c)),
                                ValidFromDate = user.InsertedAt,
                                ValidToDate = DateTime.Now
                            }
                        }
                    };
                    xml = XmlObjectConverter.GetXMLFromObject(lookupUserResponse);
                    return Ok(xml);
                case RequestConstants.LOOKUPAGENCY:
                    var agency = (LookupAgency)XmlObjectConverter.GetObjectFromXML(requestBody, typeof(LookupAgency));
                    var agencyResponse = new LookupAgencyResponse {
                        ResponseHeader = new ResponseHeader(),
                        AgencyId = new AgencyId{
                            Scheme = "",
                            Value = ""
                        },
                        OrganizationNameInformation = new OrganizationNameInformation {
                            OrganizationName = "Delta State eLibrary",
                            OrganizationNameType = "BranchName"
                        }
                    };
                    xml = XmlObjectConverter.GetXMLFromObject(agencyResponse);
                    return Ok(xml);
                case RequestConstants.CHECKINITEM:
                    var checkin = (CheckInItem)XmlObjectConverter.GetObjectFromXML(requestBody, typeof(CheckInItem));

                    var result = await _context.CheckOuts
                    .Include(x=>x.CheckOutStates).ThenInclude(x=>x.Status)
                    .Include(x=>x.Patron)
                    .OrderByDescending(x=>x.Id)
                    .Where(x=>x.VariantCopy.SerialNo.ToLower().Equals(checkin.ItemId.ItemIdentifierValue))                    
                    .SingleOrDefaultAsync();

                    DateTimeOffset returnDate = DateTimeOffset.Now;


                    var checkinResponse = new CheckInItemResponse{};

                    return Ok(xml);
                case RequestConstants.CHECKOUTITEM:
                    break;
                default:
                    return Content("No matching request found. Try again");
            }

            return Content("");
        }
        private async Task<string> ReadXmlFromRequestAsync(Stream request)
        {
            var m = new MemoryStream();
            await request.CopyToAsync(m);
            var contentLength = m.Length;
            return System.Text.Encoding.UTF8.GetString(m.ToArray());
        }
        private object LoadXml(string xml)
        {
            string filter = @"<!DOCTYPE.+?>";
            xml = Regex.Replace(xml, filter, "");

            var document = new XmlDocument();
            document.LoadXml(xml);
            var request = document.DocumentElement.ChildNodes[0];
            var requestType = request.Name.ToUpper();
            object result = new
            {
                RequestType = requestType,
                RequestBody = request.InnerXml,
                Nav = document.CreateNavigator()
            };
            return result;
        }
    }
}