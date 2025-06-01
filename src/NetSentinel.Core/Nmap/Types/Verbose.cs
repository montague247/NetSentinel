using System.Xml.Serialization;

namespace NetSentinel.Nmap.Types;

[XmlRoot("verbose")]
public sealed class Verbose
{
    [XmlAttribute("level")]
    public int Level { get; set; }
}
