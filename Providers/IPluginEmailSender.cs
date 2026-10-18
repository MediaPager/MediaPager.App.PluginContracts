namespace MediaPager.App.PluginContracts;

/// <summary>A message a plugin may ask the host to send through its configured email provider.</summary>
public sealed record PluginEmailMessage(string Recipient, string Subject, string Text, string Html);

/// <summary>Optional host capability for sending plugin-owned email through the selected provider.</summary>
public interface IPluginEmailSender
{
    Task<bool> SendAsync(PluginEmailMessage message, CancellationToken cancellationToken = default);
}
