namespace MediaPager.App.PluginContracts;

/// <summary>Severity of a plugin-raised notification, mirroring the host notifications panel.</summary>
public enum PluginNotificationLevel
{
    Info,
    Success,
    Warning,
    Error,
}

/// <summary>A point-in-time message a plugin raises into the shared notifications panel.</summary>
public sealed record PluginNotification(
    string Id,
    string PluginId,
    string Title,
    string? Message,
    PluginNotificationLevel Level)
{
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;

    public PluginError? Error { get; init; }
}

/// <summary>A plugin-authored error that the host can transport and display without
/// understanding the plugin's capability or domain.</summary>
public sealed record PluginError(string Code, string Message, string? Remediation = null);

/// <summary>An operation failed with a user-facing, structured plugin error.</summary>
public sealed class PluginOperationException(PluginError error, Exception? innerException = null)
    : Exception(error.Message, innerException)
{
    public PluginError Error { get; } = error;
}

/// <summary>Why a job stopped.</summary>
public enum PluginJobState
{
    Running,
    Completed,
    Failed,
    Cancelled,
}

/// <summary>A snapshot of one plugin-declared job, surfaced to the UI so the notifications
/// panel can monitor progress. <see cref="Progress"/> is 0..1 (null = indeterminate).</summary>
public sealed record PluginJobInfo(
    string Id,
    string PluginId,
    string Title,
    string? Status,
    double? Progress,
    PluginJobState State)
{
    public PluginError? Error { get; init; }
}

/// <summary>A cancellable handle to a long-running job a plugin started. The plugin drives it;
/// the host only observes and can request cancellation.</summary>
public interface IPluginJob
{
    string Id { get; }

    bool IsCancellationRequested { get; }

    CancellationToken CancellationToken { get; }

    void Report(double progress, string? status = null);

    void Complete(string? status = null);

    void Fail(string status);

    /// <summary>Fail this job with a structured error. The default preserves compatibility
    /// with existing job implementations by forwarding the user-facing message.</summary>
    void FailWithError(PluginError error) => Fail(error.Message);
}

/// <summary>
/// Host-provided activity surface, injected into plugins that need to notify the user or report
/// long-running progress (for example, file processing or a refresh task). The host owns the
/// pool and notifications panel; the plugin never renders UI for it.
/// </summary>
public interface IPluginActivity
{
    /// <summary>Raise a message into the shared notifications panel.</summary>
    void Notify(string title, string? message = null, PluginNotificationLevel level = PluginNotificationLevel.Info);

    /// <summary>Raise a structured plugin error into the shared notifications panel. The
    /// default preserves compatibility with existing activity implementations.</summary>
    void NotifyError(string title, PluginError error, PluginNotificationLevel level = PluginNotificationLevel.Error) =>
        Notify(title, error.Message, level);

    /// <summary>Start a long-running job and return its handle so the plugin can report progress.</summary>
    IPluginJob BeginJob(string title);

    /// <summary>Remove a terminal job owned by this plugin. Existing implementations remain
    /// compatible and may leave the default no-op behavior in place.</summary>
    bool ClearJob(IPluginJob job) => false;

    IReadOnlyList<PluginJobInfo> Jobs { get; }

    IReadOnlyList<PluginNotification> Notifications { get; }
}
