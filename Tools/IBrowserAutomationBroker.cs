namespace MediaPager.App.PluginContracts;

public sealed record BrowserAutomationRequest(
    Uri PageUri,
    string UserAgent,
    IReadOnlyList<string> BlockedUrlSubstrings,
    string RequestUrlContains,
    string? ClickSelector = null,
    int TimeoutSeconds = 30);

/// <summary>Host-owned public web fetch and browser automation operations. The plugin asks
/// for a URL match; it never receives Playwright objects, a browser process, or executable paths.</summary>
public interface IBrowserAutomationBroker
{
    Task<string> GetPublicTextAsync(Uri uri, CancellationToken cancellationToken = default);

    /// <summary>Fetch public text with optional request headers the upstream requires.
    /// The default implementation ignores them so existing brokers keep working.</summary>
    Task<string> GetPublicTextAsync(
        Uri uri,
        IReadOnlyDictionary<string, string>? requestHeaders,
        CancellationToken cancellationToken = default) =>
        GetPublicTextAsync(uri, cancellationToken);

    Task<string?> NavigateAndWaitForRequestAsync(
        BrowserAutomationRequest request,
        CancellationToken cancellationToken = default);
}
