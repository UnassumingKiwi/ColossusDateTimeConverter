using Microsoft.VisualBasic;

namespace ColossusDateTimeConverter;

public class Date
{
    public Culture Culture { get; }
    public uint Year { get; }
    public uint Month { get; }
    public uint Day { get; }

    public Date(uint year, uint month, uint day, Culture culture)
    {
        Year = year;
        Month = month; 
        Day = day;
        Culture = culture;
    }

    public Date ConvertToCulture(Culture culture)
    {
        if (culture == Culture)
            return new Date(Year, Month, Day, Culture);

        throw new NotImplementedException();
    }
}
