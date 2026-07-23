using PersonalPortfolio.v1.Services;

namespace PersonalPortfolio.v1.Pages;

public partial class Projects : IDisposable
{
    private SiteContent? _content;
    List<ProjectModel>? _projects;
    private string _searchQuery = string.Empty;
    private string? _activeTech;
    private List<ProjectModel>? _filteredCache;
    private List<string>? _allTechCache;
    private int _cacheVersion;
    private int _cachedVersion = -1;
    private readonly CancellationTokenSource _cts = new();

    [Inject] private IDataService DataService { get; set; } = null!;
    [Inject] private SeoService Seo { get; set; } = null!;
    [Inject] private ILogger<Projects> Logger { get; set; } = null!;

    private IEnumerable<ProjectModel> filteredProjects
    {
        get
        {
            if (_projects == null) return Enumerable.Empty<ProjectModel>();
            if (_cachedVersion == _cacheVersion && _filteredCache != null) return _filteredCache;

            var query = _searchQuery?.Trim().ToLowerInvariant() ?? string.Empty;
            var result = _projects.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(query))
            {
                result = result.Where(p =>
                    (p.Title?.Contains(query, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (p.Description?.Contains(query, StringComparison.OrdinalIgnoreCase) ?? false));
            }

            if (!string.IsNullOrWhiteSpace(_activeTech))
            {
                result = result.Where(p =>
                    p.Technologies?.Any(t =>
                        t.Equals(_activeTech, StringComparison.OrdinalIgnoreCase)) == true);
            }

            _filteredCache = result.ToList();
            _cachedVersion = _cacheVersion;
            return _filteredCache;
        }
    }

    private List<string> allTechnologies => _allTechCache ?? [];

    private void ClearSearch()
    {
        _searchQuery = string.Empty;
        _cacheVersion++;
    }

    private void ClearFilters()
    {
        _searchQuery = string.Empty;
        _activeTech = null;
        _cacheVersion++;
        StateHasChanged();
    }

    private void SetActiveTech(string? tech)
    {
        _activeTech = _activeTech == tech ? null : tech;
        _cacheVersion++;
        StateHasChanged();
    }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            _content = await DataService.GetSiteContent(_cts.Token);
            Seo.SetSiteContent(_content);
            _projects = await DataService.GetProjects(_cts.Token);
            if (_projects != null)
            {
                _allTechCache = _projects
                    .Where(p => p.Technologies != null)
                    .SelectMany(p => p.Technologies)
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .OrderBy(t => t)
                    .ToList();
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to load projects");
        }
    }

    public void Dispose()
    {
        _cts.Cancel();
        _cts.Dispose();
        GC.SuppressFinalize(this);
    }
}
