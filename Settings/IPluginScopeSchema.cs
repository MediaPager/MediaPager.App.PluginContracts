namespace MediaPager.App.PluginContracts;

/// <summary>One permission a plugin contributes to the host's user-scope editor. The scope
/// is plugin-local (for example "can-download"); the host prefixes it with the plugin's
/// final id segment to create a claim such as "vidsrc:can-download".</summary>
public sealed record PluginScopeDefinition(string Id, string Label, string Description);

/// <summary>Optional capability for declaring user permissions owned by this plugin.</summary>
public interface IPluginScopeSchema
{
    IReadOnlyList<PluginScopeDefinition> Scopes { get; }
}
