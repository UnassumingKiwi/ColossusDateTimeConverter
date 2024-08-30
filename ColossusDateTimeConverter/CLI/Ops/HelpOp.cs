namespace ColossusDateTimeConverter.CLI.Ops;

public class HelpOp : IOperation
{
    public string Name { get; } = "help";
    public string Usage { get; } = "help [{command}]";
    public string Description { get; } = "Provides information about the application and available commands.";

    public void Execute(params string[] arguments)
    {
        if (arguments.Length == 0)
        {
            Operations.Execute("print", "Usage:", Usage);

            var ops = Operations.GetAll();
            var namePad = ops.Select(op => op.Name.Length).Max() + 1;
            namePad = namePad > 8 ? namePad : 8;

            Operations.Execute("print", "Welcome to the Colossus DateTime converter!");
            Operations.Execute("print", "This application lets you convert between different date time calendar systems using 64-bit unix time.");
            Operations.Execute("print", "The following commands are available:");
            Operations.Execute("print", "Command".PadRight(namePad), "|", "Description");
            Operations.Execute("print", "".PadRight(namePad), "|");

            foreach (var op in ops)
                Operations.Execute("print", $"{op.Name}".PadRight(namePad), "|", op.Description);

            Operations.Execute("print", "");
            Operations.Execute("print", "To see more information about a command type \"help {command}\".");

            return;
        }

        var opName = arguments[0];
        var exists = Operations.TryGetOperation(opName, out var operation);

        if (!exists)
        {
            Operations.Execute("print", $"There is no '{opName}' command.");
            return;
        }

        Operations.Execute("print", operation.Name);
        Operations.Execute("print", operation.Description);
        Operations.Execute("print", "Usage:", operation.Usage);
    }
}
