using System.Xml.Serialization;

namespace NetSentinel.Nmap.Types;

[XmlRoot("times")]
public sealed class Times
{
    [XmlAttribute("srtt")]
    public int Srtt { get; set; }

    [XmlAttribute("rttvar")]
    public int RttVar { get; set; }

    [XmlAttribute("to")]
    public int To { get; set; }
}
