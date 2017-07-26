using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Library.NCIPServer.Helpers
{
    public class XmlObjectConverter
    {
        public static Object GetObjectFromXML(string xml, Type objectType)
        {
            using(StringReader strReader = new StringReader(xml))
            {
                XmlSerializer serializer = null;
                XmlReader xmlReader = null;
                Object obj = null;
                serializer = new XmlSerializer(objectType);
                xmlReader = XmlReader.Create(strReader);
                obj = serializer.Deserialize(xmlReader);           
                return obj;
            }
        }

        public static string GetXMLFromObject(object o)
        {
            using (StringWriter sw = new StringWriter())
            {

                XmlWriter tw = null;
                string xml = null;
                var type = o.GetType();
                              
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.OmitXmlDeclaration = true;
                tw = XmlWriter.Create(sw, settings);
                XmlSerializerNamespaces names = new XmlSerializerNamespaces();
                names.Add("", "");

                XmlSerializer serializer = new XmlSerializer(type);  
                serializer.Serialize(tw, o,names);
                tw.Flush();
                sw.Flush();
                xml = sw.ToString();
                return XmlFormatter.FormatXml(xml);                
            }
        }
    }
}