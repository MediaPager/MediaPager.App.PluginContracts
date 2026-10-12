namespace MediaPager.App.PluginContracts;

/// <summary>Music lookup inputs known by a source provider before a download begins.</summary>
public sealed record MusicTrackLookupRequest(
    string Title,
    IReadOnlyList<MusicArtistCredit> Artists,
    string? Album = null,
    string? Isrc = null);

/// <summary>Optional metadata capability for resolving a known music track to canonical
/// recording/release metadata. This is a text/identifier lookup, not an audio upload API.</summary>
public interface IMusicMetadataProviderPlugin : IMetadataProviderPlugin
{
    Task<EnrichmentData?> EnrichTrackAsync(
        MusicTrackLookupRequest request,
        CancellationToken cancellationToken);
}
