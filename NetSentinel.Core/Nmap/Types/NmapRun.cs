using System.Xml.Serialization;

namespace NetSentinel.Nmap.Types
{
    /// <summary>
    /// DTD: https://github.com/nmap/nmap/blob/master/docs/nmap.dtd
    /// </summary>
    [XmlRoot("nmaprun")]
    public sealed class NmapRun
    {
        [XmlAttribute("scanner")]
        public string? Scanner { get; set; }

        [XmlAttribute("args")]
        public string? Args { get; set; }

        [XmlAttribute("start")]
        public int Start { get; set; }

        [XmlAttribute("startstr")]
        public string? StartStr { get; set; }

        [XmlAttribute("version")]
        public string? Version { get; set; }

        [XmlAttribute("profile_name")]
        public string? ProfileName { get; set; }

        [XmlAttribute("xmloutputversion")]
        public string? XmlOutputVersion { get; set; }

        [XmlElement("scaninfo")]
        public ScanInfo? ScanInfo { get; set; }

        [XmlElement("verbose")]
        public Verbose? Verbose { get; set; }

        [XmlElement("debugging")]
        public Debugging? Debugging { get; set; }

        [XmlElement("target")]
        public Target? Target { get; set; }

        [XmlElement("taskbegin")]
        public TaskBeginOrEnd? TaskBegin { get; set; }

        [XmlElement("taskprogress")]
        public TaskProgress? TaskProgress { get; set; }

        [XmlElement("taskend")]
        public TaskBeginOrEnd? TaskEnd { get; set; }

        [XmlElement("host")]
        public Host[]? Host { get; set; }

        [XmlElement("runstats")]
        public RunStats? RunStats { get; set; }
    }
}
