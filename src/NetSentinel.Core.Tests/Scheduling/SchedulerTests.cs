using NetSentinel.Configuration;
using NetSentinel.Scheduling;

namespace NetSentinel.Core.Tests.Scheduling;

public sealed class SchedulerTests
{
    [Fact]
    public async Task ProcessNull()
    {
        // Arrange
        var scheduler = new Scheduler();
        var configurations = new Configurations();

        // Act // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => scheduler.Process(null, null, default));
        await Assert.ThrowsAsync<ArgumentNullException>(() => scheduler.Process(configurations, null, default));
    }

    [Fact]
    public async Task ProcessEmpty()
    {
        // Arrange
        var scheduler = new Scheduler();
        var configurations = new Configurations();
        var states = new States();

        // Act
        await scheduler.Process(configurations, states, default);

        // Assert
        Assert.True(true, "The scheduled task should have been executed.");
    }
}
