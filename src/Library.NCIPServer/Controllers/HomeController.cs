using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using Library.Core.Models;
using Library.NCIPServer.Models;
using Library.Repo;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IActionResult> Index()
        {
            var m = new MemoryStream();
            Request.Body.CopyTo(m);
            var contentLength = m.Length;
            var b = System.Text.Encoding.UTF8.GetString(m.ToArray());

            var document = new XmlDocument();
            document.LoadXml(b);
            var item = document.DocumentElement.ChildNodes[0];
            var xml = item.OuterXml;


            LookupItem lookupItem;
            LookupAgency lookupAgency;
            LookupUser lookupUser;
            CheckInItem checkinItem;
            CheckOutItem checkoutItem;



            switch (item.Name.ToLower())
            {
                case "lookupitem":
                    lookupItem = (LookupItem)GetObjectFromXML(xml, typeof(LookupItem));
                    //pick the agencyId and the itemId ItemIdentifierValue. The itemId is the most relevant information

                    xml = GetXMLFromObject(lookupItem);

                    var variant = await _context.Variants
                    .Include(v => v.Book)
                    .Where(e => e.Id == int.Parse(lookupItem.ItemId.ItemIdentifierValue))
                    .SingleOrDefaultAsync();
                    return Ok(document);

                case "lookupuser":
                    lookupUser = (LookupUser)GetObjectFromXML(xml, typeof(LookupUser));

                    return Ok(lookupUser);
                case "lookupagency":
                    lookupAgency = (LookupAgency)GetObjectFromXML(xml, typeof(LookupAgency));
                    return Ok(lookupAgency);
                case "checkinitem":
                    checkinItem = (CheckInItem)GetObjectFromXML(xml, typeof(CheckInItem));
                    return Ok(checkinItem);
                case "checkoutitem":
                    checkoutItem = (CheckOutItem)GetObjectFromXML(xml, typeof(CheckOutItem));
                    return Ok(checkoutItem);
                default:
                    break;
            }
            return Content(xml);
        }

        public static Object GetObjectFromXML(string xml, Type objectType)
        {
            StringReader strReader = null;
            XmlSerializer serializer = null;
            XmlTextReader xmlReader = null;
            Object obj = null;
            try
            {
                strReader = new StringReader(xml);
                serializer = new XmlSerializer(objectType);
                xmlReader = new XmlTextReader(strReader);
                obj = serializer.Deserialize(xmlReader);
            }
            catch (Exception ex)
            {
                //Handle Exception Code

            }
            finally
            {
                if (xmlReader != null)
                {
                    xmlReader.Close();
                }
                if (strReader != null)
                {
                    strReader.Close();
                }
            }
            return obj;
        }
        public static string GetXMLFromObject(object o)
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter tw = null;
            try
            {
                XmlSerializer serializer = new XmlSerializer(o.GetType());
                tw = new XmlTextWriter(sw);
                serializer.Serialize(tw, o);
            }
            catch (Exception ex)
            {
                //Handle Exception Code
            }
            finally
            {
                sw.Close();
                if (tw != null)
                {
                    tw.Close();
                }
            }
            return FormatXml(sw.ToString());
        }
        public static string GetXMLFromObject(object o, string rootName)
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter tw = null;
            try
            {
                XmlSerializer serializer = new XmlSerializer(o.GetType(), new XmlRootAttribute { ElementName = rootName });
                tw = new XmlTextWriter(sw);
                serializer.Serialize(tw, o);
            }
            catch (Exception ex)
            {
                //Handle Exception Code
            }
            finally
            {
                sw.Close();
                if (tw != null)
                {
                    tw.Close();
                }
            }
            return FormatXml(sw.ToString());
        }
        private static string FormatXml(string xml)
        {
            string result = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
            result += "<NCIPMessage version=\"2.0.0\">";
            result += xml;
            result += "</NCIPMessage>";
            return result;
        }

        private void XpathFinder()
        {
            var path = @"
                <Cell>          
                    <CellContent>
                        <Para>                               
                            <ParaLine>                      
                                <String>ABCabcABC abcABC abc ABCABCABC.</string> 
                            </ParaLine>                      
                        </Para>     
                    </CellContent>
                </Cell>
            ";

            XPathNavigator nav;
            XPathDocument docNav;
            string xPath;

            docNav = new XPathDocument("c:\\books.xml");
            docNav = new XPathDocument(path);
            nav = docNav.CreateNavigator();
            xPath = "/Cell/CellContent/Para/ParaLine/String/text()";

            string value = nav.SelectSingleNode(xPath).Value;
        }
    }
}