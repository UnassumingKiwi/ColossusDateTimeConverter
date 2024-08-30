namespace ColossusDateTimeConverter.CLI.Ops;

public class NewOp : IOperation
{
    public string Name { get; } = "new";
    public string Usage { get; } = "new datetime {culture_key} [{year}] [{month_number}|{month_name}] [{day}] [{hour}] [{minute}] [{second}]";
    public string Description { get; } = "Creates a new datetime.";

    public void Execute(params string[] arguments)
    {
        if (arguments.Length < 2)
        {
            Operations.Execute("print", "Usage:", Usage);
            return;
        }

        var type = arguments[0];
        switch (type)
        {
            case "datetime":
                {
                    DateTime dateTime;
                    try
                    {
                        dateTime = CreateDateTime(arguments[1..]);
                    }
                    catch (Exception e)
                    {
                        Operations.Execute("print", e.Message);
                        return;
                    }
                    DateTimes.Add(dateTime);
                    Operations.Execute("print", $"Create new datetime: {dateTime}");
                }
                break;
            default: Operations.Execute("print", $"Unknown type {type}."); break;
        }
    }

    private DateTime CreateDateTime(string[] arguments)
    {
        var culture = Cultures.GetCulture(arguments[0]);
        var builder = new DateTimeBuilder()
            .OfCulture(culture);

        if (arguments.Length > 1)
            builder = builder.InYear(Convert.ToInt32(arguments[1]));
        if (arguments.Length > 2)
            builder = builder.InMonth(ConvertToMonth(arguments[2], culture));
        if (arguments.Length > 3)
            builder = builder.OnDay(Convert.ToInt32(arguments[3]));
        if (arguments.Length > 4)
            builder = builder.AtHour(Convert.ToInt32(arguments[4]));
        if (arguments.Length > 5)
            builder = builder.AtMinute(Convert.ToInt32(arguments[5]));
        if (arguments.Length > 6)
            builder = builder.AtSecond(Convert.ToInt32(arguments[6]));

        return builder.Build();
    }

    private CultureMonth ConvertToMonth(string monthStr, Culture culture)
    {
        var month = culture.Months.SingleOrDefault(m => m.Name == monthStr);

        if (month is null)
        {
            var monthNumber = Convert.ToInt32(monthStr);
            month = culture.Months.SingleOrDefault(m => m.Number == monthNumber);
        }

        if (month is null)
            throw new ArgumentException($"Month {monthStr} could not be found.");

        return month;
    }
}
