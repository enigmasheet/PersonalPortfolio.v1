using PersonalPortfolio.v1.Services;

namespace PersonalPortfolio.v1.Layout;

public partial class MainLayout : IAsyncDisposable
{
    [Parameter] public RenderFragment Body { get; set; } = null!;

    private bool _drawerOpen;
    private string _siteName = "Abhay";
    private string _copyright = "© 2026 Abhay Kumar Mandal. All rights reserved.";
    private List<string> _navLinks = [];
    private static readonly string[] _navPaths = ["/", "/about", "/experience", "/projects", "/blog", "/resume", "/contact"];
    private readonly CancellationTokenSource _cts = new();
    private IJSObjectReference? _module;

    [Inject] private IDataService DataService { get; set; } = null!;
    [Inject] private ThemeService Theme { get; set; } = null!;
    [Inject] private IJSRuntime JS { get; set; } = null!;
    [Inject] private NavigationManager Navigation { get; set; } = null!;

    protected override void OnInitialized()
    {
        Navigation.LocationChanged += (_, _) => StateHasChanged();
    }

    private bool IsActive(string path)
    {
        var current = Navigation.Uri;
        var baseUri = Navigation.BaseUri;
        if (path == "/") return current.TrimEnd('/') == baseUri.TrimEnd('/');
        return current.StartsWith(baseUri.TrimEnd('/') + path, StringComparison.OrdinalIgnoreCase);
    }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            var content = await DataService.GetSiteContent(_cts.Token);
            if (content?.Site?.Name is { } name) _siteName = name;
            if (content?.Nav?.Links is { } links)
            {
                _navLinks = links.Take(_navPaths.Length).ToList();
                while (_navLinks.Count < _navPaths.Length) _navLinks.Add("");
            }
            if (content?.Footer?.Copyright is { } cr)
            {
                _copyright = cr.Replace("{year}", DateTime.UtcNow.Year.ToString());
            }
        }
        catch
        {
            // Layout renders with defaults
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _module = await JS.InvokeAsync<IJSObjectReference>("import", "./Layout/MainLayout.razor.js");
            await _module.InvokeVoidAsync("registerScroll");
        }
    }

    private async Task ToggleDrawer()
    {
        _drawerOpen = !_drawerOpen;
        if (_module != null)
            await _module.InvokeVoidAsync("setBodyScroll", !_drawerOpen);
    }

    private async Task CloseDrawer()
    {
        _drawerOpen = false;
        if (_module != null)
            await _module.InvokeVoidAsync("setBodyScroll", true);
    }

    private void ToggleTheme() => Theme.Toggle();

    private async Task ScrollToTop()
    {
        await JS.InvokeVoidAsync("window.scrollTo", new { top = 0, behavior = "smooth" });
    }

    public async ValueTask DisposeAsync()
    {
        _cts.Cancel();
        _cts.Dispose();
        if (_module != null)
        {
            await _module.InvokeVoidAsync("cleanupScroll");
            await _module.DisposeAsync();
        }
    }
}
