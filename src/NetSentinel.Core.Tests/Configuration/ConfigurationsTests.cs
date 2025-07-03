using NetSentinel.Configuration;

namespace NetSentinel.Core.Tests.Configuration;

public sealed class ConfigurationsTests
{
    [Fact]
    public void LoadNotExisting()
    {
        var path = Path.Combine($"{nameof(ConfigurationsTests)}-{nameof(LoadNotExisting)}.json");
        var configurations = Configurations.Load(path, default);

        Assert.NotNull(configurations);
        Assert.NotNull(configurations.Entries);
        Assert.NotNull(configurations.Types);
        Assert.Equal(2, configurations.Types.Count);
        Assert.Contains("Sample", configurations.Types);

        var entry = configurations.GetEntry("Sample");
        Assert.Null(entry);
    }

    [Fact]
    public void LoadExisting()
    {
        var path = Path.Combine($"{nameof(ConfigurationsTests)}-{nameof(LoadExisting)}.json");

        var configurations = Configurations.Load(path, default);
        configurations.SetEntry("a", new());
        configurations.SetEntry("b", new());
        configurations.SetEntry("c", new());
        configurations.Save(default);

        configurations = Configurations.Load(path, default);

        Assert.NotNull(configurations);
        Assert.NotNull(configurations.Entries);
        Assert.Equal(3, configurations.Entries.Count);
    }

    [Fact]
    public void Sample()
    {
        var path = Path.GetFullPath(Path.Combine($"{nameof(ConfigurationsTests)}-{nameof(Sample)}.json"));
        var configurations = Configurations.Load(path, default);

        var entry = new ConfigurationEntry
        {
            Scheduling = new()
        };
        entry.SetConfiguration(new SampleConfiguration
        {
            IntValue = 42,
            StringValue = "Hello, World!"
        });
        configurations.SetEntry(nameof(Sample), entry);
        configurations.Save(default);
    }

    [Fact]
    public void LoadSample()
    {
        Sample();

        var path = Path.GetFullPath(Path.Combine($"{nameof(ConfigurationsTests)}-{nameof(Sample)}.json"));
        var configurations = Configurations.Load(path, default);

        Assert.NotNull(configurations);
        Assert.NotNull(configurations.Entries);
        Assert.NotEmpty(configurations.Entries);
        Assert.Contains(nameof(Sample), configurations.Entries);

        var entry = configurations.GetEntry(nameof(Sample));
        Assert.NotNull(entry);
        Assert.Equal(nameof(Sample), entry.Name);
        Assert.Equal("Sample", entry.Type);
        Assert.NotNull(entry.Scheduling);

        var configuration = entry.GetConfiguration<SampleConfiguration>();
        Assert.NotNull(configuration);
        Assert.Equal(42, configuration.IntValue);
        Assert.Equal("Hello, World!", configuration.StringValue);

        configurations.SetEntry("Second sample", new ConfigurationEntry().SetConfiguration(new SampleConfiguration
        {
            IntValue = 84,
            StringValue = "Goodbye, World!"
        }));
        configurations.Save(default);

        var entries = configurations.GetEntries<SampleConfiguration>();
        Assert.NotNull(entries);
        Assert.Equal(2, entries.Length);
        Assert.Contains(entries, e => e.Name == "Second sample");
        Assert.Contains(entries, e => e.Name == nameof(Sample));
    }

    [Fact]
    public void Execute()
    {
        var configurations = new Configurations();
        Configurations.FillTypes(configurations.Types);

        var entry = new ConfigurationEntry
        {
            Scheduling = new()
        };
        entry.SetConfiguration(new SampleConfiguration
        {
            IntValue = 42,
            StringValue = "Hello, World!"
        });
        configurations.SetEntry(nameof(Execute), entry);
        Assert.True(configurations.Execute(nameof(Execute), default));
    }
}
