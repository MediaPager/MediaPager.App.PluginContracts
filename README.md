# MediaPager.App.PluginContracts

The **plugin SDK** for MediaPager — interfaces and DTOs only, **zero package dependencies**.
Plugins bind to this clean seam and never touch the host (`MediaPager.App.Core`) or any web
framework, so a plugin compiled against this assembly runs inside any MediaPager host.

## What it is

A single plugin class implements any mix of capability interfaces. The host discovers the
class via `IMediaPagerPlugin` and surfaces each capability it supports through one registry.

```csharp
public interface IMediaPagerPlugin
{
    PluginDescriptor Descriptor { get; }   // id, name, version, author, description
}
```

## Capability interfaces

| Interface | Purpose |
|---|---|
| `IStreamProviderPlugin` | Nav `Sources` + browse/details + `ResolveAsync` → a raw upstream stream URI. The core validates it (SSRF guard) and mints the proxied playback session. |
| `IMetadataProviderPlugin` | Id-based details/enrichment for scanning/updating existing library data. |
| `ISearchProviderPlugin` | Fuzzy unified-search hits contributed into the type-ahead. |
| `ISubtitleProviderPlugin` | Subtitle search + fetch; returns **native** content (SRT/VTT). The host converts SRT→WebVTT for browser tracks. |
| `IPluginScopeSchema` | Declares plugin-owned user scopes. Scope ids are namespaced by the plugin id; action dispatch can require one of those scopes. |
| `IPluginSettingsSchema` | *(optional)* Declares the settings the plugin reads so the settings UI is data-driven. |

## Supporting types

- `IPluginSettingsStore` — core-hosted, namespaced settings under
  `Settings:Plugins:<plugin-id>:<name>`. The host binds the instance to the plugin identity,
  rejects attempts to address another plugin's settings, and protects stored values at rest.
  The short key argument remains for source compatibility and must match the plugin id's
  final segment.
- `IPluginHost` — read-only access to plugins declared as dependencies in the manifest.
- `IMediaToolBroker` — typed catalog transfers with a fixed transcode-profile enum. The
  host owns paths, FFmpeg arguments, tool versions, and process execution.
- `IBrowserAutomationBroker` — bounded public HTTPS text fetches and page navigation that
  returns only a matched request URL. Plugins never receive a Playwright/browser object.
- `IPluginScopeSchema` — declare plugin-local scope ids such as `can-download`. For plugin
  `mediapager.stream.vidsrc`, that scope becomes `vidsrc:can-download`. Set
  `PluginActionDescriptor.RequiredScope` to the plugin-local id to have the host hide/disable
  the action in the SPA and enforce the scope again when it is dispatched. The server accepts
  only scopes declared by that loaded plugin; plugins cannot grant themselves permissions.
- `PluginSettingDefinition` — one data-driven setting field (`String`/`Password`, `Required`,
  `Secret`, `Default`).
- `MediaKind` — the six content kinds (`Movie`, `Tv`, `Music`, `Podcast`, `Audiobook`, `Book`).
- `TitleDetails` / `BrowseItem` / `SeasonInfo` / `EpisodeInfo` / `CreditRole` / `EnrichmentData`
  — the content DTOs.
- `Paged<T>` — a page of results (`Items`, `Page`, optional `TotalPages`).

## Conventions

- "Not available" is a `null` return (e.g. `ResolveAsync`, `GetDetailsAsync`), never an
  exception for ordinary misses — a provider fails clean.
- Plugins reference **Contracts only**; the host references Contracts; hosts reference Core
  (+ plugins at runtime).
- The SDK stays dependency-free: browser and media interfaces are host broker contracts;
  plugins do not reference browser engines, tool binaries, Core, or API assemblies.

## Example skeleton

```csharp
public sealed class MyPlugin : IMediaPagerPlugin, IPluginSettingsSchema
{
    public PluginDescriptor Descriptor { get; } = new(
        Id: "mediapager.mine",
        Name: "My Plugin",
        Version: "0.1.0",
        Author: "Me",
        Description: "Does a thing.");

    public IReadOnlyList<PluginSettingDefinition> Settings { get; } =
    [
        new("apiKey", "API key", PluginSettingType.Password, Required: true, Secret: true),
    ];
}
```
