namespace ColossusDateTimeConverter.CLI;

public interface IOperation
{
    string Name { get; }
    string Usage { get; }
    string Description { get; }

    public void Execute(params string[] arguments);
}
