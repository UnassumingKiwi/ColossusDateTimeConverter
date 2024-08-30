namespace ColossusDateTimeConverter;

public static class DateTimeFormatter
{
    private static string YEAR = "y";
    private static string YEAR_SIGNIFIER = "x";
    private static string MONTH_NAME = "mon";
    private static string MONTH_NUMBER = "m";
    private static string DAY_NUMBER = "d";
    private static string HOUR_12_LONG = "shh";
    private static string HOUR_12_SHORT = "sh";
    private static string HOUR_LONG = "hh";
    private static string HOUR_SHORT = "h";
    private static string MINUTE_LONG = "MM";
    private static string MINUTE_SHORT = "M";
    private static string SECOND = "s";

    public static string FormatDateTime(DateTime date, string format, string yearSignifier = "")
    {
        var formatted = "";
        var working = format;
        while(working.Length > 0)
        {
            if (working.StartsWith(YEAR))
            {
                formatted += date.Year;
                working = working[1..];
            }
            else if (working.StartsWith(YEAR_SIGNIFIER))
            {
                formatted += yearSignifier;
                working = working[1..];
            }
            else if (working.StartsWith(MONTH_NAME))
            {
                formatted += date.Month.Name;
                working = working[3..];
            }
            else if (working.StartsWith(MONTH_NUMBER))
            {
                formatted += date.Month.Number;
                working = working[1..];
            }
            else if (working.StartsWith(DAY_NUMBER))
            {
                formatted += date.Day;
                working = working[1..];
            }
            else if (working.StartsWith(HOUR_12_LONG))
            {
                formatted += $"{((date.Hour + 11) % 12) + 1:D2}";
                working = working[3..];
            }
            else if (working.StartsWith(HOUR_12_SHORT))
            {
                formatted += ((date.Hour + 11) % 12) + 1;
                working = working[2..];
            }
            else if (working.StartsWith(HOUR_LONG))
            {
                formatted += $"{date.Hour:D2}";
                working = working[2..];
            }
            else if (working.StartsWith(HOUR_SHORT))
            {
                formatted += date.Hour;
                working = working[1..];
            }
            else if (working.StartsWith(MINUTE_LONG))
            {
                formatted += $"{date.Minute:D2}";
                working = working[2..];
            }
            else if (working.StartsWith(MINUTE_SHORT))
            {
                formatted += date.Minute;
                working = working[1..];
            }
            else if (working.StartsWith(SECOND))
            {
                formatted += $"{date.Second:D2}";
                working = working[1..];
            }
            else
            {
                formatted += working[0];
                working = working[1..];
            }
        }

        return formatted;
    }
}