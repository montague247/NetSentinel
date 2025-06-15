using Charon.System;
using Serilog;

namespace NetSentinel.System;

public static class PackageUpdater
{
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
        Shell.SudoExecute(Shell.AptGetCommand, ["update"], options);

        Log.Information("List upgradable packages");
        Shell.BashExecute([Shell.AptGetCommand, "-s", "upgrade", "|", "grep", "'^Inst'"]);

        Log.Information("Upgrade packages");
        Shell.SudoExecute(Shell.AptGetCommand, ["upgrade", "-y"], options);

        Log.Information("Perform a full distribution upgrade");
        Shell.SudoExecute(Shell.AptGetCommand, ["dist-upgrade", "-y"], options);

        Log.Information("Remove unused packages and clean up");
        Shell.SudoExecute(Shell.AptGetCommand, ["autoremove", "-y"], options);

        Log.Information("Clean up package cache");
        Shell.SudoExecute(Shell.AptGetCommand, ["autoclean"], options);

        Log.Information("Clean up any residual package files");
        Shell.SudoExecute(Shell.AptGetCommand, ["clean"], options);

        Log.Information("System packages updated successfully after {Duration}", DateTime.UtcNow - startedUtc);

        // Check if the system needs a reboot
        if (Shell.Execute("test", ["-f", "/var/run/reboot-required"]) == 0)
            Log.Warning("A reboot is required to complete the updates. Please reboot your system.");
        else
            Log.Information("No reboot is required after the updates.");
    }
}
