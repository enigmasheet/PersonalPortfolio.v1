using PersonalPortfolio.v1.Services;
using Ganss.Xss;

namespace PersonalPortfolio.v1.Pages;

public partial class BlogPost : IAsyncDisposable
{
    [Parameter] public string Slug { get; set; } = string.Empty;

    [Inject] private IDataService DataService { get; set; } = null!;
    [Inject] private IJSRuntime JS { get; set; } = null!;
    [Inject] private HttpClient Http { get; set; } = null!;

    private static readonly HtmlSanitizer Sanitizer = new();

    private BlogPostMeta? _post;
    private string? _htmlContent;
    private bool _loading = true;
    private bool _notFound;
    private bool _error;
    private bool _linkCopied;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            var posts = await DataService.GetBlogPosts();
            _post = posts?.FirstOrDefault(p =>
                p.Slug?.Equals(Slug, StringComparison.OrdinalIgnoreCase) == true);

            if (_post == null)
            {
                _notFound = true;
                _loading = false;
                return;
            }

            if (!string.IsNullOrWhiteSpace(_post.FileName))
            {
                try
                {
                    var raw = await DataService.GetSiteContent() is not null
                        ? await Http.GetStringAsync($"{SiteConfig.DataPath_BlogPosts}/{_post.FileName}")
                        : throw new InvalidOperationException();
                    _htmlContent = Sanitizer.Sanitize(raw);
                }
                catch
                {
                    // Post content failed to load; show error state
                    _error = true;
                }
            }
        }
        catch
        {
            // Blog post not found or request failed
            _error = true;
        }
        _loading = false;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await using var module = await JS.InvokeAsync<IJSObjectReference>("import", "./Pages/BlogPost.razor.js");
            await module.InvokeVoidAsync("registerProgress");
        }
    }

    private async Task CopyLink()
    {
        try
        {
            await JS.InvokeVoidAsync("navigator.clipboard.writeText", $"{SiteConfig.BaseUrl}/blog/{Slug}");
            _linkCopied = true;
            StateHasChanged();
            await Task.Delay(3000);
            _linkCopied = false;
            StateHasChanged();
        }
        catch
        {
            // Clipboard API not available — ignore silently
        }
    }

    private void Reload() => _ = JS.InvokeVoidAsync("location.reload");

    public async ValueTask DisposeAsync()
    {
        try
        {
            await using var module = await JS.InvokeAsync<IJSObjectReference>("import", "./Pages/BlogPost.razor.js");
            await module.InvokeVoidAsync("cleanupProgress");
        }
        catch { }
    }
}
