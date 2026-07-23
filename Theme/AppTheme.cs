namespace PersonalPortfolio.v1.Theme;

public static class AppTheme
{
    private const string NearBlack = "#0a0a0a";
    private const string NearWhite = "#f5f5f5";
    private const string LightBorder = "#e5e5e5";
    private const string DarkBorder = "#2a2a2a";

    public static MudTheme Theme { get; } = new()
    {
        PaletteLight = new PaletteLight
        {
            Primary = "#5a52e0",
            PrimaryDarken = "#4a42d0",
            PrimaryLighten = "#7b75e8",
            Secondary = "#7c4dff",
            Tertiary = "#2ecc71",
            Info = "#2196F3",
            Success = "#2ecc71",
            Warning = "#FF9800",
            Error = "#e74c3c",
            Dark = NearBlack,
            Surface = "#ffffff",
            Background = "#fafafa",
            AppbarBackground = "#fafafad9",
            DrawerBackground = "#ffffff",
            TextPrimary = NearBlack,
            TextSecondary = "#595959",
            Divider = LightBorder,
            DrawerText = NearBlack,
            AppbarText = NearBlack,
            TableLines = LightBorder,
            LinesDefault = LightBorder,
        },
        PaletteDark = new PaletteDark
        {
            Primary = "#8b83ff",
            PrimaryDarken = "#a39cff",
            PrimaryLighten = "#736aff",
            Secondary = "#a082ff",
            Tertiary = "#3ddc84",
            Info = "#64B5F6",
            Success = "#3ddc84",
            Warning = "#FFB74D",
            Error = "#e74c3c",
            Dark = NearWhite,
            Surface = "#1a1a1a",
            Background = NearBlack,
            AppbarBackground = $"{NearBlack}d9",
            DrawerBackground = "#1a1a1a",
            TextPrimary = NearWhite,
            TextSecondary = "#999999",
            Divider = DarkBorder,
            DrawerText = NearWhite,
            AppbarText = NearWhite,
            TableLines = DarkBorder,
            LinesDefault = DarkBorder,
        },
        Typography = new Typography
        {
            Default = new DefaultTypography
            {
                FontFamily = ["Inter", "-apple-system", "BlinkMacSystemFont", "Segoe UI", "sans-serif"],
                FontSize = "1rem",
                FontWeight = "400",
                LineHeight = "1.6",
            },
        },
        LayoutProperties = new LayoutProperties
        {
            DefaultBorderRadius = "8px",
            AppbarHeight = "64px",
        },
    };
}
