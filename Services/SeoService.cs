namespace PersonalPortfolio.v1.Services;

public sealed class SeoService
{
    private SiteContent? _content;

    public void SetSiteContent(SiteContent? content) => _content = content;

    public string DefaultTitle =>
        _content?.Site is { } site
            ? $"{site.FullName} — {site.ShortTitle}"
            : "Abhay Kumar Mandal — Backend Software Developer";

    public string DefaultDescription =>
        _content?.Site is { } site
            ? $"Portfolio of {site.FullName} — a results-driven {site.ShortTitle} specializing in JavaScript, Next.js, and cloud technologies."
            : "Portfolio of Abhay Kumar Mandal — a results-driven Backend Software Developer specializing in JavaScript, Next.js, .NET, Blazor, and cloud technologies.";

    public string PageTitle(string section) =>
        $"{section} | {(_content?.Site?.Name ?? "Abhay Kumar Mandal")}";

    public static string OgUrl(string path = "") =>
        string.IsNullOrEmpty(path) ? SiteConfig.SiteUrl : $"{SiteConfig.SiteUrl}{path}";
}
