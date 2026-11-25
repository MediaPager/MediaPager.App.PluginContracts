namespace MediaPager.App.PluginContracts;

/// <summary>Read-only catalog metadata needed by a filesystem reconciliation agent.</summary>
public sealed record CatalogScanCatalogInfo(
    int Id,
    int CatalogTypeId,
    string CatalogTypeSlug,
    string Name,
    string? Path);

public sealed record CatalogScanItemInfo(
    int Id,
    int CatalogId,
    string Kind,
    string Title,
    string? StoragePath,
    long? SizeBytes,
    string? ExternalId,
    string? Fingerprint,
    double? Confidence,
    string? ScanSignature,
    DateTimeOffset? LastScannedAt,
    DateTimeOffset? LastSeenAt);

public sealed record CatalogScanTag(string Type, string Value);

public sealed record CatalogScanMetadataUpdate(
    string? Title = null,
    int? Year = null,
    string? Overview = null,
    string? ImageUrl = null,
    string? BackdropUrl = null,
    string? ExternalId = null,
    double? Rating = null,
    IReadOnlyList<CatalogScanTag>? Tags = null);

/// <summary>Fingerprint identity and metadata observed during one scan of a file.</summary>
public sealed record CatalogScanIdentityUpdate(
    string? Fingerprint = null,
    double? Confidence = null,
    string? ScanSignature = null,
    DateTimeOffset? LastScannedAt = null,
    DateTimeOffset? LastSeenAt = null,
    CatalogScanMetadataUpdate? Metadata = null);

public sealed record CatalogScanCreateRequest(
    int CatalogId,
    string Title,
    string Kind,
    string StoragePath,
    long SizeBytes,
    CatalogScanIdentityUpdate? Scan = null);

/// <summary>Host service granted by the <c>catalog-scanning</c> capability. It keeps agent
/// state on CatalogItem rows without exposing AuthDbContext or host configuration to plugins.
/// All paths are validated to stay inside the owning catalog folder.</summary>
public interface ICatalogScanStore
{
    Task<IReadOnlyList<CatalogScanCatalogInfo>> ListCatalogsAsync(CancellationToken cancellationToken = default);

    Task<CatalogScanCatalogInfo?> GetCatalogAsync(int catalogId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<CatalogScanItemInfo>> ListItemsAsync(int catalogId, CancellationToken cancellationToken = default);

    Task<CatalogScanItemInfo?> FindByPathAsync(string path, CancellationToken cancellationToken = default);

    Task<int> CreateAsync(CatalogScanCreateRequest request, CancellationToken cancellationToken = default);

    Task UpdateAsync(int itemId, CatalogScanIdentityUpdate update, CancellationToken cancellationToken = default);

    Task DeleteAsync(int itemId, CancellationToken cancellationToken = default);
}
