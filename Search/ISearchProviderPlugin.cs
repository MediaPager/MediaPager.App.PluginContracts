namespace MediaPager.App.PluginContracts;

public sealed record SearchRequest(
    string? Query,
    int Limit,
    IReadOnlyList<MediaKind>? SupportedStreamKinds = null);

/// <summary>A unified-search hit contributed by a provider. SourceKey lets the UI route the
/// hit back to its source. (Named ProviderSearchHit to avoid clashing with the API's wire
/// DTO, MediaPager.App.Api.Dtos.SearchHit.)</summary>
public sealed record ProviderSearchHit(
    MediaKind Kind,
    string ExternalId,
    string Title,
    string SourceKey,
    int? Year = null,
    double? VoteAverage = null,
    string? Overview = null,
    string? ArtworkUrl = null,
    int? CatalogItemId = null,
    int? CatalogId = null)
{
    public IReadOnlyList<ContentMetadata>? Metadata { get; init; }
}

/// <summary>
/// Contributes hits to the unified type-ahead, which fuses core-local library search
/// with every registered search provider. Distinct from IMetadataProviderPlugin:
/// search is fuzzy discovery; metadata lookup is id-based (scanning/update). Search
/// providers that source titles from streaming services can honor SupportedStreamKinds;
/// local-library providers should search the library independently of that filter.
/// </summary>
public interface ISearchProviderPlugin : IMediaPagerPlugin
{
    Task<IReadOnlyList<ProviderSearchHit>> SearchAsync(SearchRequest request, CancellationToken cancellationToken);
}
