using PersonalPortfolio.v1.Services;

namespace PersonalPortfolio.v1;

public partial class App : IDisposable
{
    private bool _isDark;

    [Inject] private IJSRuntime JS { get; set; } = null!;
    [Inject] private ThemeService Theme { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            var stored = await JS.InvokeAsync<string?>("getLocalStorage", "theme");
            _isDark = stored switch
            {
                "dark" => true,
                "light" => false,
                _ => true
            };
        }
        catch
        {
            // Default to dark mode on error
            _isDark = true;
        }
        Theme.IsDark = _isDark;
        Theme.ThemeChanged += OnThemeChanged;
    }

    private async void OnThemeChanged(bool dark)
    {
        _isDark = dark;
        StateHasChanged();
        try { await JS.InvokeVoidAsync("setLocalStorage", "theme", dark ? "dark" : "light"); }
        catch
        {
            // Ignore localStorage errors
        }
    }

    public void Dispose()
    {
        Theme.ThemeChanged -= OnThemeChanged;
        GC.SuppressFinalize(this);
    }
}
