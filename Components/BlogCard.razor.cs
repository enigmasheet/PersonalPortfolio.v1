namespace PersonalPortfolio.v1.Components;

public partial class BlogCard
{
    [Inject] private NavigationManager Navigation { get; set; } = null!;

    [Parameter] public string Slug { get; set; } = string.Empty;
    [Parameter] public string Title { get; set; } = string.Empty;
    [Parameter] public string? Summary { get; set; }
    [Parameter] public DateTime Date { get; set; }
    [Parameter] public List<string>? Tags { get; set; }

    private int _readingTime => string.IsNullOrWhiteSpace(Summary)
        ? 0
        : Math.Max(1, Summary.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length / 200);

    private void Navigate() => Navigation.NavigateTo($"/blog/{Slug}");

    private void HandleKeyDown(KeyboardEventArgs e)
    {
        if (e.Key is "Enter" or " ")
            Navigate();
    }
}
