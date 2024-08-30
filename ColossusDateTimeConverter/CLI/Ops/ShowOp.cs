namespace ColossusDateTimeConverter.CLI.Ops;

public class ShowOp : IOperation
{
    public string Name { get; } = "show";
    public string Usage { get; } = "show {type} all|{key} [format] [year_signifier]" + Environment.NewLine
        + "types: culture, datetime";
    public string Description { get; } = "Shows the value of an object in memory.";

    public void Execute(params string[] arguments)
    {
        if(arguments.Length < 2)
        {
            Operations.Execute("print", "Usage:", Usage);
            return;
        }

        var type = arguments[0];
        var key = arguments[1];
        var all = false;
        if(arguments[1] == "all")
            all = true;

        switch (type)
        {
            case ("culture"):
                {
                    if (all)
                        ShowAllCultures();
                    else
                        ShowCulture(key);
                }
                break;
            case ("datetime"):
                {
                    if (all)
                        ShowAllDateTimes();
                    else
                    {
                        string? format = null;
                        string? yearSignifier = null;

                        if (arguments.Length > 2)
                            format = arguments[2];
                        if (arguments.Length > 3)
                            yearSignifier = arguments[3];

                        ShowDateTime(key, format, yearSignifier);
                    }
                }break;
            default: Operations.Execute("print", $"Unknown type {type}."); break;

        }
    }

    private void ShowAllDateTimes()
    {
        var pairs = DateTimes.GetKeyValuePairs();
        var maxId = pairs.Select(p => p.Key).Max();
        var pad = maxId.ToString().Length + 1;
        pad = pad > 3 ? pad : 3;

        Operations.Execute("print", $"Id".PadRight(pad), "|",
            "Date");
        Operations.Execute("print", "".PadRight(pad), "|",
            "");

        foreach (var pair in pairs)
            Operations.Execute("print", $"{pair.Key}".PadRight(pad), "|",
                pair.Value.ToString());
    }

    private void ShowDateTime(string key, string? format, string? yearSignifier)
    {
        var keyInt = Convert.ToInt32(key);
        var dateTime = DateTimes.Get(keyInt);

        if(format is null)
            Operations.Execute("print", dateTime.ToString());
        else
            Operations.Execute("print", dateTime.ToString(format, yearSignifier));
    }

    private void ShowAllCultures()
    {
        var cultureNames = Cultures.GetAll().Select(c => c.Name).ToList();
        foreach (var cultureName in cultureNames)
            Operations.Execute("print", cultureName);
    }

    private void ShowCulture(string name)
    {
        Culture culture;
        try
        {
            culture = Cultures.GetCulture(name);
        }
        catch (Exception e)
        {
            Operations.Execute("print", e.Message);
            return;
        }

        Operations.Execute("print", culture.Name);
        Operations.Execute("print", 
            $"Offset: {culture.Offset:N0}".PadRight(26), "|", 
            $"Start Year: {culture.StartYear}", "|");
        Operations.Execute("print", 
            $"Seconds/Minute: {culture.SecondsPerMinute}".PadRight(26), "|", 
            $"Minutes/Hour: {culture.MinutesPerHour}".PadRight(18), "|", 
            $"Hours/Day: {culture.HoursPerDay}");
        Operations.Execute("print", 
            $"Leap Seconds/Year: {culture.LeapSecondsPerYear:N0}".PadRight(26), "|", 
            $"Year Signifier: {culture.YearSignifier}", "|");
        Operations.Execute("print", $"Standard Format: {culture.StandardFormat}");
        Operations.Execute("print", "Months:");


        var namePad = culture.Months.Select(m => m.Name.Length).Max() + 1;
        namePad = namePad < 5 ? 5 : namePad;
        var orderedMonths = culture.Months.OrderBy(m => m.Number).ToList();

        Operations.Execute("print", "|",
            $"Name".PadRight(namePad), "|",
            $"Number".PadRight(7), "|",
            $"Days".PadRight(5), "|",
            $"Gets Leap Day");

        Operations.Execute("print", "|",
            $"----".PadRight(namePad), "|",
            $"------".PadRight(7), "|",
            $"----".PadRight(5), "|",
            "-------------");

        foreach (var month in culture.Months)
        {
            var getsLeapDay = month.GetsLeapDay ? "Yes" : "";
            Operations.Execute("print", "|",
                $"{month.Name}".PadRight(namePad), "|",
                $"#{month.Number}".PadRight(7), "|",
                $"{month.Days}".PadRight(5), "|",
                $"{getsLeapDay}");
        }
    }
}
