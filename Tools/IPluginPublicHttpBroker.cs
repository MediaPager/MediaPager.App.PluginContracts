namespace MediaPager.App.PluginContracts;

/// <summary>A bounded response fetched by the host on behalf of one plugin.</summary>
public sealed record PluginPublicHttpResponse(byte[] Content, string? ContentType, Uri FinalUri);

/// <summary>Optional capability for fetching bounded public HTTPS documents. The host validates
/// every redirect and destination; this is not a general proxy and cannot fetch private hosts.</summary>
public interface IPluginPublicHttpBroker
{
    Task<PluginPublicHttpResponse> GetAsync(Uri uri, CancellationToken cancellationToken = default);
}
