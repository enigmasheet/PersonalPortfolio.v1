using PersonalPortfolio.v1.Services;

namespace PersonalPortfolio.v1.Pages;

public partial class ProjectDetail
{
    [Parameter] public string Slug { get; set; } = string.Empty;

    [Inject] private IDataService DataService { get; set; } = null!;

    private ProjectModel? _project;
    private bool _loading = true;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            var projects = await DataService.GetProjects();
            _project = projects?.FirstOrDefault(p =>
                p.Slug?.Equals(Slug, StringComparison.OrdinalIgnoreCase) == true);
        }
        catch
        {
            // Project not found — _project stays null, showing not-found UI
        }
        _loading = false;
    }
}
