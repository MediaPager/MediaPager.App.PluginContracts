namespace MediaPager.App.PluginContracts;

/// <summary>A plugin-owned, per-user boolean preference rendered in the shared Profile UI.</summary>
public sealed record PluginUserPreferenceDefinition(
    string Key,
    string Label,
    string Description,
    bool DefaultValue = false)
{
    /// <summary>Optional plugin-local scope which the preference hides when enabled.</summary>
    public string? HidesScope { get; init; }
}

/// <summary>Optional capability for declaring namespaced preferences shown in the user's Profile.</summary>
public interface IPluginUserSettingsSchema
{
    IReadOnlyList<PluginUserPreferenceDefinition> UserSettings { get; }
}
