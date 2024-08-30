namespace ColossusDateTimeConverter;

public class DateTimeBuilder
{
    private Culture? Culture { get; init; }
    private int? Year { get; init; }
    private CultureMonth? Month { get; init; }
    private int Day { get; init; } = 1;
    private int Hour { get; init; }
    private int Minute { get; init; }
    private int Second { get; init; }

    public DateTimeBuilder OfCulture(Culture culture)
    {
        return new DateTimeBuilder
        {
            Culture = culture,
            Year = Year ?? culture.StartYear,
            Month = Month ?? culture.GetFirstMonth(),
            Day = Day,
            Hour = Hour,
            Minute = Minute,
            Second = Second,
        };
    }

    public DateTimeBuilder InYear(int year)
    {
        return new DateTimeBuilder
        {
            Culture = Culture,
            Year = year,
            Month = Month,
            Day = Day,
            Hour = Hour,
            Minute = Minute,
            Second = Second,
        };
    }

    public DateTimeBuilder InMonth(CultureMonth month)
    {
        return new DateTimeBuilder
        {
            Culture = Culture,
            Year = Year,
            Month = month,
            Day = Day,
            Hour = Hour,
            Minute = Minute,
            Second = Second,
        };
    }

    public DateTimeBuilder OnDay(int day)
    {
        return new DateTimeBuilder
        {
            Culture = Culture,
            Year = Year,
            Month = Month,
            Day = day,
            Hour = Hour,
            Minute = Minute,
            Second = Second,
        };
    }

    public DateTimeBuilder AtHour(int hour)
    {
        return new DateTimeBuilder
        {
            Culture = Culture,
            Year = Year,
            Month = Month,
            Day = Day,
            Hour = hour,
            Minute = Minute,
            Second = Second,
        };
    }

    public DateTimeBuilder AtMinute(int minute)
    {
        return new DateTimeBuilder
        {
            Culture = Culture,
            Year = Year,
            Month = Month,
            Day = Day,
            Hour = Hour,
            Minute = minute,
            Second = Second,
        };
    }

    public DateTimeBuilder AtSecond(int second)
    {
        return new DateTimeBuilder
        {
            Culture = Culture,
            Year = Year,
            Month = Month,
            Day = Day,
            Hour = Hour,
            Minute = Minute,
            Second = second,
        };
    }

    public DateTime Build()
    {
        if (Culture is null)
            throw new ApplicationException("Missing culture.");
        if (Year is null)
            throw new ApplicationException("Missing year.");
        if (Month is null)
            throw new ApplicationException("Missing month.");

        var unixSeconds = Culture.Offset
            + Culture.YearToSeconds(Year.Value)
            + Culture.MonthToSeconds(Month)
            + Culture.DaysToSeconds(Day)
            + Culture.HoursToSeconds(Hour)
            + Culture.MinutesToSeconds(Minute)
            + Second;

        return new DateTime(unixSeconds, Culture);
    }
}
