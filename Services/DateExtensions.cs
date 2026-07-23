namespace PersonalPortfolio.v1.Services;

public static class DateExtensions
{
    public static string ToDisplayDate(this DateTime dt) => dt.ToString("MMM dd, yyyy");
    public static string ToDisplayMonthYear(this DateTime dt) => dt.ToString("MMM yyyy");
    public static string ToDisplayDateRange(DateTime start, DateTime? end) =>
        $"{start.ToDisplayMonthYear()} – {(end.HasValue ? end.Value.ToDisplayMonthYear() : "Present")}";
}
