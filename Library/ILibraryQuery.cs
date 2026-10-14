namespace MediaPager.App.PluginContracts;

/// <summary>A local-library search result row: the host data layer projected into a
/// plugin-safe shape (no host types leak into plugin code).</summary>
public sealed record LibraryItemHit(
    string ItemId,
    MediaKind Kind,
    string Title,
    int? Year = null,
    double? Rating = null,
    string? Overview = null,
    string? ArtworkUrl = null,
    int? CatalogItemId = null,
    int? CatalogId = null,
    string? ExternalId = null);

/// <summary>Read-only facade over the host's media library. Host-implemented (Core) and
/// registered as a singleton in the host's DI container so plugins can query local items
/// without referencing the host's data layer.</summary>
public interface ILibraryQuery
{
    Task<IReadOnlyList<LibraryItemHit>> SearchAsync(string query, int limit, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<LibraryItemHit>> SearchAsync(
        string query,
        int limit,
        string? mediaTypeSlug,
        string? catalogTypeSlug,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<LibraryItemHit>> SearchAsync(
        string query,
        int limit,
        string? mediaTypeSlug,
        string? catalogTypeSlug,
        int? catalogId,
        CancellationToken cancellationToken = default);
}
