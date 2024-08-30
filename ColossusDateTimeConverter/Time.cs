namespace ColossusDateTimeConverter
{
    public class Time
    {
        public Culture Culture { get; }
        public uint Hour { get; }
        public uint Minute { get; }
        public double Second { get; }

        public Time(uint hour, uint minute, double second, Culture culture)
        {
            Hour = hour;
            Minute = minute;
            Second = second;
            Culture = culture;
        }

        public Time ConvertToCulture(Culture culture)
        {
            if (culture == Culture)
                return new Time(Hour, Minute, Second, Culture);

            throw new NotImplementedException();
        }
    }
}
