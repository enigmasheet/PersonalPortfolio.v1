namespace PersonalPortfolio.v1.Components.Shared;

public partial class SectionHeader
{
    [Parameter] public string? Label { get; set; }
    [Parameter] public string? Title { get; set; }
    [Parameter] public string? Description { get; set; }
    [Parameter] public bool Centered { get; set; } = true;

    private string _titleClass => Centered ? "section-title text-center" : "section-title";
    private string _descClass => "text-secondary section-desc" + (Centered ? " text-center" : "");
}
