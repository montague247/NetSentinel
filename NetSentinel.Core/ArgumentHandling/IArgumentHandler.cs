namespace NetSentinel.ArgumentHandling
{
    public interface IArgumentHandler
    {
        void Process(string[] arguments, ref int index);
    }
}
