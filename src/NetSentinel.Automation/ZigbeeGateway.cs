using System.Runtime.InteropServices;
using Charon;
using Charon.Security;
using Charon.System;
using Serilog;

namespace NetSentinel.Automation;

public static class ZigbeeGateway
{
    public static void EnsureRaspBeeIIService(IShellOptions shellOptions)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            throw new PlatformNotSupportedException("RaspBee II service is not supported on Windows.");
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            throw new PlatformNotSupportedException("RaspBee II service is not supported on macOS.");

        if (!shellOptions.NoInstall)
            Shell.CheckInstall(shellOptions, "libusb-1.0-0-dev", "libudev-dev", "deconz");

        Service.Start("deconz", shellOptions);
    }

    public static void EnsureZigbee2MQTTService(IShellOptions shellOptions)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            throw new PlatformNotSupportedException("Zigbee2MQTT service is not supported on Windows.");
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            throw new PlatformNotSupportedException("Zigbee2MQTT service is not supported on macOS.");

        if (!shellOptions.NoInstall)
        {
            Shell.EnsureNodeJS(shellOptions);
            Shell.CheckInstall(shellOptions, "git", "make", "g++", "libpcap-dev");

            EnsureZigbee2MQTTApp(shellOptions);
        }

        Service.Start("zigbee2mqtt", shellOptions);
    }

    public static void EnsureServices(IShellOptions shellOptions)
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            return;

        EnsureRaspBeeIIService(shellOptions);
        EnsureZigbee2MQTTService(shellOptions);
    }

    private static void EnsureZigbee2MQTTApp(IShellOptions shellOptions)
    {
        var appPath = "/opt/zigbee2mqtt";

        if (Directory.Exists(appPath))
            Shell.SudoExecute("git", "/opt", ["update", "https://github.com/Koenkk/zigbee2mqtt.git"], shellOptions);
        else
            Shell.SudoExecute("git", "/opt", ["clone", "https://github.com/Koenkk/zigbee2mqtt.git"], shellOptions);

        Shell.Execute("npm", appPath, ["ci"]);

        var configurationPath = Path.Combine(appPath, "data", "configuration.yaml");

        if (File.Exists(configurationPath))
            Log.Information("Zigbee2MQTT configuration already exists at {Path}", configurationPath);
        else
        {
            Log.Information("Creating Zigbee2MQTT configuration at {Path}", configurationPath);
            var defaultConfig = $@"mqtt:
  base_topic: zigbee2mqtt
  server: 'mqtt://localhost'
  user: '{SecurityExtensions.CreateSecurePassword(16)}'
  password: '{SecurityExtensions.CreateSecurePassword(32)}'

serial:
  port: '/dev/ttyAMA0'  # Port for RaspBee II
  adapter: 'deconz'  # Adapter for RaspBee II
frontend:
  port: 8080";

            File.WriteAllText(configurationPath, defaultConfig);

            Log.Information("Zigbee2MQTT configuration created successfully at {Path}", configurationPath);
        }
    }
}
