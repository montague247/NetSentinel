using System.Runtime.CompilerServices;

namespace NetSentinel.Core.Tests
{
    public static class TestExtensions
    {
        public static string? GetCallerPath([CallerFilePath] string? sourceFilePath = null)
        {
            return sourceFilePath == null ? throw new ArgumentNullException(nameof(sourceFilePath)) : Path.GetDirectoryName(sourceFilePath);
        }
    }
}
