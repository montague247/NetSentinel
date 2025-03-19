using System.Xml.Serialization;
using NetSentinel.Discovery;
using NetSentinel.Nmap.Types;
using Serilog;

namespace NetSentinel.Nmap
{
    /// <summary>
    /// Discovery via nmap: https://www.device42.com/blog/2023/03/29/nmap-host-discovery-the-ultimate-guide/
    /// </summary>
    public static class NmapExecute
    {
        public static NmapRun? ArpPingScan(string ipRange, IDiscoveryOptions options)
        {
            return Execute(options, "-sn", "-PR", ipRange);
        }

        private static NmapRun? Execute(IDiscoveryOptions options, params string[] arguments)
        {
            var outputPath = GetTempFile(Path.GetFullPath("."));
            var list = new List<string>(arguments)
            {
                "-oX",
                outputPath
            };

            Shell.SudoExecute("nmap", list);

            var xml = File.ReadAllText(outputPath);

            if (options.KeepTempFiles)
                Log.Warning($"Nmap output file: {outputPath}");
            else
                File.Delete(outputPath);

            var xmlSerializer = new XmlSerializer(typeof(NmapRun));

            using var reader = new StringReader(xml);
            return (NmapRun?)xmlSerializer.Deserialize(reader);
        }

        private static string GetTempFile(string path)
        {
            var rnd = new Random();

            do
            {
                var name = $"nmap_{rnd.Next():0000000000}.xml.temp";
                var fileName = Path.Combine(path, name);

                if (!File.Exists(fileName))
                    return fileName;
            } while (true);
        }
    }
}
