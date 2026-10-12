namespace MediaPager.App.PluginContracts;

/// <summary>A generic library metadata write requested by an installed plugin. Plugins that
/// ingest or transform media can use this without taking a dependency on host storage models.</summary>
public sealed record LibraryItemWriteRequest(
    int? CatalogItemId,
    int? CatalogTypeId,
    MediaKind? Kind,
    string Title,
    string? ExternalId = null,
    string? ImageUrl = null,
    string? BackdropUrl = null,
    string? Overview = null,
    int? Year = null,
    int? CatalogId = null)
{
    public MusicTrackMetadata? MusicMetadata { get; init; }
    public string? SeriesTitle { get; init; }
    public string? SeriesExternalId { get; init; }
    public string? SeriesImageUrl { get; init; }
    public string? SeriesBackdropUrl { get; init; }
    public string? SeriesOverview { get; init; }
    public int? SeriesYear { get; init; }
    public string? SeasonTitle { get; init; }
    public int? SeasonNumber { get; init; }
    public int? EpisodeNumber { get; init; }
}

public sealed record LibraryItemHandle(int? ItemId, bool Created);

public sealed record LibraryCatalogInfo(
    int Id,
    int CatalogTypeId,
    string CatalogTypeSlug,
    string Name,
    string? Path);

public interface ILibraryStore
{
    Task<LibraryCatalogInfo?> GetCatalogAsync(int catalogId, CancellationToken cancellationToken = default);

    Task<LibraryItemHandle> SaveAsync(LibraryItemWriteRequest request, CancellationToken cancellationToken = default);

    Task UpdateStoragePathAsync(int itemId, string path, CancellationToken cancellationToken = default);

    Task DeleteAsync(int itemId, CancellationToken cancellationToken = default);
}
