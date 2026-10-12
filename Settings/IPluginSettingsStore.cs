namespace MediaPager.App.PluginContracts;

/// <summary>
/// Plugin-facing settings view. The host binds this store to the caller's plugin identity;
/// the legacy <c>pluginKey</c> argument must match that plugin's settings key.
/// Storage is backed by <c>plugins.{pluginKey}.{name}</c> with app configuration as a
/// fallback. Plugins must never receive an unscoped host repository.
/// </summary>
public interface IPluginSettingsStore
{
    Task<string?> GetAsync(string pluginKey, string name, CancellationToken cancellationToken = default);

    /// <summary>Pass a null/whitespace value to delete the stored setting.</summary>
    Task SetAsync(string pluginKey, string name, string? value, CancellationToken cancellationToken = default);

    Task<IReadOnlyDictionary<string, string>> GetAllAsync(string pluginKey, CancellationToken cancellationToken = default);
}
