using System.Xml.Serialization;

namespace NetSentinel
{
    public static class SerializationExtensions
    {
        public static T? FromXml<T>(this string xml)
        {
            var serializer = new XmlSerializer(typeof(T));

            using var reader = new StringReader(xml);

            return (T?)serializer.Deserialize(reader);
        }
    }
}