namespace PersonalPortfolio.v1.Services;

public record class SiteContent
{
    public SiteInfo? Site { get; init; }
    public NavContent? Nav { get; init; }
    public HeroContent? Hero { get; init; }
    public Dictionary<string, SectionHeader>? Sections { get; init; }
    public AboutContent? About { get; init; }
    public ResumeContent? Resume { get; init; }
    public ContactContent? Contact { get; init; }
    public FooterContent? Footer { get; init; }
    public ProjectDetailContent? ProjectDetail { get; init; }
    public BlogPostContent? BlogPost { get; init; }
}

public record class SiteInfo
{
    public string Name { get; init; } = string.Empty;
    public string FullName { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public string ShortTitle { get; init; } = string.Empty;
    public string JobTitle { get; init; } = string.Empty;
    public List<string> KnowsAbout { get; init; } = [];
}

public record class NavContent
{
    public List<string> Links { get; init; } = [];
}

public record class HeroContent
{
    public string Greeting { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string PrimaryCta { get; init; } = string.Empty;
    public string SecondaryCta { get; init; } = string.Empty;
}

public record class SectionHeader
{
    public string Label { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string? Cta { get; init; }
}

public record class AboutContent
{
    public List<string> Paragraphs { get; init; } = [];
    public AboutBlockTitles? BlockTitles { get; init; }
    public List<Achievement> Achievements { get; init; } = [];
    public List<StatItem> Stats { get; init; } = [];
    public string Quote { get; init; } = string.Empty;
}

public record class AboutBlockTitles
{
    public string Skills { get; init; } = string.Empty;
    public string Certifications { get; init; } = string.Empty;
    public string Achievements { get; init; } = string.Empty;
    public string Testimonials { get; init; } = string.Empty;
}

public record class Achievement
{
    public string Year { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
}

public record class StatItem
{
    public string Label { get; init; } = string.Empty;
    public int Target { get; init; }
    public string Suffix { get; init; } = string.Empty;
}

public record class ResumeContent
{
    public string Location { get; init; } = string.Empty;
    public List<EducationItem> Education { get; init; } = [];
}

public record class EducationItem
{
    public string Degree { get; init; } = string.Empty;
    public string School { get; init; } = string.Empty;
    public string Dates { get; init; } = string.Empty;
}

public record class ContactContent
{
    public string Intro { get; init; } = string.Empty;
}

public record class FooterContent
{
    public string Copyright { get; init; } = string.Empty;
}

public record class ProjectDetailContent
{
    public string BackLink { get; init; } = string.Empty;
    public string TechnologiesLabel { get; init; } = string.Empty;
    public string TagsLabel { get; init; } = string.Empty;
    public string LinksLabel { get; init; } = string.Empty;
    public string ViewSource { get; init; } = string.Empty;
    public string LiveDemo { get; init; } = string.Empty;
    public string Offline { get; init; } = string.Empty;
    public string NotFoundHeading { get; init; } = string.Empty;
    public string NotFoundDescription { get; init; } = string.Empty;
}

public record class BlogPostContent
{
    public string BackLink { get; init; } = string.Empty;
    public string ShareLabel { get; init; } = string.Empty;
    public string LinkCopied { get; init; } = string.Empty;
    public string NotFoundHeading { get; init; } = string.Empty;
    public string NotFoundDescription { get; init; } = string.Empty;
    public string TagsLabel { get; init; } = string.Empty;
    public string ContentError { get; init; } = string.Empty;
}
