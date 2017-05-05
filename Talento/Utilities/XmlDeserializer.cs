using System;
using System.Web;
using System.Web.Hosting;
using System.Xml;
using System.Xml.Serialization;
using Talento.Entities;

namespace Talento.Tests.Utilities
{
    public static class XmlDeserializer
    {
        public static ApplicationSettingXml DeserealizeXml(string xmlSource)
        {
            //cannot deserialize Identity, create a POCO
            //string path = @"C:\Users\1377753\Source\Talento_\Talento\Talento\App_Data\" + xmlSource;

            string path = "../../Resources/" + xmlSource;

            XmlSerializer serializedObject = new XmlSerializer(typeof(ApplicationSettingXml));
            ApplicationSettingXml appSettings;
            using (XmlReader reader = XmlReader.Create(path))
            {
                appSettings = (ApplicationSettingXml)serializedObject.Deserialize(reader);
            }
            return appSettings;
        }
    }
}
