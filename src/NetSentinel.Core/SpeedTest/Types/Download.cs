using System.Xml.Serialization;

namespace NetSentinel.SpeedTest.Types;

[XmlRoot("download")]
public sealed class Download
{
    [XmlAttribute("testlength")]
    public int TestLength { get; set; }

    [XmlAttribute("initialtest")]
    public string? InitialTest { get; set; }

    [XmlAttribute("mintestsize")]
    public string? MinTestSize { get; set; }

    [XmlAttribute("threadsperurl")]
    public int ThreadsPerUrl { get; set; }
}
