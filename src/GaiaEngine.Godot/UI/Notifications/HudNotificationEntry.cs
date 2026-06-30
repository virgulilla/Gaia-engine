using System;

namespace GaiaEngine.Godot.UI.Notifications;

/// <summary>
/// Represents one deterministic HUD notification entry.
/// </summary>
public sealed record HudNotificationEntry
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HudNotificationEntry"/> class.
    /// </summary>
    /// <param name="notificationId">The stable notification identifier.</param>
    /// <param name="category">The notification category.</param>
    /// <param name="priority">The notification priority.</param>
    /// <param name="title">The short notification title.</param>
    /// <param name="message">The player-facing notification message.</param>
    /// <param name="timestamp">The deterministic source timestamp.</param>
    /// <param name="durationSeconds">The preferred display duration in seconds. Zero or less means persistent.</param>
    /// <param name="actionLabel">The optional action label.</param>
    /// <exception cref="ArgumentException">Thrown when one required text argument is empty.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="timestamp"/> is negative.</exception>
    public HudNotificationEntry(
        string notificationId,
        HudNotificationCategory category,
        HudNotificationPriority priority,
        string title,
        string message,
        long timestamp,
        double durationSeconds,
        string? actionLabel)
    {
        if (string.IsNullOrWhiteSpace(notificationId))
        {
            throw new ArgumentException("The notification identifier must contain a value.", nameof(notificationId));
        }

        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("The notification title must contain a value.", nameof(title));
        }

        if (string.IsNullOrWhiteSpace(message))
        {
            throw new ArgumentException("The notification message must contain a value.", nameof(message));
        }

        if (timestamp < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(timestamp), "The notification timestamp must be zero or greater.");
        }

        NotificationId = notificationId;
        Category = category;
        Priority = priority;
        Title = title;
        Message = message;
        Timestamp = timestamp;
        DurationSeconds = durationSeconds;
        ActionLabel = actionLabel;
    }

    /// <summary>
    /// Gets the stable notification identifier.
    /// </summary>
    public string NotificationId { get; }

    /// <summary>
    /// Gets the notification category.
    /// </summary>
    public HudNotificationCategory Category { get; }

    /// <summary>
    /// Gets the notification priority.
    /// </summary>
    public HudNotificationPriority Priority { get; }

    /// <summary>
    /// Gets the short notification title.
    /// </summary>
    public string Title { get; }

    /// <summary>
    /// Gets the player-facing notification message.
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// Gets the deterministic source timestamp.
    /// </summary>
    public long Timestamp { get; }

    /// <summary>
    /// Gets the preferred display duration in seconds. Zero or less means persistent.
    /// </summary>
    public double DurationSeconds { get; }

    /// <summary>
    /// Gets the optional action label.
    /// </summary>
    public string? ActionLabel { get; }
}
