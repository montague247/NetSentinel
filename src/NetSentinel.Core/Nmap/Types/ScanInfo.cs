using System.Xml.Serialization;

namespace NetSentinel.Nmap.Types
{
    [XmlRoot("scaninfo")]
    public sealed class ScanInfo
    {
        [XmlAttribute("type")]
        public string? Type { get; set; }

        [XmlAttribute("scanflags")]
        public string? ScanFlags { get; set; }

        [XmlAttribute("protocol")]
        public string? Protocol { get; set; }

        [XmlAttribute("numservices")]
        public int NumServices { get; set; }

        [XmlAttribute("services")]
        public string? Services { get; set; }
    }
}
