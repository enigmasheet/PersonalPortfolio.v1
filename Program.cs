using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using PersonalPortfolio.v1.Layout;
using PersonalPortfolio.v1.Services;
using System.Diagnostics.CodeAnalysis;

namespace PersonalPortfolio.v1;

public class Program
{
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(MainLayout))]
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        builder.Logging.SetMinimumLevel(LogLevel.Warning);

        var baseAddress = new Uri(builder.HostEnvironment.BaseAddress);
        builder.Services.AddScoped(sp =>
        {
            var client = new HttpClient { BaseAddress = baseAddress };
            client.DefaultRequestHeaders.UserAgent.ParseAdd(SiteConfig.UserAgent);
            return client;
        });

        builder.Services.AddMudServices();
        builder.Services.AddScoped<IDataService, JsonDataService>();
        builder.Services.AddScoped<ThemeService>();
        builder.Services.AddScoped<SeoService>();

        await builder.Build().RunAsync();
    }
}
