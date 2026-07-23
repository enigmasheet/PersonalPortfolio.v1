using PersonalPortfolio.v1.Services;

namespace PersonalPortfolio.v1.Components;

public partial class CommandPalette : IDisposable
{
    private bool _isOpen;
    private string _query = string.Empty;
    private ElementReference _searchInput;
    private List<IGrouping<string, SearchItem>> _results = [];
    private SearchItem? _selectedItem;
    private List<ProjectModel>? _projects;
    private List<BlogPostMeta>? _posts;
    private bool _dataLoaded;
    private CancellationTokenSource? _debounceCts;
    private IJSObjectReference? _module;

    private sealed record SearchItem(string Title, string Subtitle, string Url);

    [Inject] private IDataService DataService { get; set; } = null!;
    [Inject] private NavigationManager Navigation { get; set; } = null!;
    [Inject] private IJSRuntime JS { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        Navigation.LocationChanged += OnLocationChanged;
        if (!_dataLoaded)
        {
            _dataLoaded = true;
            try
            {
                _projects = await DataService.GetProjects();
                _posts = await DataService.GetBlogPosts();
            }
            catch
            {
                // Data will be null; search just returns empty
            }
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _module = await JS.InvokeAsync<IJSObjectReference>("import", "./Components/CommandPalette.razor.js");
            await _module.InvokeVoidAsync("register", DotNetObjectReference.Create(this));
        }
    }

    [JSInvokable]
    public async Task Toggle()
    {
        _isOpen = !_isOpen;
        StateHasChanged();
        if (_isOpen)
        {
            await Task.Delay(100);
            await _searchInput.FocusAsync();
        }
    }

    private void OnLocationChanged(object? sender, LocationChangedEventArgs e)
    {
        _isOpen = false;
        StateHasChanged();
    }

    private void HandleKey(KeyboardEventArgs e)
    {
        if (e.Key == "Escape")
        {
            _isOpen = false;
            StateHasChanged();
            return;
        }

        if (e.Key == "ArrowDown")
        {
            SelectNext();
            _ = ScrollSelectedIntoView();
            return;
        }

        if (e.Key == "ArrowUp")
        {
            SelectPrevious();
            _ = ScrollSelectedIntoView();
            return;
        }

        if (e.Key == "Enter" && _selectedItem != null)
        {
            NavigateTo(_selectedItem.Url);
            return;
        }

        _debounceCts?.Cancel();
        _debounceCts = new CancellationTokenSource();
        var token = _debounceCts.Token;
        _ = Task.Run(async () =>
        {
            try
            {
                await Task.Delay(200, token);
                if (!token.IsCancellationRequested)
                    await InvokeAsync(UpdateResults);
            }
            catch (TaskCanceledException)
            {
            }
        }, token);
    }

    private void UpdateResults()
    {
        var q = _query?.Trim() ?? string.Empty;
        var items = new List<SearchItem>();

        if (_projects is not null)
        {
            items.AddRange(_projects
                .Where(p => string.IsNullOrWhiteSpace(q) ||
                            (p.Title?.Contains(q, StringComparison.OrdinalIgnoreCase) ?? false) ||
                            (p.Description?.Contains(q, StringComparison.OrdinalIgnoreCase) ?? false))
                .Select(p => new SearchItem(
                    p.Title ?? "Untitled",
                    p.Description ?? "",
                    $"/projects/{p.Slug}")));
        }

        if (_posts is not null)
        {
            items.AddRange(_posts
                .Where(p => string.IsNullOrWhiteSpace(q) ||
                            (p.Title?.Contains(q, StringComparison.OrdinalIgnoreCase) ?? false) ||
                            (p.Summary?.Contains(q, StringComparison.OrdinalIgnoreCase) ?? false))
                .Select(p => new SearchItem(
                    p.Title ?? "Untitled",
                    p.Summary ?? "",
                    $"/blog/{p.Slug}")));
        }

        _selectedItem = items.FirstOrDefault();

        _results =
        [
            ..items.GroupBy(i => i.Url.StartsWith("/projects")
            ? "Projects"
            : "Blog Posts")
        ];
    }
    private void SelectItem(SearchItem item) => _selectedItem = item;

    private void SelectNext()
    {
        var all = _results.SelectMany(g => g).ToList();
        var idx = all.IndexOf(_selectedItem!);
        _selectedItem = all[(idx + 1) % all.Count];
    }

    private void SelectPrevious()
    {
        var all = _results.SelectMany(g => g).ToList();
        var idx = all.IndexOf(_selectedItem!);
        _selectedItem = all[(idx - 1 + all.Count) % all.Count];
    }

    private async Task ScrollSelectedIntoView()
    {
        if (_module is not null)
            await _module.InvokeVoidAsync("scrollItemIntoView", ".cmd-item.selected");
    }

    private void NavigateTo(string url)
    {
        _isOpen = false;
        _query = string.Empty;
        Navigation.NavigateTo(url);
    }

    public async void Dispose()
    {
        Navigation.LocationChanged -= OnLocationChanged;
        _debounceCts?.Cancel();
        _debounceCts?.Dispose();
        if (_module is not null)
        {
            await _module.InvokeVoidAsync("cleanup");
            await _module.DisposeAsync();
        }
        GC.SuppressFinalize(this);
    }
}
