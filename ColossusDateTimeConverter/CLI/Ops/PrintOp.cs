namespace ColossusDateTimeConverter.CLI.Ops;

public class PrintOp : IOperation
{
    public string Name { get; } = "print";
    public string Usage { get; } = "print {text1} [{text2}...]";
    public string Description { get; } = "Prints text to the screen.";

    public void Execute(params string[] arguments)
    {
        if (arguments.Length == 0)
            return;

        var message = string.Join(' ', arguments);
        Console.WriteLine(message);
    }
}
