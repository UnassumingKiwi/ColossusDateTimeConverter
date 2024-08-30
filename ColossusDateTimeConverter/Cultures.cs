namespace ColossusDateTimeConverter;

public static class Cultures
{
    private static Dictionary<string, Culture> cultures = new Dictionary<string, Culture>();

    public static void AddCulture(Culture culture)
    {
        cultures.Add(culture.Key, culture);
    }

    public static Culture GetCulture(string name)
    {
        cultures.TryGetValue(name, out var culture);

        if (culture is null)
            throw new ArgumentException($"Culture '{name}' does not exist.");

        return culture;
    }

    public static List<Culture> GetAll()
    {
        return cultures.Values.ToList();
    }
}
