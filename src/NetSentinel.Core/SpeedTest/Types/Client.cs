using System.Xml.Serialization;

namespace NetSentinel.SpeedTest.Types;

[XmlRoot("client")]
public sealed class Client
{
    private readonly Lazy<Coordinate> _geoCoordinate;

    public Client()
    {
        // note: geo coordinate will not be recalculated on Latitude or Longitude change
        _geoCoordinate = new Lazy<Coordinate>(() => new Coordinate(Latitude, Longitude));
    }

    [XmlAttribute("ip")]
    public string? Ip { get; set; }

    [XmlAttribute("lat")]
    public double Latitude { get; set; }

    [XmlAttribute("lon")]
    public double Longitude { get; set; }

    [XmlAttribute("isp")]
    public string? Isp { get; set; }

    [XmlAttribute("isprating")]
    public double IspRating { get; set; }

    [XmlAttribute("rating")]
    public double Rating { get; set; }

    [XmlAttribute("ispdlavg")]
    public int IspAvarageDownloadSpeed { get; set; }

    [XmlAttribute("ispulavg")]
    public int IspAvarageUploadSpeed { get; set; }

    public Coordinate GeoCoordinate
    {
        get { return _geoCoordinate.Value; }
    }
}
