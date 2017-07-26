namespace Library.NCIPServer.Helpers
{
    public class XmlFormatter
    {
        public static string FormatXml(string xml)
        {
            string result = @"<?xml version=""1.0"" encoding=""utf-8""?><!DOCTYPE NCIPMessage PUBLIC ""-//NISO//NCIP XML Schema Version2//EN"" ""http://sanelib.com/ncip/ncip_20.xsd""><NCIPMessage version=""2.0.0"">{0}</NCIPMessage>";
            return string.Format(result, xml);
        }
    }
}