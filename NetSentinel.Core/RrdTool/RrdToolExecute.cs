namespace NetSentinel.RrdTool
{
    public static class RrdToolExecute
    {
        /// <summary>
        /// Creates an RRD (Round-Robin Database) using the specified builder action.
        /// </summary>
        /// <param name="create">An action that configures the <see cref="CreateBuilder"/> instance.</param>
        public static void Create(Action<CreateBuilder> create)
        {
            var builder = new CreateBuilder();
            create(builder);

            Execute(builder.Build());
        }

        public static void Update(Action<UpdateBuilder> update)
        {
            var builder = new UpdateBuilder();
            update(builder);

            Execute(builder.Build());
        }

        public static void Graph(Action<GraphBuilder> graph)
        {
            var builder = new GraphBuilder();
            graph(builder);

            Execute(builder.Build());
        }

        private static void Execute(List<string> arguments)
        {
            Shell.Execute("rrdtool", arguments);
        }
    }
}
