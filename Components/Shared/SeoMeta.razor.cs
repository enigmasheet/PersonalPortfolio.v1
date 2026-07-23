using PersonalPortfolio.v1.Services;

namespace PersonalPortfolio.v1.Components.Shared;

public partial class SeoMeta
{
    [Parameter] public string Title { get; set; } = string.Empty;
    [Parameter] public string Description { get; set; } = string.Empty;
    [Parameter] public string OgType { get; set; } = SiteConfig.DefaultOgType;
    [Parameter] public string OgImage { get; set; } = SiteConfig.OgImageUrl;
    [Parameter] public string OgUrl { get; set; } = string.Empty;
    [Parameter] public string OgSiteName { get; set; } = SiteConfig.Domain;
    [Parameter] public string? Author { get; set; }
}
