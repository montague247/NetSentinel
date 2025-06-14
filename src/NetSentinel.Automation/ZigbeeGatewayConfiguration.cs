using System.Text.Json.Serialization;
using Charon.System;

namespace NetSentinel.Automation;

public sealed class ZigbeeGatewayConfiguration : IShellOptions
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool SudoAlternative { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool NoInstall { get; set; }
}
