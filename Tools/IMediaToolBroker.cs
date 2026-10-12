namespace MediaPager.App.PluginContracts;

public enum MediaTranscodeProfile
{
    CopyToMp4,
    CopyAudio,
    H264,
    H265,
    Av1,
    AacAudio,
    Mp3Audio,
    OpusAudio,
}

public sealed record MediaTransferRequest(
    Uri SourceUri,
    int CatalogId,
    MediaKind Kind,
    string Title,
    string? ExternalId = null,
    string? ImageUrl = null,
    string? BackdropUrl = null,
    string? Overview = null,
    int? Year = null,
    bool ScratchInCatalog = true,
    MediaTranscodeProfile Profile = MediaTranscodeProfile.CopyToMp4,
    string? JobId = null)
{
    public MusicTrackMetadata? MusicMetadata { get; init; }
}

public sealed record MediaToolProgress(double Progress, string Message);
public sealed record ResolvedStreamSource(Uri UpstreamUri, MediaKind Kind);

/// <summary>Host-owned media operations. Plugins request a typed operation and catalog id;
/// the host resolves destinations, owns process execution, and never returns raw paths.</summary>
public interface IMediaToolBroker
{
    Task<bool> IsDestinationAvailableAsync(int catalogId, string catalogTypeSlug, CancellationToken cancellationToken = default);

    Task<bool> HasOtherStreamItemActionAsync(MediaKind kind, CancellationToken cancellationToken = default);

    Task<ResolvedStreamSource?> ResolveStreamAsync(PluginActionContext context, CancellationToken cancellationToken = default);

    Task TransferAsync(
        MediaTransferRequest request,
        IProgress<MediaToolProgress>? progress = null,
        CancellationToken cancellationToken = default);
}
