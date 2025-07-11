using System.Runtime.InteropServices;
using Charon;
using Charon.System;
using Serilog;

namespace NetSentinel.Automation;

public static class ZigbeeGateway
{
    private const string AppPath = "/opt/zigbee2mqtt";

    public static void EnsureRaspBeeIIService(IShellOptions shellOptions, CancellationToken cancellationToken)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            throw new PlatformNotSupportedException("RaspBee II service is not supported on Windows.");
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            throw new PlatformNotSupportedException("RaspBee II service is not supported on macOS.");

        if (!shellOptions.NoInstall)
        {
            Log.Warning("Install RTC manually: https://phoscon.de/en/raspbee2/install#raspbian");

            Shell.BashExecute("wget -O - http://phoscon.de/apt/deconz.pub.key | gpg --dearmor | sudo tee /etc/apt/trusted.gpg.d/deconz.gpg > /dev/null");
            var parts = Shell.GetOutput("lsb_release", ["-cs"]).GetAwaiter().GetResult()?.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

            if (parts == null || parts.Length == 0)
                throw new InvalidOperationException("Failed to determine distribution codename.");

            if (cancellationToken.IsCancellationRequested)
                return;

            var codename = parts[^1];
            var repoLine = $"deb http://phoscon.de/apt/deconz {codename} main";

            Shell.BashExecute($"echo \"{repoLine}\" | sudo tee /etc/apt/sources.list.d/deconz.list");

            if (cancellationToken.IsCancellationRequested)
                return;

            Shell.CheckInstall(shellOptions, "libusb-1.0-0-dev", "libudev-dev", "deconz");
        }

        if (!cancellationToken.IsCancellationRequested)
            Charon.System.Service.Start("deconz", shellOptions);
    }

    public static void EnsureZigbee2MQTTService(IShellOptions shellOptions, CancellationToken cancellationToken)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            throw new PlatformNotSupportedException("Zigbee2MQTT service is not supported on Windows.");
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            throw new PlatformNotSupportedException("Zigbee2MQTT service is not supported on macOS.");

        if (!shellOptions.NoInstall)
        {
            Shell.EnsureNodeJS(shellOptions);

            if (cancellationToken.IsCancellationRequested)
                return;

            Shell.CheckInstall(shellOptions, "nodejs", "git", "make", "g++", "gcc", "libsystemd-dev");

            if (cancellationToken.IsCancellationRequested)
                return;

            Shell.SudoExecute("corepack", ["enable"], shellOptions);

            EnsureZigbee2MQTTApp(shellOptions, cancellationToken);
            CreateZigbee2MQTTService(shellOptions, cancellationToken);
        }

        if (!cancellationToken.IsCancellationRequested)
            Charon.System.Service.Start("zigbee2mqtt", shellOptions);
    }

    public static void EnsureServices(IShellOptions shellOptions, CancellationToken cancellationToken)
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            return;

        EnsureRaspBeeIIService(shellOptions, cancellationToken);
        EnsureZigbee2MQTTService(shellOptions, cancellationToken);
    }

    private static void EnsureZigbee2MQTTApp(IShellOptions shellOptions, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
            return;

        Log.Information("Ensuring Zigbee2MQTT application at {Path}", AppPath);

        if (Directory.Exists(AppPath))
            Shell.SudoExecute("git", AppPath, ["pull", "https://github.com/Koenkk/zigbee2mqtt.git"], shellOptions);
        else
        {
            Shell.SudoExecute("mkdir", [AppPath], shellOptions);
            Shell.SudoExecute("chown", ["-R", $"{Environment.UserName}:", AppPath], shellOptions);
            Shell.Execute("git", "/opt", ["clone", "--depth", "1", "https://github.com/Koenkk/zigbee2mqtt.git"]);
        }

        Log.Information("Install Zigbee2MQTT application");

        Shell.Execute("pnpm", AppPath, ["install", "--frozen-lockfile"], environmentVariables: new Dictionary<string, string>
        {
            ["CI"] = "true"
        });

        var tempConfigurationPath = Path.GetFullPath("Zigbee2MQTT-configuration.yaml");
        var configurationPath = Path.Combine(AppPath, "data", "configuration.yaml");

        if (File.Exists(tempConfigurationPath))
            Log.Information("Zigbee2MQTT configuration already exists at {Path}", configurationPath);
        else
        {
            Log.Information("Creating Zigbee2MQTT configuration at {Path}", tempConfigurationPath);

            var defaultConfig = $@"# Indicates the configuration version (used by configuration migrations)
version: 4

# Home Assistant integration (MQTT discovery)
homeassistant:
  enabled: true

# Enable the frontend, runs on port 8080 by default
frontend:
  enabled: true
  port: 8080
  url: 'http://{Environment.MachineName}.local:8080'

# MQTT settings
mqtt:
  # MQTT base topic for zigbee2mqtt MQTT messages
  base_topic: zigbee2mqtt
  # MQTT server URL
  server: mqtt://localhost:1883
  # MQTT server authentication
  user: '{SecurityExtensions.CreateSecurePassword(16)}'
  password: '{SecurityExtensions.CreateSecurePassword(32)}'

serial:
  # Serial port for the Zigbee adapter
  # For RaspBee II, this is typically /dev/ttyAMA0
  port: /dev/ttyAMA0
  # Serial adapter type
  # For RaspBee II, use 'deconz'
  adapter: deconz

# Periodically check whether devices are online/offline
availability:
  enabled: true

# Advanced settings
advanced:
  # channel: 11
  # Let Zigbee2MQTT generate a network key on first start
  network_key: GENERATE
  # Let Zigbee2MQTT generate a pan_id on first start
  pan_id: GENERATE
  # Let Zigbee2MQTT generate a ext_pan_id on first start
  ext_pan_id: GENERATE";

            File.WriteAllText(tempConfigurationPath, defaultConfig);

            Shell.SudoExecute("cp", [tempConfigurationPath, configurationPath], shellOptions);

            Log.Information("Zigbee2MQTT configuration created successfully at {Path}", configurationPath);
        }
    }

    private static void CreateZigbee2MQTTService(IShellOptions shellOptions, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
            return;

        var tempPath = Path.GetFullPath("zigbee2mqtt.service");

        File.WriteAllText(tempPath, $@"[Unit]
Description=zigbee2mqtt
After=network.target

[Service]
Environment=NODE_ENV=production
Type=notify
ExecStart=/usr/bin/node index.js
WorkingDirectory={AppPath}
StandardOutput=inherit
# Or use StandardOutput=null if you don't want Zigbee2MQTT messages filling syslog, for more options see systemd.exec(5)
StandardError=inherit
WatchdogSec=10s
Restart=always
RestartSec=10s
User=pi

[Install]
WantedBy=multi-user.target");

        var path = Path.Combine("/etc", "systemd", "system", "zigbee2mqtt.service");

        Shell.SudoExecute("cp", [tempPath, path], shellOptions);
    }
}
