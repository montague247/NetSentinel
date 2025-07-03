using NetSentinel.Argument;

namespace NetSentinel.System;

[ArgumentHandler("--package-update", "Updates system packages and dependencies")]
public sealed class PackageUpdateArgumentHandler : ArgumentHandlerBase
{
    [ArgumentHandlerProperty("--packages", "Comma separated list of specific packages to install or update")]
    public List<string>? Packages { get; set; }

    public override void Execute(IGlobalOptions options)
    {
        var configuration = new PackageUpdateConfiguration
        {
            Packages = Packages?.ToArray(),
            NoInstall = options.NoInstall,
            SudoAlternative = options.SudoAlternative
        };

        new PackageUpdateConfigurationHandler().Execute(configuration, default);
    }

    protected override bool Process(string argument, string[] arguments, ref int index)
    {
        switch (argument)
        {
            case "--packages":
                Packages ??= [];
                Packages.AddRange(arguments[++index].Split(',').Select(p => p.Trim()));
                return true;
        }

        return false;
    }
}
