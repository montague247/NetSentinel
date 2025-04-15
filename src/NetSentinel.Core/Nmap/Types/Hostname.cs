using System.Xml.Serialization;

namespace NetSentinel.Nmap.Types
{
    [XmlRoot("hostname")]
    public sealed class Hostname
    {
        [XmlAttribute("name")]
        public string? Name { get; set; }

        [XmlAttribute("type")]
        public string? Type { get; set; }
    }
}
