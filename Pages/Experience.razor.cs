using PersonalPortfolio.v1.Services;

namespace PersonalPortfolio.v1.Pages;

public partial class Experience : IDisposable
{
    private SiteContent? _content;
    private List<ExperienceModel>? _experiences;
    private readonly CancellationTokenSource _cts = new();

    [Inject] private IDataService DataService { get; set; } = null!;
    [Inject] private SeoService Seo { get; set; } = null!;
    [Inject] private ILogger<Experience> Logger { get; set; } = null!;

    private List<ExperienceModel> orderedExperiences =>
        _experiences?.OrderByDescending(e => e.Date).ToList() ?? [];

    protected override async Task OnInitializedAsync()
    {
        try
        {
            _content = await DataService.GetSiteContent(_cts.Token);
            Seo.SetSiteContent(_content);
            _experiences = await DataService.GetExperiences(_cts.Token);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to load experiences");
        }
    }

    public void Dispose()
    {
        _cts.Cancel();
        _cts.Dispose();
        GC.SuppressFinalize(this);
    }
}
