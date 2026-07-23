using PersonalPortfolio.v1.Services;

namespace PersonalPortfolio.v1.Components;

public partial class Hero : IDisposable
{
    [Inject] private IDataService DataService { get; set; } = null!;


    private SiteContent? _content;
    private string _phrases = "Backend Software Developer|Product-Oriented Technologist|Problem Solver";
    private string _firstPhrase = "Backend Software Developer";
    private readonly CancellationTokenSource _cts = new();

    protected override async Task OnInitializedAsync()
    {
        try
        {
            _content = await DataService.GetSiteContent(_cts.Token);
            if (_content?.Site is { } site)
            {
                var parts = new List<string>();
                if (!string.IsNullOrWhiteSpace(site.Title)) parts.Add(site.Title);
                if (!string.IsNullOrWhiteSpace(site.ShortTitle)) parts.Add(site.ShortTitle);
                if (!string.IsNullOrWhiteSpace(site.JobTitle)) parts.Add(site.JobTitle);
                if (site.KnowsAbout?.Count > 0) parts.AddRange(site.KnowsAbout.Take(3));
                if (parts.Count > 0) { _phrases = string.Join("|", parts); _firstPhrase = parts[0]; }
            }
        }
        catch
        {
            // Site content will use defaults
        }
    }

    public void Dispose()
    {
        _cts.Cancel();
        _cts.Dispose();
        GC.SuppressFinalize(this);
    }
}
