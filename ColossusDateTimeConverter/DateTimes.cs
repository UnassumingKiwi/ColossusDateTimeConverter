namespace ColossusDateTimeConverter;

public static class DateTimes
{
    private static Dictionary<int, DateTime> dateTimes = new Dictionary<int, DateTime>();
    private static int nextId { get; set; } = 1;

    public static void Add(DateTime date)
    {
        dateTimes.Add(nextId++, date);
    }

    public static List<KeyValuePair<int, DateTime>> GetKeyValuePairs()
    {
        return dateTimes.ToList();
    }

    public static List<DateTime> GetAll()
    {
        return dateTimes.Values.ToList();
    }

    public static DateTime Get(int id)
    {
        return dateTimes[id];
    }
}
