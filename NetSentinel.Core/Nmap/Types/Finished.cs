using System.Xml.Serialization;

namespace NetSentinel.Nmap.Types
{
    [XmlRoot("finished")]
    public sealed class Finished
    {
        [XmlAttribute("time")]
        public int Time { get; set; }

        [XmlAttribute("timestr")]
        public string? TimeStr { get; set; }

        [XmlAttribute("elapsed")]
        public decimal? Elapsed { get; set; }

        [XmlAttribute("summary")]
        public string? Summary { get; set; }

        [XmlAttribute("exit")]
        public string? Exit { get; set; }
    }
}
