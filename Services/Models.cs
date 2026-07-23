namespace PersonalPortfolio.v1.Services;

public record class ProjectModel
{
    public string Slug { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public List<string> ImageUrls { get; init; } = [];
    public string GitHubLink { get; init; } = string.Empty;
    public string LiveDemo { get; init; } = string.Empty;
    public bool LiveDemoActive { get; init; }
    public List<string> Technologies { get; init; } = [];
    public List<string> Tags { get; init; } = [];
}

public record class ExperienceModel
{
    public string? Title { get; init; }
    public string? Company { get; init; }
    public string? Description { get; init; }
    public string? Link { get; init; }
    public DateTime Date { get; init; }
    public DateTime? EndDate { get; init; }
}

public record class SkillCategory
{
    public string Category { get; init; } = string.Empty;
    public List<string> Skills { get; init; } = [];
    public int Level { get; init; }
}

public record class BlogPostMeta
{
    public string Slug { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public DateTime Date { get; init; }
    public string Summary { get; init; } = string.Empty;
    public List<string> Tags { get; init; } = [];
    public string FileName { get; init; } = string.Empty;
}

public record class Certification
{
    public string Title { get; init; } = string.Empty;
    public string Issuer { get; init; } = string.Empty;
    public string Link { get; init; } = string.Empty;
}

public record class Testimonial
{
    public string Name { get; init; } = string.Empty;
    public string Role { get; init; } = string.Empty;
    public string Text { get; init; } = string.Empty;
}
