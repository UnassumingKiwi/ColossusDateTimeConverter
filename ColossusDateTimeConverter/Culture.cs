using ColossusDateTimeConverter.PersistenceObjects;

namespace ColossusDateTimeConverter;

public class Culture
{
    public string Key { get; }
    public string Name { get; }
    public long Offset { get;  }

    public int StartYear { get; }

    public long SecondsPerMinute { get; }  
    public long MinutesPerHour { get; }
    public long HoursPerDay { get; }

    public List<CultureMonth> Months { get; }
    public long LeapSecondsPerYear { get; }

    public string YearSignifier { get; }
    public string StandardFormat { get; }

    public Culture(string key, string name, long offset, int startYear, long secondsPerMinute, long minutesPerHour, long hoursPerDay, List<CultureMonth> months, double leapDaysPerYear, string yearSignifier, string standardFormat)
    {
        ValidateMonths(name, months);

        Key = key;
        Name = name;
        Offset = offset;
        StartYear = startYear;
        SecondsPerMinute = secondsPerMinute;
        MinutesPerHour = minutesPerHour;
        HoursPerDay = hoursPerDay;
        Months = months;
        LeapSecondsPerYear = (long)(leapDaysPerYear * hoursPerDay * minutesPerHour * secondsPerMinute);
        YearSignifier = yearSignifier;
        StandardFormat = standardFormat;
    }

    public Culture(string key, string name, long offset, int startYear, long secondsPerMinute, long minutesPerHour, long hoursPerDay, List<CultureMonth> months, long leapSecondsPerYear, string yearSignifier, string standardFormat)
    {
        ValidateMonths(name, months);

        Key = key;
        Name = name;
        Offset = offset;
        StartYear = startYear;
        SecondsPerMinute = secondsPerMinute;
        MinutesPerHour = minutesPerHour;
        HoursPerDay = hoursPerDay;
        Months = months;
        LeapSecondsPerYear = leapSecondsPerYear;
        YearSignifier = yearSignifier;
        StandardFormat = standardFormat;
    }

    private static void ValidateMonths(string name, List<CultureMonth> months)
    {
        List<int> monthNumbers = months.Select(m => m.Number).ToList();
        if (monthNumbers.Min() != 1)
            throw new ArgumentException($"{name} : Month numbers do not start at 1.");
        if (monthNumbers.Count != monthNumbers.Distinct().Count())
            throw new ArgumentException($"{name} : At least 2 months have the same number.");
        if (Enumerable.Range(1, monthNumbers.Count).Except(monthNumbers).Any())
            throw new ArgumentException($"{name} : Month numbers are not in counting order.");
        
        var leapMonths = months.Where(m => m.GetsLeapDay).Count();
        if (leapMonths == 0)
            throw new ArgumentException($"{name} : Missing leap month.");
        if (leapMonths > 1)
            throw new ArgumentException($"{name} : More than one leap month found.");
    }

    public CultureMonth GetFirstMonth()
    {
        return Months.Single(m => m.Number == 1);
    }

    public int GetYear(long unixSeconds)
    {
        var lYear = ((unixSeconds - Offset) / GetSecondsInAYear()) + StartYear;
        return Convert.ToInt32(lYear);
    }

    public CultureMonth GetMonth(long unixSeconds)
    {
        var year = GetYear(unixSeconds);
        var hasLeapDay = YearHasLeapDay(year);
        var seconds = (unixSeconds - Offset) % GetSecondsInAYear();
        var days = seconds / GetSecondsInADay();

        CultureMonth? currentMonth = null;
        var orderedMonths = Months.OrderBy(m => m.Number).ToList();
        foreach (var month in orderedMonths)
        {
            var monthDays = hasLeapDay && month.GetsLeapDay ? month.Days + 1 : month.Days;
            if (monthDays >= days)
            {
                currentMonth = month;
                break;
            }
            days -= monthDays;
        }

        if (currentMonth is null)
            throw new ApplicationException("Could not find current month.");

        return currentMonth;
    }

    public int GetDay(long unixSeconds)
    {
        var year = GetYear(unixSeconds);
        var hasLeapDay = YearHasLeapDay(year);
        var seconds = (unixSeconds - Offset) % GetSecondsInAYear();
        var days = seconds / GetSecondsInADay();

        CultureMonth? currentMonth = null;
        long currentDay = 0;
        var orderedMonths = Months.OrderBy(m => m.Number).ToList();
        foreach (var month in orderedMonths)
        {
            var monthDays = hasLeapDay && month.GetsLeapDay ? month.Days + 1 : month.Days;
            if (monthDays >= days)
            {
                currentDay = days != 0 ? days : 1;
                break;
            }
            days -= monthDays;
        }

        if (currentDay == 0)
            throw new ApplicationException("Could not find current day.");

        return Convert.ToInt32(currentDay);
    }

    public int GetHour(long unixSeconds)
    {
        var seconds = ((unixSeconds - Offset) % GetSecondsInAYear()) % GetSecondsInADay();
        return Convert.ToInt32(seconds / GetSecondsInAnHour());
    }

    public int GetMinute(long unixSeconds)
    {
        var seconds = ((unixSeconds - Offset) % GetSecondsInAYear()) % GetSecondsInADay() % GetSecondsInAnHour();
        return Convert.ToInt32(seconds / SecondsPerMinute);
    }

    public int GetSecond(long unixSeconds)
    {
        var seconds = ((unixSeconds - Offset) % GetSecondsInAYear()) % GetSecondsInADay() % GetSecondsInAnHour() % SecondsPerMinute;
        return Convert.ToInt32(seconds);
    }

    public long YearToSeconds(int year)
    {
        return GetSecondsInAYear() * (year - StartYear);
    }

    public long MonthToSeconds(CultureMonth month)
    {
        var firstMonth = Months.OrderBy(m => m.Number).First();
        return MonthCountToSeconds(firstMonth, 1, month.Number - 1, true);
    }

    public long DaysToSeconds(int days)
    {
        return days * HoursPerDay * MinutesPerHour * SecondsPerMinute;
    }

    public long HoursToSeconds(int hours)
    {
        return hours * MinutesPerHour * SecondsPerMinute;
    }

    public long MinutesToSeconds(int minutes)
    {
        return minutes * SecondsPerMinute;
    }

    public long MonthCountToSeconds(CultureMonth month, int currentDay, int number, bool inFuture)
    {
        if (number < 0)
            throw new ArgumentException("Months in future cannot be negative.");

        if (number == 0)
            return 0;

        return inFuture ? MonthCountToSecondsInFuture(month, currentDay, number) : MonthCountToSecondsInPast(month, currentDay, number);
    }

    private long MonthCountToSecondsInFuture(CultureMonth month, int currentDay, int number)
    {
        var days = month.Days;
        for(var n = 1; n < number; n++)
        {
            var workMonth = Months.Single(m => m.Number == (((month.Number + n - 1) % Months.Count) + 1));
            days += workMonth.Days;
        }

        return DaysToSeconds(days);
    }

    private long MonthCountToSecondsInPast(CultureMonth month, int currentDay, int number)
    {
        var lastMonth = Months.Single(m => month.Number > 1
            ? m.Number == month.Number - 1
            : m.Number == Months.Count);

        var days = currentDay + (lastMonth.Days >= currentDay
            ? lastMonth.Days - currentDay
            : 0);
        for (var n = 1; n < number; n++)
        {
            var monthNumber = month.Number - n;
            while (monthNumber < 1)
                monthNumber += Months.Count();

            var workMonth = Months.Single(m => m.Number == (((monthNumber - 1) % Months.Count) + 1));
            days += workMonth.Days;
        }

        return DaysToSeconds(days);
    }

    public long GetSecondsInAYear()
    {
        var daysInAYear = Months.Sum(m => m.Days);
        return (daysInAYear * HoursPerDay * MinutesPerHour * SecondsPerMinute) + LeapSecondsPerYear;
    }

    private long GetSecondsInADay()
    {
        return SecondsPerMinute * MinutesPerHour * HoursPerDay;
    }

    private long GetSecondsInAnHour()
    {
        return SecondsPerMinute * MinutesPerHour;
    }

    private bool YearHasLeapDay(int year)
    {
        var leapSeconds = LeapSecondsPerYear * year;
        var leapSecondsRemaining = leapSeconds % GetSecondsInADay();

        return leapSecondsRemaining < LeapSecondsPerYear;
    }

    public CulturePersistanceObject ToPersistenceObject()
    {
        return new CulturePersistanceObject
        {
            Name = Name,
            Offset = Offset,
            StartYear = StartYear,
            SecondsPerMinute = SecondsPerMinute,
            MinutesPerHour = MinutesPerHour,
            HoursPerDay = HoursPerDay,
            LeapSecondsPerYear = LeapSecondsPerYear,
            YearSignifier = YearSignifier,
            StandardFormat = StandardFormat,
            Months = Months.Select(m => m.ToPersistenceObject()).ToList()
        };
    }
}