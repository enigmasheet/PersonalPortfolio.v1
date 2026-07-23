namespace PersonalPortfolio.v1.Services;

public static class SiteConfig
{
    // Identity
#pragma warning disable S1075 // URIs should not be hardcoded (deliberate config)
    public const string SiteUrl = "https://abhaymandal.com.np";
#pragma warning restore S1075
    public static string BaseUrl => SiteUrl;
    public const string Email = "info@abhaymandal.com.np";
    public static string Domain => new Uri(SiteUrl).Host;

    // Data paths
    public const string DataPath = "data";
    public static string DataPath_Projects => $"{DataPath}/projects.json";
    public static string DataPath_Experience => $"{DataPath}/experience.json";
    public static string DataPath_Skills => $"{DataPath}/skills.json";
    public static string DataPath_BlogIndex => $"{DataPath}/blog.json";
    public static string DataPath_BlogPosts => $"{DataPath}/blog";
    public static string DataPath_Certifications => $"{DataPath}/certifications.json";
    public static string DataPath_Testimonials => $"{DataPath}/testimonials.json";
    public static string DataPath_SiteContent => $"{DataPath}/site-content.json";

    // GitHub
    public const string GitHubUsername = "enigmasheet";
    public static string GitHubUrl => $"https://github.com/{GitHubUsername}";
    public static string GitHubApiUrl => $"https://api.github.com/users/{GitHubUsername}";
    public static string GitHubOgImageUrl(string owner, string repo) =>
        $"https://opengraph.githubassets.com/1/{owner}/{repo}";

    // Social
    public static string LinkedInUrl => "https://www.linkedin.com/in/abhaykumarmandal/";
    public static string RoadmapUrl => "https://roadmap.sh/u/devabhay";

    // Resume
    public const string ResumePath = "/data/Abhay_Resume.pdf";

    // OG Image
    public const string OgImagePath = "/og-image.png";
    public static string OgImageUrl => $"{SiteUrl}{OgImagePath}";

    // GitHub Readme Stats Cards (tokyonight theme)
    public const string GitHubReadmeStatsBase = "https://github-readme-stats-sigma-five.vercel.app";
    public static string GitHubStatsCardUrl => $"{GitHubReadmeStatsBase}/api?username={GitHubUsername}&show_icons=true&theme=tokyonight&hide_border=true&count_private=true";
    public static string GitHubLanguagesCardUrl => $"{GitHubReadmeStatsBase}/api/top-langs/?username={GitHubUsername}&layout=compact&theme=tokyonight&hide_border=true&langs_count=6";

    // Formspree
    public const string FormspreeId = "xvgkrkwk";
    public static string FormspreeUrl => $"https://formspree.io/f/{FormspreeId}";
    public const string FormspreeHoneypotField = "_gotcha";
    public const string FormspreeTimestampField = "_timestamp";
    public const string FormspreeSubjectField = "_subject";

    // SEO defaults
    public const string TwitterCardType = "summary_large_image";
    public const string DefaultOgType = "website";
    public const string SchemaOrgContext = "https://schema.org";

    // Carousel
    public const int CarouselIntervalMs = 5000;

    // HTTP
    public const string UserAgent = "PersonalPortfolio/1.0";

    // Share URLs
    public static string TwitterShareUrl(string text, string url) =>
        $"https://twitter.com/intent/tweet?text={Uri.EscapeDataString(text)}&url={Uri.EscapeDataString(url)}";
    public static string LinkedInShareUrl(string url) =>
        $"https://www.linkedin.com/sharing/share-offsite/?url={Uri.EscapeDataString(url)}";

    // SVG fallback images
    public const string FallbackImageSvg_ProjectCard =
        "<svg xmlns='http://www.w3.org/2000/svg' width='400' height='250'>" +
        "<rect fill='var(--surface,%23f0f0f0)' width='400' height='250'/>" +
        "<text fill='var(--text-secondary,%23999)' font-family='sans-serif' font-size='16' " +
        "x='50%25' y='50%25' text-anchor='middle' dominant-baseline='middle'>No preview</text></svg>";
    public const string FallbackImageSvg_ProjectDetail =
        "<svg xmlns=%22http://www.w3.org/2000/svg%22 width=%22800%22 height=%22400%22>" +
        "<rect fill=%22%23f0f0f0%22 width=%22800%22 height=%22400%22/></svg>";

    // JS interop function names
    public const string JsGetTheme = "getTheme";
    public const string JsToggleTheme = "toggleTheme";
    public const string JsCloseCmdPalette = "closeCmdPalette";
    public const string JsObserveSkillBars = "observeSkillBars";
    public const string JsBlogProgressCleanup = "__blogProgressCleanup";

    // CSS bridge classes
    public const string CssNavOpen = "nav-open";
    public const string CssCmdSelected = "selected";
    public const string CssCmdOpen = "open";
    public const string CssSkillBarFill = ".skill-bar-fill";

    // Element IDs
    public const string NavToggleId = "nav-toggle";

    // Inline JS expressions
    public static string JsCloseDrawer => $"document.getElementById('{NavToggleId}').checked=false";
    public const string JsScrollToTop = "window.scrollTo({top:0,behavior:'smooth'})";

    // Utility CSS classes
    public const string CssTextCenter = "text-center";
    public const string CssVisuallyHidden = "visually-hidden";
    public const string CssNotFoundSection = "not-found-section";
    public const string CssNotFoundHeading = "not-found-heading";
    public const string CssNotFoundDesc = "not-found-desc";
    public const string CssContainerNarrow = "container-narrow";

    // Person JSON-LD knowsAbout
    public static readonly string[] PersonKnowsAbout = ["JavaScript", "Next.js", "React", "Node.js", ".NET", "Azure"];
}
