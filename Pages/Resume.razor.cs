using PersonalPortfolio.v1.Services;

namespace PersonalPortfolio.v1.Pages;

public partial class Resume : IDisposable
{
    private SiteContent? _content;
    private List<ExperienceModel>? _experiences;
    private List<SkillCategory>? _skills;
    private bool _dataLoaded;
    private readonly CancellationTokenSource _cts = new();
    private string _resumeDescription = "Resume of Abhay Kumar Mandal — experience, education, and skills.";

    [Inject] private IDataService DataService { get; set; } = null!;
    [Inject] private SeoService Seo { get; set; } = null!;
    [Inject] private IJSRuntime JS { get; set; } = null!;
    [Inject] private ILogger<Resume> Logger { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        if (_dataLoaded) return;
        _dataLoaded = true;
        try
        {
            _content = await DataService.GetSiteContent(_cts.Token);
            Seo.SetSiteContent(_content);
            _resumeDescription = $"Resume of {_content?.Site?.FullName ?? "Abhay Kumar Mandal"} — experience, education, and skills.";
            _experiences = await DataService.GetExperiences(_cts.Token);
            _skills = await DataService.GetSkills(_cts.Token);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to load resume data");
        }
    }

    private void Print() => _ = JS.InvokeVoidAsync("window.print");

    public void Dispose()
    {
        _cts.Cancel();
        _cts.Dispose();
        GC.SuppressFinalize(this);
    }
}
