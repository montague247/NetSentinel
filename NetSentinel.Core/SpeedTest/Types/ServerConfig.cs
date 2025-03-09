using System.Xml.Serialization;

namespace NetSentinel.SpeedTest.Types
{
    [XmlRoot("server-config")]
    public sealed class ServerConfig
    {
        [XmlAttribute("ignoreids")]
        public string? IgnoreIds { get; set; }
    }
}
