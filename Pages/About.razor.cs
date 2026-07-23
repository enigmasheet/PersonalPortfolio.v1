using PersonalPortfolio.v1.Services;

namespace PersonalPortfolio.v1.Pages;

public partial class About : IDisposable
{
    private SiteContent? _content;
    private List<SkillCategory>? _skills;
    private List<Certification>? _certifications;
    private List<Testimonial>? _testimonials;
    private bool _dataLoaded;
    private readonly CancellationTokenSource _cts = new();

    [Inject] private IDataService DataService { get; set; } = null!;
    [Inject] private SeoService Seo { get; set; } = null!;
    [Inject] private IJSRuntime JS { get; set; } = null!;
    [Inject] private ILogger<About> Logger { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        if (_dataLoaded) return;
        _dataLoaded = true;
        try
        {
            _content = await DataService.GetSiteContent(_cts.Token);
            Seo.SetSiteContent(_content);
            _skills = await DataService.GetSkills(_cts.Token);
            _certifications = await DataService.GetCertifications(_cts.Token);
            _testimonials = await DataService.GetTestimonials(_cts.Token);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to load About data");
        }
    }

    public void Dispose()
    {
        _cts.Cancel();
        _cts.Dispose();
        GC.SuppressFinalize(this);
    }
}
