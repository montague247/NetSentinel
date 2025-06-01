using System.Xml.Serialization;

namespace NetSentinel.Nmap.Types;

[XmlRoot("host")]
public sealed class Host
{
    [XmlAttribute("starttime")]
    public int StartTime { get; set; }

    [XmlAttribute("endtime")]
    public int EndTime { get; set; }

    [XmlAttribute("timedout")]
    public bool TimedOut { get; set; }

    [XmlAttribute("comment")]
    public string? Comment { get; set; }

    [XmlElement("status")]
    public Status? Status { get; set; }

    [XmlElement("address")]
    public Address[]? Address { get; set; }

    [XmlArray("hostnames")]
    [XmlArrayItem("hostname")]
    public Hostname[]? Hostnames { get; set; }

    [XmlElement("times")]
    public Times? Times { get; set; }
}
