namespace PersonalPortfolio.v1.Services;

public interface IDataService
{
    Task<List<ProjectModel>> GetProjects(CancellationToken ct = default);
    Task<List<ExperienceModel>> GetExperiences(CancellationToken ct = default);
    Task<List<SkillCategory>> GetSkills(CancellationToken ct = default);
    Task<List<BlogPostMeta>> GetBlogPosts(CancellationToken ct = default);
    Task<List<Certification>> GetCertifications(CancellationToken ct = default);
    Task<List<Testimonial>> GetTestimonials(CancellationToken ct = default);
    Task<SiteContent?> GetSiteContent(CancellationToken ct = default);
}
