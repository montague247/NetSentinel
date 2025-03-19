using System.Xml.Serialization;

namespace NetSentinel.Nmap.Types
{
    [XmlRoot("host")]
    public sealed class Host
    {
        [XmlAttribute("starttime")]
        public int StartTime { get; set; }

        [XmlAttribute("endtime")]
        public int EndTime { get; set; }
    }
}
