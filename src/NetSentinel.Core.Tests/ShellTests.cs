namespace NetSentinel.Core.Tests
{
    public sealed class ShellTests
    {
        [Fact]
        public void ExecuteWhichGit()
        {
            Assert.Equal(0, Shell.Execute("which", ["git"]));
        }

        [Fact]
        public void ExecuteWhichGrmlToul()
        {
            Assert.Equal(1, Shell.Execute("which", ["GrmlToul"]));
        }
    }
}
