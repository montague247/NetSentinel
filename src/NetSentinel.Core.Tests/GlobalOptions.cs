namespace NetSentinel.Core.Tests;

public sealed class GlobalOptions : IGlobalOptions
{
    public bool SudoAlternative { get; set; }

    public bool NoInstall { get; set; } = true;

    public static GlobalOptions Instance { get; } = new();
}
