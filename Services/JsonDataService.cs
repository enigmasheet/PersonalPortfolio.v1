namespace PersonalPortfolio.v1.Services;

public class JsonDataService(HttpClient http) : IDataService
{
    private List<ProjectModel>? _projects;
    private List<ExperienceModel>? _experiences;
    private List<SkillCategory>? _skills;
    private List<BlogPostMeta>? _blogPosts;
    private List<Certification>? _certifications;
    private List<Testimonial>? _testimonials;
    private SiteContent? _siteContent;

    public async Task<List<ProjectModel>> GetProjects(CancellationToken ct = default) =>
        _projects ??= await http.GetFromJsonAsync<List<ProjectModel>>(SiteConfig.DataPath_Projects, ct) ?? [];

    public async Task<List<ExperienceModel>> GetExperiences(CancellationToken ct = default) =>
        _experiences ??= await http.GetFromJsonAsync<List<ExperienceModel>>(SiteConfig.DataPath_Experience, ct) ?? [];

    public async Task<List<SkillCategory>> GetSkills(CancellationToken ct = default) =>
        _skills ??= await http.GetFromJsonAsync<List<SkillCategory>>(SiteConfig.DataPath_Skills, ct) ?? [];

    public async Task<List<BlogPostMeta>> GetBlogPosts(CancellationToken ct = default) =>
        _blogPosts ??= await http.GetFromJsonAsync<List<BlogPostMeta>>(SiteConfig.DataPath_BlogIndex, ct) ?? [];

    public async Task<List<Certification>> GetCertifications(CancellationToken ct = default) =>
        _certifications ??= await http.GetFromJsonAsync<List<Certification>>(SiteConfig.DataPath_Certifications, ct) ?? [];

    public async Task<List<Testimonial>> GetTestimonials(CancellationToken ct = default) =>
        _testimonials ??= await http.GetFromJsonAsync<List<Testimonial>>(SiteConfig.DataPath_Testimonials, ct) ?? [];

    public async Task<SiteContent?> GetSiteContent(CancellationToken ct = default) =>
        _siteContent ??= await http.GetFromJsonAsync<SiteContent>(SiteConfig.DataPath_SiteContent, ct);
}
