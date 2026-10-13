namespace MediaPager.App.PluginContracts;

/// <summary>Where a plugin action icon is rendered. Poster cards are the grid tiles in the
/// Stream home and source browse grids; detail screens are the per-title sheets.</summary>
public enum PluginActionSurface
{
    PosterCard,
    DetailScreen,
}

/// <summary>Where the host renders a plugin action. Positioned actions stack by
/// <see cref="PluginActionDescriptor.Order"/> within their selected corner.</summary>
public enum PluginActionPosition
{
    Inline,
    TopLeft,
    TopRight,
    BottomLeft,
    BottomRight,
}

/// <summary>What happens when an action icon is clicked. <see cref="Dispatch"/> posts the
/// context to the host, which calls <see cref="IPluginActions.InvokeActionAsync"/>.
/// <see cref="OpenUi"/> embeds the plugin's own UI at <see cref="PluginActionDescriptor.UiPath"/>
/// (served from <c>/plugins/{key}/ui/</c>); <see cref="HostAction"/> invokes a generic host UI
/// action named by <see cref="PluginActionDescriptor.HostActionId"/>.</summary>
public enum PluginActionClick
{
    Dispatch,
    OpenUi,
    HostAction,
}

/// <summary>What content context a declared action accepts.</summary>
public enum PluginActionScope
{
    Any,
    StreamItem,
    LibraryItem,
    Series,
    Season,
    Episode,
}

/// <summary>Known generic host actions plugins may request through a HostAction descriptor.</summary>
public static class PluginHostActionIds
{
    /// <summary>Open the shared metadata editor for the CatalogItemId in the action context.</summary>
    public const string EditCatalogItem = "catalog-item.edit";
}

/// <summary>An action a plugin contributes to poster cards and/or detail screens. The host
/// renders it generically — it never knows what the action does. <see cref="Kinds"/> filters
/// which catalog types see it (null = every kind).</summary>
[method: System.Text.Json.Serialization.JsonConstructor]
public sealed record PluginActionDescriptor(
    string ActionId,
    string Label,
    string Icon,
    PluginActionSurface Surface,
    PluginActionPosition Position = PluginActionPosition.TopRight,
    int Order = 0,
    IReadOnlyList<MediaKind>? Kinds = null,
    PluginActionClick Click = PluginActionClick.Dispatch,
    string? UiPath = null,
    PluginActionScope Scope = PluginActionScope.Any,
    string? HostActionId = null,
    bool Enabled = true,
    string? AvailabilityMessage = null,
    string? AvailabilityPluginId = null,
    string? ConfirmationTitle = null,
    string? ConfirmationMessage = null,
    bool ShowLabel = true,
    string? RequiredScope = null)
{
    // Keep the pre-scope constructor ABI so already-built community plugins continue to load.
    public PluginActionDescriptor(
        string ActionId,
        string Label,
        string Icon,
        PluginActionSurface Surface,
        PluginActionPosition Position,
        int Order,
        IReadOnlyList<MediaKind>? Kinds,
        PluginActionClick Click,
        string? UiPath,
        PluginActionScope Scope,
        string? HostActionId,
        bool Enabled,
        string? AvailabilityMessage,
        string? AvailabilityPluginId,
        string? ConfirmationTitle,
        string? ConfirmationMessage,
        bool ShowLabel)
        : this(ActionId, Label, Icon, Surface, Position, Order, Kinds, Click, UiPath, Scope,
            HostActionId, Enabled, AvailabilityMessage, AvailabilityPluginId, ConfirmationTitle,
            ConfirmationMessage, ShowLabel, null)
    {
    }
}

/// <summary>What the user clicked, handed to <see cref="IPluginActions.InvokeActionAsync"/>
/// for a <see cref="PluginActionClick.Dispatch"/> action. Catalog context (item/type/kind)
/// is present for library titles; source context (source key/external id) for browse titles.</summary>
public sealed record PluginActionContext(
    string ActionId,
    PluginActionSurface Surface,
    int? CatalogItemId = null,
    int? CatalogTypeId = null,
    MediaKind? Kind = null,
    string? SourceKey = null,
    string? ExternalId = null,
    string? Title = null,
    int? Season = null,
    int? Episode = null,
    string? ImageUrl = null,
    string? BackdropUrl = null,
    string? Overview = null,
    int? Year = null)
{
    public IReadOnlyList<ContentMetadata>? Metadata { get; init; }
    public MusicTrackMetadata? MusicMetadata { get; init; }
    public IReadOnlyList<string>? UserScopes { get; init; }
}

/// <summary>Optional capability: contributes positioned action icons to poster cards and
/// detail screens. The host is action-agnostic — it renders whatever a plugin declares and
/// dispatches clicks back. A plugin that raises notifications/progress does so through the
/// injected <see cref="IPluginActivity"/>.</summary>
public interface IPluginActions : IMediaPagerPlugin
{
    IReadOnlyList<PluginActionDescriptor> Actions { get; }

    Task<IReadOnlyList<PluginActionDescriptor>> GetActionsAsync(CancellationToken cancellationToken) =>
        Task.FromResult(Actions);

    Task InvokeActionAsync(PluginActionContext context, CancellationToken cancellationToken) => Task.CompletedTask;
}
