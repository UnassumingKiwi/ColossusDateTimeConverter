namespace ColossusDateTimeConverter.CLI;

public class Command
{
    public static Command Parse(string input)
    {
        var working = input.TrimStart().TrimEnd();

        if (working.Length == 0)
            return new Command("print");

        var operation = "";
        while (working.Length > 0 && working[0] != ' ')
        {
            operation += working[0];
            working = working[1..];
        }

        var openQuotes = "";
        var arguments = new List<string>();
        var argument = "";
        while (working.Length > 0)
        {
            if (working[0] == ' ' && openQuotes.Length == 0)
            {
                if (argument.Length > 0)
                    arguments.Add(argument);

                argument = "";
                working = working[1..];
                continue;
            }
            else if (working[0] == '"')
            {
                if (openQuotes.Length == 0)
                {
                    openQuotes = "\"";
                    working = working[1..];
                    continue;
                }
                else if (openQuotes == "\"")
                {
                    openQuotes = "";
                    working = working[1..];
                    continue;
                }
            }
            else if (working[0] == '\'')
            {
                if (openQuotes.Length == 0)
                {
                    openQuotes = "\'";
                    working = working[1..];
                    continue;
                }
                else if (openQuotes == "\'")
                {
                    openQuotes = "";
                    working = working[1..];
                    continue;
                }
            }

            argument += working[0];
            working = working[1..];
        }

        if (argument.Length > 0)
            arguments.Add(argument);

        return new Command(operation, arguments.ToArray());
    }

    public string Operation { get; }
    public string[] Arguments { get; }

    public Command(string operation, params string[] arguments)
    {
        Operation = operation;
        Arguments = arguments;
    }
}
