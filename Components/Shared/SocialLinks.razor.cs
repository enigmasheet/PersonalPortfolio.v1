namespace PersonalPortfolio.v1.Components.Shared;

public partial class SocialLinks
{
    [Parameter] public Size Size { get; set; } = Size.Medium;
    [Parameter] public int Spacing { get; set; } = 4;
    [Parameter] public string Class { get; set; } = string.Empty;

    private Size IconSize => Size;
}
