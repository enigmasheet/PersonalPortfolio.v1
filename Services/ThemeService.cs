namespace PersonalPortfolio.v1.Services;

public sealed class ThemeService
{
    private bool _isDark;

    public bool IsDark
    {
        get => _isDark;
        set
        {
            if (_isDark == value) return;
            _isDark = value;
            ThemeChanged?.Invoke(value);
        }
    }

    public bool IsLight => !_isDark;
    public event Action<bool>? ThemeChanged;

    public void Toggle()
    {
        IsDark = !IsDark;
    }

    public void SetDark(bool dark) => IsDark = dark;
}
