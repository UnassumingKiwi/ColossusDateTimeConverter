using ColossusDateTimeConverter.PersistenceObjects;

namespace ColossusDateTimeConverter;

public class CultureMonth
{
    public string Name { get; }
    public int Number { get; }
    public int Days { get; }
    public bool GetsLeapDay { get; }

    public CultureMonth(string name, int number, int days, bool getsLeapDay)
    {
        Name = name;
        Number = number;
        Days = days;
        GetsLeapDay = getsLeapDay;
    }

    public CultureMonthPersistenceObject ToPersistenceObject()
    {
        return new CultureMonthPersistenceObject
        {
            Name = Name,
            Number = Number,
            Days = Days,
            GetsLeapDay = GetsLeapDay
        };
    }
}
