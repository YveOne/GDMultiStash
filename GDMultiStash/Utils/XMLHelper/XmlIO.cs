using System;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace XMLHelper
{
    public static class XmlIO
    {

        public static T ReadXmlText<T>(string xmlText)
        {
            using (StringReader strReader = new StringReader(xmlText))
            {
                XmlReaderSettings settings = new XmlReaderSettings
                {
                    IgnoreWhitespace = false,
                    IgnoreComments = true
                };
                using (XmlReader xmlReader = XmlReader.Create(strReader, settings))
                {
                    return (T)new XmlSerializer(typeof(T), typeof(T).GetNestedTypes()).Deserialize(xmlReader);
                }
            }
        }

        public static void WriteXmlFile<T>(T obj, string filePath)
        {
            using (var sw = new Utf8StringWriter())
            {
                XmlWriterSettings settings = new XmlWriterSettings
                {
                    Encoding = Encoding.GetEncoding("ISO-8859-1"),
                    NewLineChars = Environment.NewLine,
                    ConformanceLevel = ConformanceLevel.Auto,
                    Indent = true,
                };
                using (XmlWriter writer = XmlWriter.Create(sw, settings))
                {
                    XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
                    namespaces.Add("", "");
                    XmlSerializer slz = new XmlSerializer(typeof(T), typeof(T).GetNestedTypes());
                    slz.Serialize(new XmlWriterEE(writer), obj, namespaces); // write closing tags
                    string xmlText = sw.ToString();
                    File.WriteAllText(filePath, xmlText, Encoding.UTF8);
                }
            }
        }

    }
}
