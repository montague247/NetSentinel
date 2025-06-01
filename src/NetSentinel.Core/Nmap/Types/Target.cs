using System.Xml.Serialization;

namespace NetSentinel.Nmap.Types;

[XmlRoot("target")]
public sealed class Target
{
    [XmlAttribute("specification")]
    public string? Specification { get; set; }

    [XmlAttribute("status")]
    public string? Status { get; set; }

    [XmlAttribute("reason")]
    public string? Reason { get; set; }
}
