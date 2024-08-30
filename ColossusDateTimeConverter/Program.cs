using ColossusDateTimeConverter.CLI;

internal class Program
{
    public static bool Running { get; set; } = true;
    private static void Main(string[] args)
    {
        Console.Title = "Colossus DateTime Converter";

        Operations.AddAll();
        Operations.Execute("print", "Colossus DateTime Converter started...");

        while (Running)
        {
            Console.Write("> ");
            var input = Console.ReadLine();

            if (input is null)
                continue;

            var command = Command.Parse(input);
            Operations.Execute(command.Operation, command.Arguments);
        }
    }
}
