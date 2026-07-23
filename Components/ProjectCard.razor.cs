namespace PersonalPortfolio.v1.Components;

public partial class ProjectCard
{
    [Inject] private IJSRuntime JS { get; set; } = null!;
    [Inject] private NavigationManager Nav { get; set; } = null!;

    [Parameter] public string Slug { get; set; } = string.Empty;
    [Parameter] public string Title { get; set; } = string.Empty;
    [Parameter] public string Description { get; set; } = string.Empty;
    [Parameter] public List<string>? ImageUrls { get; set; }
    [Parameter] public string? GitHubLink { get; set; }
    [Parameter] public string? LiveDemo { get; set; }
    [Parameter] public bool LiveDemoActive { get; set; }
    [Parameter] public List<string>? Technologies { get; set; }
    [Parameter] public List<string>? Tags { get; set; }

    private MudCard? _cardRef;
    private List<string> _images => ImageUrls ?? [];
    private int _currentImage;

    private void NavigateToDetail() => Nav.NavigateTo($"/projects/{Slug}");

    private async Task HandleKeyDown(KeyboardEventArgs e)
    {
        if (e.Key is "Enter" or " ")
        {
            NavigateToDetail();
            await JS.InvokeVoidAsync("eval", "document.activeElement?.blur()");
        }
    }

    private void PrevImage()
    {
        _currentImage = _currentImage > 0 ? _currentImage - 1 : _images.Count - 1;
    }

    private void NextImage()
    {
        _currentImage = (_currentImage + 1) % _images.Count;
    }

    private async Task HandleImageError()
    {
        if (_images.Count > 0)
        {
            var fallback = await JS.InvokeAsync<string?>("getGitHubOgImage", Slug);
            if (!string.IsNullOrWhiteSpace(fallback))
            {
                _images[_currentImage] = fallback;
                StateHasChanged();
            }
        }
    }
}
