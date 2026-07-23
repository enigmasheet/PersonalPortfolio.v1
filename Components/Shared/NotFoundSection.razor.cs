namespace PersonalPortfolio.v1.Components.Shared;

public partial class NotFoundSection
{
    [Parameter] public string Heading { get; set; } = "404";
    [Parameter] public string SubHeading { get; set; } = "Page not found";
    [Parameter] public string Description { get; set; } = "The page you're looking for doesn't exist.";
    [Parameter] public string BackLink { get; set; } = "/";
    [Parameter] public string BackText { get; set; } = "Go Home";
    [Parameter] public bool IsError { get; set; }
    [Parameter] public bool ShowReload { get; set; }

    [Inject] private IJSRuntime JS { get; set; } = null!;

    private void Reload() => _ = JS.InvokeVoidAsync("location.reload");
}
