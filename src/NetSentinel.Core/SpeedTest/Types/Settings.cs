using System.Xml.Serialization;

namespace NetSentinel.SpeedTest.Types;

[XmlRoot("settings")]
public sealed class Settings
{
    public Settings()
    {
        Servers = [];
    }

    [XmlElement("client")]
    public Client? Client { get; set; }

    [XmlElement("times")]
    public Times? Times { get; set; }

    [XmlElement("download")]
    public Download? Download { get; set; }

    [XmlElement("upload")]
    public Upload? Upload { get; set; }

    [XmlElement("server-config")]
    public ServerConfig? ServerConfig { get; set; }

    public List<Server> Servers { get; set; }
}
