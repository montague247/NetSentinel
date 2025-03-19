using System.Xml.Serialization;

namespace NetSentinel.Nmap.Types
{
    [XmlRoot("runstats")]
    public sealed class RunStats
    {
        [XmlAttribute("finished")]
        public Finished? Finished { get; set; }

        [XmlAttribute("hosts")]
        public Hosts? Hosts { get; set; }
    }
}
