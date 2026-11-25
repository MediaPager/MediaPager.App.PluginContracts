namespace MediaPager.App.PluginContracts;

/// <summary>One callable background agent exposed by a plugin. The host scheduler reads
/// these descriptors, honours Enabled/Interval, and calls RunAsync when an agent is due.</summary>
public sealed record AgentDescriptor(
    string Id,
    string Name,
    string Description,
    bool Enabled,
    TimeSpan Interval,
    IReadOnlyList<string> CatalogTypeSlugs);

public sealed record AgentRunRequest(
    string AgentId,
    int? CatalogId = null,
    bool Force = false);

public sealed record AgentRunResult(
    string AgentId,
    int Scanned,
    int Added,
    int Updated,
    int Removed,
    int Skipped,
    IReadOnlyList<string>? Errors = null)
{
    public bool Success => Errors is not { Count: > 0 };

    public string Summary =>
        $"Scanned {Scanned}; added {Added}; updated {Updated}; removed {Removed}; skipped {Skipped}.";
}

/// <summary>Capability interface for plugins that perform recurring catalog work. Agents do
/// not require a new worker protocol to be useful; the host invokes in-process plugins
/// directly and worker-backed agents can be added without changing this contract.</summary>
public interface IAgentPlugin : IMediaPagerPlugin
{
    Task<IReadOnlyList<AgentDescriptor>> GetAgentsAsync(CancellationToken cancellationToken = default);

    Task<AgentRunResult> RunAsync(AgentRunRequest request, CancellationToken cancellationToken = default);
}
