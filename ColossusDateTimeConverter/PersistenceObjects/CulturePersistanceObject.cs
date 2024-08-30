namespace ColossusDateTimeConverter.PersistenceObjects;

public class CulturePersistanceObject
{
    public string Key { get; set; }
    public string Name { get; set; }
    public long Offset { get; set; }
    public int StartYear { get; set; }

    public long SecondsPerMinute { get; set; }
    public long MinutesPerHour { get; set; }
    public long HoursPerDay { get; set; }

    public List<CultureMonthPersistenceObject> Months { get; set; }
    public long LeapSecondsPerYear { get; set; }

    public string YearSignifier { get; set; }
    public string StandardFormat { get; set; }

    public Culture ToCulture()
    {
        return new Culture(Key, Name, Offset, StartYear, SecondsPerMinute, MinutesPerHour, HoursPerDay, Months.Select(m => m.ToCultureMonth()).ToList(), LeapSecondsPerYear, YearSignifier, StandardFormat);
    }
}
