using System.Text.Json.Serialization;
using Charon.System;

namespace NetSentinel.System
{
    public sealed class PackageUpdateConfiguration : IShellOptions
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool SudoAlternative { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool NoInstall { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? Packages { get; set; }
    }
}
