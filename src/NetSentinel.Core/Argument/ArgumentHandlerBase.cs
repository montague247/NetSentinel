namespace NetSentinel.Argument;

public abstract class ArgumentHandlerBase : IArgumentHandler
{
    public abstract void Execute(IGlobalOptions options);

    public virtual void Process(string[] arguments, ref int index)
    {
        while (index < arguments.Length)
        {
            if (!Process(arguments[index], arguments, ref index))
                return;

            index++;
        }
    }

    protected abstract bool Process(string argument, string[] arguments, ref int index);
}
