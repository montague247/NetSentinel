using System.Xml.Serialization;

namespace NetSentinel.SpeedTest.Types
{
    [XmlRoot("settings")]
    public sealed class ServersList
    {
        public ServersList()
        {
            Servers = [];
        }

        [XmlArray("servers")]
        [XmlArrayItem("server")]
        public List<Server> Servers { get; set; }

        public void CalculateDistances(Coordinate? clientCoordinate)
        {
            if (clientCoordinate == null)
                return;

            foreach (var server in Servers)
            {
                server.Distance = clientCoordinate.GetDistanceTo(server.GeoCoordinate);
            }
        }
    }
}
