using NetSentinel.Configuration;

namespace NetSentinel.Core.Tests.Configuration;

[Configuration("Sample")]
public sealed class SampleConfiguration
{
    public int? IntValue { get; set; }

    public string? StringValue { get; set; }
}
