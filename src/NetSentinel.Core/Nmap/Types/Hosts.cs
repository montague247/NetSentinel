using System.Xml.Serialization;

namespace NetSentinel.Nmap.Types
{
    [XmlRoot("hosts")]
    public sealed class Hosts
    {
        [XmlAttribute("up")]
        public int Up { get; set; }

        [XmlAttribute("down")]
        public int Down { get; set; }

        [XmlAttribute("total")]
        public int Total { get; set; }
    }
}