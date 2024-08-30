namespace ColossusDateTimeConverter.PersistenceObjects;

public class CultureMonthPersistenceObject
{
    public string Name { get; set; }
    public int Number { get; set; }
    public int Days { get; set; }
    public bool GetsLeapDay { get; set; }

    public CultureMonth ToCultureMonth()
    {
        return new CultureMonth(Name, Number, Days, GetsLeapDay);
    }
}
