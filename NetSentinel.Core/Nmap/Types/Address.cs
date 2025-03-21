using System.Xml.Serialization;

namespace NetSentinel.Nmap.Types
{
    [XmlRoot("address")]
    public sealed class Address
    {
        [XmlAttribute("addr")]
        public string? Addr { get; set; }

        [XmlAttribute("addrtype")]
        public string? AddrType { get; set; }

        [XmlAttribute("vendor")]
        public string? Vendor { get; set; }
    }
}
