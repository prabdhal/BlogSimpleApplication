namespace BlogSimple.Web.Services;

public static class TimeToWordsConverters
{
    public static string Second = "a second ago";
    public static string Seconds = "seconds ago";
    public static string Minute = "a minute ago";
    public static string Minutes = "minutes ago";
    public static string Hour = "an hour ago";
    public static string Hours = "hours ago";
    public static string Day = "a day ago";
    public static string Days = "days ago";
    public static string Month = "a month ago";
    public static string Months = "month ago";
    public static string Year = "a year ago";
    public static string Years = "years ago";


    public static string ConvertTimeToWords(DateTime date)
    {
        string result = "";

        int yearDiff = GetYearDifference(date);
        int monthDiff = GetMonthDifference(date);
        int dayDiff = GetDayDifference(date);
        int hourDiff = GetHourDifference(date);
        int minuteDiff = GetMinuteDifference(date);
        int secondDiff = GetSecondDifference(date);

        if (yearDiff > 0)
        {
            if (yearDiff > 1)
                result = yearDiff + " " + Years;
            else
                result = Year;
        }
        else if (monthDiff > 0)
        {
            if (monthDiff > 1)
                result = monthDiff + " " + Months;
            else
                result = Month;
        }
        else if (dayDiff > 0)
        {
            if (dayDiff > 1)
                result = dayDiff + " " + Days;
            else
                result = Day;
        }
        else if (hourDiff > 0)
        {
            if (hourDiff > 1)
                result = hourDiff + " " + Hours;
            else
                result = Hour;
        }
        else if (minuteDiff > 0)
        {
            if (minuteDiff > 1)
                result = minuteDiff + " " + Minutes;
            else
                result = Minute;
        }
        else if (secondDiff > 0)
        {
            if (secondDiff > 1)
                result = secondDiff + " " + Seconds;
            else
                result = Second;
        }

        return result;
    }

    public static int GetYearDifference(DateTime date)
    {
        var diffYear = DateTime.Now.Year - date.Year;
        return diffYear < 0 ? 0 : diffYear;
    }

    public static int GetMonthDifference(DateTime date)
    {
        var diffMonth = DateTime.Now.Month - date.Month;
        return diffMonth < 0 ? 0 : diffMonth;
    }

    public static int GetDayDifference(DateTime date)
    {
        var diffDay = DateTime.Now.Day - date.Day;
        return diffDay < 0 ? 0 : diffDay;
    }

    public static int GetHourDifference(DateTime date)
    {
        var diffHour = DateTime.Now.Hour - date.Hour;
        return diffHour < 0 ? 0 : diffHour;
    }

    public static int GetMinuteDifference(DateTime date)
    {
        var diffMinute = DateTime.Now.Minute - date.Minute;
        return diffMinute < 0 ? 0 : diffMinute;
    }

    public static int GetSecondDifference(DateTime date)
    {
        var diffSecond = DateTime.Now.Second - date.Second;
        return diffSecond < 0 ? 0 : diffSecond;
    }
}
