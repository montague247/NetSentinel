using System.Xml.Serialization;

namespace NetSentinel.SpeedTest.Types
{
    [XmlRoot("server")]
    public sealed class Server
    {
        private readonly Lazy<Coordinate> _geoCoordinate;

        public Server()
        {
            // note: geo coordinate will not be recalculated on Latitude or Longitude change
            _geoCoordinate = new Lazy<Coordinate>(() => new Coordinate(Latitude, Longitude));
        }

        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlAttribute("name")]
        public string? Name { get; set; }

        [XmlAttribute("country")]
        public string? Country { get; set; }

        [XmlAttribute("sponsor")]
        public string? Sponsor { get; set; }

        [XmlAttribute("host")]
        public string? Host { get; set; }

        [XmlAttribute("url")]
        public string? Url { get; set; }

        [XmlAttribute("lat")]
        public double Latitude { get; set; }

        [XmlAttribute("lon")]
        public double Longitude { get; set; }

        public double Distance { get; set; }

        public int Latency { get; set; }

        public Coordinate GeoCoordinate
        {
            get { return _geoCoordinate.Value; }
        }
    }
}
