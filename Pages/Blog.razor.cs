using PersonalPortfolio.v1.Services;

namespace PersonalPortfolio.v1.Pages;

public partial class Blog : IDisposable
{
    private SiteContent? _content;
    private List<BlogPostMeta>? _posts;
    private string? _activeTag;
    private List<BlogPostMeta>? _filteredCache;
    private List<string>? _allTagsCache;
    private int _cacheVersion;
    private int _cachedVersion = -1;
    private readonly CancellationTokenSource _cts = new();

    [Inject] private IDataService DataService { get; set; } = null!;
    [Inject] private SeoService Seo { get; set; } = null!;
    [Inject] private ILogger<Blog> Logger { get; set; } = null!;

    private List<BlogPostMeta> filteredPosts
    {
        get
        {
            if (_posts == null) return [];
            if (_cachedVersion == _cacheVersion && _filteredCache != null) return _filteredCache;
            var result = string.IsNullOrWhiteSpace(_activeTag)
                ? _posts
                : _posts.Where(p => p.Tags?.Contains(_activeTag, StringComparer.OrdinalIgnoreCase) == true);
            _filteredCache = result.OrderByDescending(p => p.Date).ToList();
            _cachedVersion = _cacheVersion;
            return _filteredCache;
        }
    }

    private List<string> allTags => _allTagsCache ?? [];

    private void SetActiveTag(string? tag)
    {
        _activeTag = _activeTag == tag ? null : tag;
        _cacheVersion++;
    }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            _content = await DataService.GetSiteContent(_cts.Token);
            Seo.SetSiteContent(_content);
            _posts = await DataService.GetBlogPosts(_cts.Token);
            if (_posts != null)
            {
                _allTagsCache = _posts
                    .Where(p => p.Tags != null)
                    .SelectMany(p => p.Tags)
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .OrderBy(t => t)
                    .ToList();
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to load blog posts");
        }
    }

    public void Dispose()
    {
        _cts.Cancel();
        _cts.Dispose();
        GC.SuppressFinalize(this);
    }
}
