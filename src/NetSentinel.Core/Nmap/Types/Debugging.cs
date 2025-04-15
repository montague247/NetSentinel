using System.Xml.Serialization;

namespace NetSentinel.Nmap.Types
{
    [XmlRoot("debugging")]
    public sealed class Debugging
    {
        [XmlAttribute("level")]
        public int Level { get; set; }
    }
}
