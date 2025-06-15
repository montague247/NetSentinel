using Charon.System;
using Serilog;

namespace NetSentinel.System;

public static class PackageUpdater
{
    private const string AptCommand = "apt-get";

    public static void Execute(IShellOptions options)
    {
        // Check if the system is running on a Debian-based distribution
        if (!Shell.IsDebianBased())
            return;

        Log.Information("Updating system packages...");

        // Check if the user has sudo privileges
        if (!Shell.HasSudoPrivileges())
        {
            Log.Error("This operation requires sudo privileges. Please run as a user with sudo access.");

            return;
        }

        var startedUtc = DateTime.UtcNow;

        Log.Information("Ensure that the necessary tools are installed");
        Shell.SudoExecute(AptCommand, ["update"], options);

        Log.Information("List upgradable packages");
        Shell.Execute(AptCommand, ["list", "--upgradable"]);

        Log.Information("Upgrade packages");
        Shell.SudoExecute(AptCommand, ["upgrade", "-y"], options);

        Log.Information("Perform a full distribution upgrade");
        Shell.SudoExecute(AptCommand, ["dist-upgrade", "-y"], options);

        Log.Information("Remove unused packages and clean up");
        Shell.SudoExecute(AptCommand, ["autoremove", "-y"], options);

        Log.Information("Clean up package cache");
        Shell.SudoExecute(AptCommand, ["autoclean"], options);

        Log.Information("Clean up any residual package files");
        Shell.SudoExecute(AptCommand, ["clean"], options);

        Log.Information("System packages updated successfully after {Duration}", DateTime.UtcNow - startedUtc);

        // Check if the system needs a reboot
        if (Shell.Execute("test", ["-f", "/var/run/reboot-required"], logFailed: false) == 0)
            Log.Warning("A reboot is required to complete the updates. Please reboot your system.");
        else
            Log.Information("No reboot is required after the updates.");
    }
}
