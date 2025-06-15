using Charon.System;
using NetSentinel.Configuration;

namespace NetSentinel.System
{
    public sealed class PackageUpdateConfigurationHandler : IConfigurationHandler<PackageUpdateConfiguration>
    {
        public void Execute(PackageUpdateConfiguration configuration)
        {
            PackageUpdater.Execute(configuration);

            CheckInstall(configuration, configuration.Packages);
        }

        private static void CheckInstall(IShellOptions options, string[]? packages)
        {
            if (packages == null)
                return;

            foreach (var package in packages)
            {
                Shell.CheckInstall(options, package);
            }
        }
    }
}
