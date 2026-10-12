namespace MediaPager.App.PluginContracts;

/// <summary>A grid/list item — enough to render a poster card and open a detail sheet.</summary>
public sealed record BrowseItem(
    string ExternalId,
    string Title,
    MediaKind Kind,
    int? Year = null,
    string? Overview = null,
    string? ArtworkUrl = null,
    string? BackdropUrl = null,
    double? VoteAverage = null)
{
    public IReadOnlyList<ContentMetadata>? Metadata { get; init; }
}

/// <summary>A generic labeled value for provider-specific content details.</summary>
public sealed record ContentMetadata(string Label, string Value);

public sealed record EpisodeInfo(
    int EpisodeNumber,
    string? Name = null,
    string? Overview = null,
    string? StillUrl = null);

public sealed record SeasonInfo(
    int SeasonNumber,
    string? Name = null,
    string? Overview = null,
    string? PosterUrl = null,
    IReadOnlyList<EpisodeInfo>? Episodes = null);

/// <summary>A cast/crew member. Role is the played character or the job ("Director", "Writer").</summary>
public sealed record CreditRole(string Name, string Role, string? ProfileUrl = null);

/// <summary>Full detail-sheet payload for a title (may include seasons/episodes for TV).</summary>
public sealed record TitleDetails(
    string ExternalId,
    MediaKind Kind,
    string Title,
    string? Overview = null,
    int? Year = null,
    string? OriginalTitle = null,
    string? SortTitle = null,
    string? ContentRating = null,
    double? Rating = null,
    DateTimeOffset? OriginalAvailableAt = null,
    string? ArtworkUrl = null,
    string? BackdropUrl = null,
    IReadOnlyList<CreditRole>? Credits = null,
    IReadOnlyList<SeasonInfo>? Seasons = null)
{
    public IReadOnlyList<ContentMetadata>? Metadata { get; init; }
}

/// <summary>Canonical metadata for backfilling a library entry (for example, during library ingestion).</summary>
public sealed record EnrichmentData(
    string Title,
    string? Overview = null,
    int? Year = null,
    string? OriginalTitle = null,
    string? ContentRating = null,
    double? Rating = null,
    DateTimeOffset? OriginalAvailableAt = null,
    string? ArtworkUrl = null,
    string? BackdropUrl = null,
    IReadOnlyList<string>? Genres = null,
    IReadOnlyList<CreditRole>? Credits = null)
{
    public MusicTrackMetadata? Music { get; init; }
}
