namespace PersonalPortfolio.v1.Components.Shared;

public partial class Skeleton
{
    [Parameter] public string Type { get; set; } = "card";
    [Parameter] public int Count { get; set; } = 3;
    [Parameter] public int Lines { get; set; } = 3;
}
