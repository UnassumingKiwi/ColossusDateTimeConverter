namespace ColossusDateTimeConverter;

public class DateTime : IEquatable<DateTime>, IEquatable<long>
{
    public Culture Culture { get; }
    public long UnixSeconds { get; }

    public int Year { get; }
    public CultureMonth Month { get; }
    public int Day { get; }
    public int Hour { get; }
    public int Minute { get; }
    public int Second { get; }

    public DateTime(long unixSeconds, Culture culture)
    {
        UnixSeconds = unixSeconds;
        Culture = culture;

        Year = Culture.GetYear(UnixSeconds);
        Month = Culture.GetMonth(UnixSeconds);
        Day = Culture.GetDay(UnixSeconds);
        Hour = Culture.GetHour(UnixSeconds);
        Minute = Culture.GetMinute(UnixSeconds);
        Second = Culture.GetSecond(UnixSeconds);
    }

    public override string ToString()
    {
        return DateTimeFormatter.FormatDateTime(this, Culture.StandardFormat, Culture.YearSignifier);
    }

    public string ToString(string format, string? yearSignifier = null)
    {
        return DateTimeFormatter.FormatDateTime(this, format, Culture.YearSignifier);
    }

    public DateTime AddSeconds(long seconds)
    {
        return new DateTime(UnixSeconds + seconds, Culture);
    }

    public DateTime AddSeconds(int seconds)
    {
        var lSeconds = Convert.ToInt64(seconds);
        return new DateTime(UnixSeconds + lSeconds, Culture);
    }

    public DateTime AddMinutes(int minutes)
    {
        var minuteSeconds = Culture.MinutesToSeconds(minutes);
        return new DateTime(UnixSeconds + minuteSeconds, Culture);
    }

    public DateTime AddHours(int hours)
    {
        var hourSeconds = Culture.HoursToSeconds(hours);
        return new DateTime(UnixSeconds + hourSeconds, Culture);
    }

    public DateTime AddDays(int days)
    {
        var daySeonds = Culture.DaysToSeconds(days);
        return new DateTime(UnixSeconds + daySeonds, Culture);
    }

    public DateTime AddMonths(int months)
    {
        var monthSeconds = Culture.MonthCountToSeconds(Month, Day, Math.Abs(months), months > 0);
        return new DateTime(UnixSeconds + monthSeconds, Culture);
    }

    public DateTime AddYears(int years)
    {
        var yearSeconds = Culture.GetSecondsInAYear() * years;
        return new DateTime(UnixSeconds + yearSeconds, Culture);
    }

    public DateTime ToCulture(Culture culture)
    {
        return new DateTime(UnixSeconds, culture);
    }

    public override bool Equals(object? obj)
    {
        return obj is DateTime dt
            ? Equals(dt)
            : obj is long l 
                && Equals(l);
    }

    public bool Equals(DateTime? other)
    {
        if (other is null)
            return false;

        return other.UnixSeconds == UnixSeconds;
    }

    public bool Equals(long other)
    {
        return other == UnixSeconds;
    }

    public override int GetHashCode()
    {
        return UnixSeconds.GetHashCode();
    }

    #region Operators
    public static bool operator ==(DateTime dt1, DateTime dt2)
    {
        return dt1.Equals(dt2);
    }

    public static bool operator !=(DateTime dt1, DateTime dt2)
    {
        return !dt1.Equals(dt2);
    }

    public static bool operator >(DateTime dt1, DateTime dt2)
    {
        return dt1.UnixSeconds > dt2.UnixSeconds;
    }

    public static bool operator <(DateTime dt1, DateTime dt2)
    {
        return dt1.UnixSeconds < dt2.UnixSeconds;
    }


    public static bool operator >=(DateTime dt1, DateTime dt2)
    {
        return dt1.UnixSeconds >= dt2.UnixSeconds;
    }

    public static bool operator <=(DateTime dt1, DateTime dt2)
    {
        return dt1.UnixSeconds <= dt2.UnixSeconds;
    }

    public static bool operator ==(DateTime dt, long l)
    {
        return dt.Equals(l);
    }

    public static bool operator !=(DateTime dt, long l)
    {
        return !dt.Equals(l);
    }

    public static bool operator >(DateTime dt, long l)
    {
        return dt.UnixSeconds > l;
    }

    public static bool operator <(DateTime dt, long l)
    {
        return dt.UnixSeconds < l;
    }


    public static bool operator >=(DateTime dt, long l)
    {
        return dt.UnixSeconds >= l;
    }

    public static bool operator <=(DateTime dt, long l)
    {
        return dt.UnixSeconds <= l;
    }
    #endregion
}
