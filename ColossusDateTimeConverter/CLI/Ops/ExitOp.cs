namespace ColossusDateTimeConverter.CLI.Ops;

public class ExitOp : IOperation
{
    public string Name { get; } = "exit";
    public string Usage { get; } = "exit";
    public string Description { get; } = "Exits the application.";

    public void Execute(params string[] arguments)
    {
        Console.WriteLine("Exiting...");
        Program.Running = false;
    }
}
