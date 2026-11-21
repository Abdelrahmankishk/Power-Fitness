namespace GymManagementSystem.BLL.Common;

public static class EgyptTimeZone
{
    private static readonly TimeZoneInfo TimeZone =
        TimeZoneInfo.FindSystemTimeZoneById(
            OperatingSystem.IsWindows() ? "Egypt Standard Time" : "Africa/Cairo");

    public static DateTime ToUtc(DateTime egyptLocalTime)
    {
        var localTime = DateTime.SpecifyKind(egyptLocalTime, DateTimeKind.Unspecified);
        return TimeZoneInfo.ConvertTimeToUtc(localTime, TimeZone);
    }

    public static DateTime ToEgyptLocal(DateTime utcTime)
    {
        var utc = DateTime.SpecifyKind(utcTime, DateTimeKind.Utc);
        return TimeZoneInfo.ConvertTimeFromUtc(utc, TimeZone);
    }
}
