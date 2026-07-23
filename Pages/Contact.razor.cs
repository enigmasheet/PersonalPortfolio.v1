using PersonalPortfolio.v1.Services;

namespace PersonalPortfolio.v1.Pages;

public partial class Contact : IDisposable
{
    private SiteContent? _content;
    private readonly CancellationTokenSource _cts = new();
    private ContactFormModel _form = new();
    private string _honeypot = string.Empty;
    private bool _submitting;
    private bool _submitted;
    private bool _formValid;
    private MudForm? _formRef;

    [Inject] private IDataService DataService { get; set; } = null!;
    [Inject] private SeoService Seo { get; set; } = null!;
    [Inject] private HttpClient Http { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [Inject] private ILogger<Contact> Logger { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            _content = await DataService.GetSiteContent(_cts.Token);
            Seo.SetSiteContent(_content);
        }
        catch
        {
            // Site content will use defaults
        }
    }

    public void Dispose()
    {
        _cts.Cancel();
        _cts.Dispose();
        GC.SuppressFinalize(this);
    }

    private async Task HandleSubmit()
    {
        if (!string.IsNullOrEmpty(_honeypot)) return;

        _submitting = true;

        try
        {
            var formData = new Dictionary<string, string>
            {
                ["name"] = _form.Name,
                ["email"] = _form.Email,
                ["message"] = _form.Message,
                [SiteConfig.FormspreeSubjectField] = $"Contact from {_form.Name}"
            };

            using var response = await Http.PostAsJsonAsync(SiteConfig.FormspreeUrl, formData);

            if (response.IsSuccessStatusCode)
            {
                _submitted = true;
            }
            else
            {
                Snackbar.Add("Server rejected the submission. Please try again or email me directly.", Severity.Error);
            }
        }
        catch (HttpRequestException)
        {
            Snackbar.Add("Could not reach the server. Please check your connection or email me directly.", Severity.Error);
        }
        catch (Exception ex)
        {
            Snackbar.Add("Something went wrong. Please try again or email me directly.", Severity.Error);
            Logger.LogError(ex, "Form submission failed");
        }
        finally
        {
            _submitting = false;
        }
    }

    private void ResetForm()
    {
        _form = new();
        _submitting = false;
        _submitted = false;
        _honeypot = string.Empty;
    }
}
