using System.Xml.Serialization;

namespace NetSentinel.Nmap.Types;

[XmlRoot("runstats")]
public sealed class RunStats
{
    [XmlElement("finished")]
    public Finished? Finished { get; set; }

    [XmlElement("hosts")]
    public Hosts? Hosts { get; set; }
}
