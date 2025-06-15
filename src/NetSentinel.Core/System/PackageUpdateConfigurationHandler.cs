using Charon.System;
using NetSentinel.Configuration;

namespace NetSentinel.System
{
    public sealed class PackageUpdateConfigurationHandler : IConfigurationHandler<PackageUpdateConfiguration>
    {
        public void Execute(PackageUpdateConfiguration configuration)
        {
            if (configuration.Packages != null && configuration.Packages.Length > 0)
            {
                foreach (var package in configuration.Packages)
                {
                    Shell.CheckInstall(configuration, package);
                }
            }

            PackageUpdater.Execute(configuration);
        }
    }
}
