using System.Xml.Serialization;

namespace NetSentinel.Nmap.Types;

[XmlRoot("status")]
public sealed class Status
{
    [XmlAttribute("state")]
    public string? State { get; set; }

    [XmlAttribute("reason")]
    public string? Reason { get; set; }

    [XmlAttribute("reason_ttl")]
    public int ReasonTtl { get; set; }
}
