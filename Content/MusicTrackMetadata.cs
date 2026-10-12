namespace MediaPager.App.PluginContracts;

/// <summary>An artist credit attached to a music release or recording.</summary>
public sealed record MusicArtistCredit(string Name, string? MusicBrainzId = null);

/// <summary>Structured music metadata shared by providers, plugin actions, and the host
/// media-transfer pipeline.</summary>
public sealed record MusicTrackMetadata(
    string? MusicBrainzRecordingId = null,
    string? MusicBrainzReleaseId = null,
    string? Isrc = null,
    string? Album = null,
    IReadOnlyList<MusicArtistCredit>? Artists = null,
    IReadOnlyList<MusicArtistCredit>? AlbumArtists = null,
    int? TrackNumber = null,
    int? TrackCount = null,
    int? DiscNumber = null,
    int? DiscCount = null,
    int? Year = null,
    IReadOnlyList<string>? Genres = null);
