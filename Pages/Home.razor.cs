using PersonalPortfolio.v1.Services;

namespace PersonalPortfolio.v1.Pages;

public partial class Home : IAsyncDisposable
{
    private SiteContent? _content;
    private string personJson = "{}";
    private string seoTitle = "Abhay Kumar Mandal — Backend Software Developer";
    private string seoDescription = "Portfolio of Abhay Kumar Mandal — a results-driven Backend Software Developer specializing in JavaScript, Next.js, and cloud technologies.";
    private List<Testimonial>? _testimonials;
    private bool _dataLoaded;
    private readonly CancellationTokenSource _cts = new();
    private IJSObjectReference? _module;

    private bool _showAbout;
    private bool _showExperience;
    private bool _showProjects;
    private bool _showBlog;
    private bool _showContact;
    private bool _showResume;

    [Inject] private IDataService DataService { get; set; } = null!;
    [Inject] private SeoService Seo { get; set; } = null!;
    [Inject] private ILogger<Home> Logger { get; set; } = null!;
    [Inject] private IJSRuntime JS { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        if (!_dataLoaded)
        {
            _dataLoaded = true;
            try
            {
                _content = await DataService.GetSiteContent(_cts.Token);
                Seo.SetSiteContent(_content);
                _testimonials = await DataService.GetTestimonials(_cts.Token);

                var site = _content?.Site;
                if (site != null)
                {
                    seoTitle = $"{site.FullName} — {site.ShortTitle}";
                    seoDescription = $"Portfolio of {site.FullName} — a results-driven {site.ShortTitle} specializing in JavaScript, Next.js, and cloud technologies.";
                    personJson = $$"""
                        {
                            "@context": "{{SiteConfig.SchemaOrgContext}}",
                            "@type": "Person",
                            "name": "{{site.FullName}}",
                            "url": "{{SiteConfig.SiteUrl}}/",
                            "jobTitle": "{{site.JobTitle}}",
                            "knowsAbout": {{System.Text.Json.JsonSerializer.Serialize(site.KnowsAbout)}},
                            "sameAs": [
                                "{{SiteConfig.GitHubUrl}}",
                                "{{SiteConfig.LinkedInUrl}}",
                                "{{SiteConfig.RoadmapUrl}}"
                            ]
                        }
                        """;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to load site content");
                _testimonials = [];
            }
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _module = await JS.InvokeAsync<IJSObjectReference>("import", "./Pages/Home.razor.js");
            await _module.InvokeVoidAsync("observeLazySections", DotNetObjectReference.Create(this));
        }
    }

    [JSInvokable]
    public void ShowSection(string id)
    {
        switch (id)
        {
            case "about": _showAbout = true; break;
            case "experience": _showExperience = true; break;
            case "projects": _showProjects = true; break;
            case "blog": _showBlog = true; break;
            case "contact": _showContact = true; break;
            case "resume": _showResume = true; break;
        }
        StateHasChanged();
    }

    public async ValueTask DisposeAsync()
    {
        _cts.Cancel();
        _cts.Dispose();
        if (_module is not null)
        {
            await _module.InvokeVoidAsync("cleanup");
            await _module.DisposeAsync();
        }
    }
}
