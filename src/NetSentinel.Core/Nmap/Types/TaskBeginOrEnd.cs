using System.Xml.Serialization;

namespace NetSentinel.Nmap.Types;

[XmlRoot("taskbeginorend")]
public sealed class TaskBeginOrEnd
{
    [XmlAttribute("task")]
    public string? Task { get; set; }

    [XmlAttribute("time")]
    public int Time { get; set; }

    [XmlAttribute("extrainfo")]
    public string? ExtraInfo { get; set; }
}
