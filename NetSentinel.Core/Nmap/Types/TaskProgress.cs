using System.Xml.Serialization;

namespace NetSentinel.Nmap.Types
{
    [XmlRoot("taskprogress")]
    public sealed class TaskProgress
    {
        [XmlAttribute("task")]
        public string? Task { get; set; }

        [XmlAttribute("time")]
        public int Time { get; set; }

        [XmlAttribute("percent")]
        public decimal? Percent { get; set; }

        [XmlAttribute("remaining")]
        public int? Remaining { get; set; }

        [XmlAttribute("etc")]
        public int? Etc { get; set; }
    }
}
