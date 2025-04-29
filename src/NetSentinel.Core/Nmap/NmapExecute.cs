using Charon.System;
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
        public static NmapRun? ArpPingScan(string ipRange, IDiscoveryOptions options, IShellOptions shellOptions)
        {
            return Execute(options, shellOptions, "-sn", "-PR", ipRange);
        }

        private static NmapRun? Execute(IDiscoveryOptions options, IShellOptions shellOptions, params string[] arguments)
        {
            CheckInstall(shellOptions);

            var outputPath = GetTempFile(Path.GetFullPath("."));
            var list = new List<string>(arguments)
            {
                "--stats-every",
                "1m",
                "-oX",
                outputPath
            };

            Shell.SudoExecute("nmap", list, shellOptions);

            var xml = File.ReadAllText(outputPath);

            if (options.KeepTempFiles)
                Log.Warning($"Nmap output file: {outputPath}");
            else
                File.Delete(outputPath);

            return xml.FromXml<NmapRun>();
        }

        private static void CheckInstall(IShellOptions shellOptions)
        {
            if (shellOptions.NoInstall)
                return;

            Shell.CheckInstall("nmap", shellOptions);
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
