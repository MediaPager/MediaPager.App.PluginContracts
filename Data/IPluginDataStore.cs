namespace MediaPager.App.PluginContracts;

/// <summary>A plugin-owned, durable JSON document. The host scopes documents to the calling
/// plugin; keys and collection names have no meaning to the host.</summary>
public sealed record PluginDataDocument(string Key, string Json, DateTimeOffset UpdatedAt);

/// <summary>One document supplied when atomically replacing a plugin collection.</summary>
public sealed record PluginDataWrite(string Key, string Json);

public sealed record PluginDataPage(IReadOnlyList<PluginDataDocument> Items, int Total);

/// <summary>Optional capability for durable, plugin-owned application data. This is not a
/// settings store: do not put credentials or secrets here. The host binds all operations to
/// the plugin identity and never exposes its DbContext or database connection. Main-screen
/// plugin UI data endpoints expose only collections prefixed with <c>public.</c>; keep raw
/// upstream URLs and other implementation details in a separate collection. A public
/// collection may be scope-gated by naming it <c>public.scoped.&lt;scope-id&gt;.&lt;collection&gt;</c>
/// when the plugin declares that scope.</summary>
public interface IPluginDataStore
{
    Task<PluginDataPage> ListAsync(
        string pluginKey,
        string collection,
        int offset = 0,
        int limit = 250,
        CancellationToken cancellationToken = default);

    Task<PluginDataDocument?> GetAsync(
        string pluginKey,
        string collection,
        string key,
        CancellationToken cancellationToken = default);

    /// <summary>Atomically replaces one collection. If validation or a database write fails,
    /// the previous collection remains intact.</summary>
    Task ReplaceCollectionAsync(
        string pluginKey,
        string collection,
        IReadOnlyList<PluginDataWrite> documents,
        CancellationToken cancellationToken = default);

    Task UpsertAsync(
        string pluginKey,
        string collection,
        string key,
        string json,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        string pluginKey,
        string collection,
        string key,
        CancellationToken cancellationToken = default);
}
