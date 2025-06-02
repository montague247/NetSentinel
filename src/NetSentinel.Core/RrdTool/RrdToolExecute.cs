using Charon.System;

namespace NetSentinel.RrdTool;

public static class RrdToolExecute
{
    /// <summary>
    /// Creates an RRD (Round-Robin Database) using the specified builder action.
    /// </summary>
    /// <param name="create">An action that configures the <see cref="CreateBuilder"/> instance.</param>
    /// <param name="globalOptions">Global options.</param>
    public static void Create(Action<CreateBuilder> create, IGlobalOptions globalOptions)
    {
        var builder = new CreateBuilder();
        create(builder);

        Execute(builder.Build(), globalOptions);
    }

    /// <summary>
    /// Updates an RRD (Round-Robin Database) using the specified builder action.
    /// </summary>
    /// <param name="update">An action that configures the <see cref="UpdateBuilder"/> instance.</param>
    /// <param name="globalOptions">Global options.</param>
    public static void Update(Action<UpdateBuilder> update, IGlobalOptions globalOptions)
    {
        var builder = new UpdateBuilder();
        update(builder);

        Execute(builder.Build(), globalOptions);
    }

    /// <summary>
    /// Graph an RRD (Round-Robin Database) using the specified builder action.
    /// </summary>
    /// <param name="update">An action that configures the <see cref="GraphBuilder"/> instance.</param>
    /// <param name="globalOptions">Global options.</param>
    public static void Graph(Action<GraphBuilder> graph, IGlobalOptions globalOptions)
    {
        var builder = new GraphBuilder();
        graph(builder);

        Execute(builder.Build(), globalOptions);
    }

    private static void Execute(List<string> arguments, IShellOptions shellOptions)
    {
        if (shellOptions != null && !shellOptions.NoInstall)
            Shell.CheckInstall("rrdtool", shellOptions);

        Shell.Execute("rrdtool", arguments);
    }
}
