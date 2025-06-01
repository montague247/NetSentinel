namespace NetSentinel.Argument;

public interface IArgumentHandler
{
    void Process(string[] arguments, ref int index);

    void Execute(IGlobalOptions options);
}
