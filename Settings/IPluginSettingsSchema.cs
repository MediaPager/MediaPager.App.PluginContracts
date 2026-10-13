namespace MediaPager.App.PluginContracts;

/// <summary>Type of a data-driven setting field; the settings UI renders generically from these.</summary>
public enum PluginSettingType
{
    String,
    Password,
    Boolean,
    Catalog,
}

/// <summary>One entry in a plugin's settings schema. Values live under
/// <c>Settings:Plugins:&lt;descriptor-id&gt;:&lt;Key&gt;</c>; secret values are protected by the host.</summary>
[method: System.Text.Json.Serialization.JsonConstructor]
public sealed record PluginSettingDefinition(
    string Key,
    string Label,
    PluginSettingType Type = PluginSettingType.String,
    bool Required = false,
    bool Secret = false,
    string? Default = null,
    string? CatalogTypeSlug = null,
    string? Group = null,
    string? Description = null,
    string? Placeholder = null)
{
    // Preserve the constructor ABI used by already-built community plugins.
    public PluginSettingDefinition(
        string Key,
        string Label,
        PluginSettingType Type,
        bool Required,
        bool Secret,
        string? Default,
        string? CatalogTypeSlug)
        : this(Key, Label, Type, Required, Secret, Default, CatalogTypeSlug, null, null, null)
    {
    }
}

/// <summary>Optional capability: declare settings the plugin reads so the settings UI and
/// first-boot defaults are data-driven instead of hard-coded per provider.</summary>
public interface IPluginSettingsSchema
{
    IReadOnlyList<PluginSettingDefinition> Settings { get; }
}
