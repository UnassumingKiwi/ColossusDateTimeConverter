namespace ColossusDateTimeConverter.CLI.Ops;

public class ConvertOp : IOperation
{
    public string Name { get; } = "convert";
    public string Usage { get; } = "convert datetime {id} {culture} {format} {year_signifier}";
    public string Description { get; } = "Converts a datetime from one culture to another.";

    public void Execute(params string[] arguments)
    {
        if(arguments.Length < 3)
        {
            Operations.Execute("print", "Usage:", Usage);
            return;
        }

        var type = arguments[0];
        switch (type)
        {
            case ("datetime"):
                {
                    var dateTime = DateTimes.Get(Convert.ToInt32(arguments[1]));
                    var culture = Cultures.GetCulture(arguments[2]);
                    string? format = null;
                    string? yearSignifier = null;

                    if (arguments.Length > 3)
                        format = arguments[3];
                    if (arguments.Length > 4)
                        yearSignifier = arguments[4];

                    ConvertDateTime(dateTime, culture, format, yearSignifier);
                }
                break;
            default: Operations.Execute("print", $"Unknown type {type}."); break;
        }
    }

    private void ConvertDateTime(DateTime dateTime, Culture culture, string? format, string? yearSignifier)
    {
        var converted = dateTime.ToCulture(culture);

        Operations.Execute("print", "Original ", "|", dateTime.ToString());
        Operations.Execute("print", "Converted", "|", converted.ToString());
    }
}
