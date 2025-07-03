using Charon.System;
using NetSentinel.Configuration;

namespace NetSentinel.System
{
    public sealed class PackageUpdateConfigurationHandler : ConfigurationHandlerBase<PackageUpdateConfiguration>
    {
        public override void Execute(PackageUpdateConfiguration configuration, CancellationToken cancellationToken)
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
