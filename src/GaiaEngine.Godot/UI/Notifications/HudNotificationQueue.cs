using System;
using System.Collections.Generic;

namespace GaiaEngine.Godot.UI.Notifications;

/// <summary>
/// Maintains a deterministic HUD notification queue with active items and recent history.
/// </summary>
public sealed class HudNotificationQueue
{
    private readonly int activeLimit;
    private readonly int historyLimit;
    private readonly List<QueuedNotification> pendingEntries = new();
    private readonly List<QueuedNotification> activeEntries = new();
    private readonly List<HudNotificationEntry> historyEntries = new();
    private readonly HashSet<string> knownNotificationIds = new(StringComparer.Ordinal);

    /// <summary>
    /// Initializes a new instance of the <see cref="HudNotificationQueue"/> class.
    /// </summary>
    /// <param name="activeLimit">The maximum number of active visible notifications.</param>
    /// <param name="historyLimit">The maximum number of history entries to retain.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when one limit is not positive.</exception>
    public HudNotificationQueue(int activeLimit, int historyLimit)
    {
        if (activeLimit <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(activeLimit), "The active notification limit must be greater than zero.");
        }

        if (historyLimit <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(historyLimit), "The notification history limit must be greater than zero.");
        }

        this.activeLimit = activeLimit;
        this.historyLimit = historyLimit;
    }

    /// <summary>
    /// Enqueues one notification when it has not already been seen.
    /// </summary>
    /// <param name="entry">The notification entry to enqueue.</param>
    public void Enqueue(HudNotificationEntry entry)
    {
        ArgumentNullException.ThrowIfNull(entry);
        if (!knownNotificationIds.Add(entry.NotificationId))
        {
            return;
        }

        pendingEntries.Add(new QueuedNotification(entry, 0d));
        pendingEntries.Sort(CompareQueuedNotifications);
        PromotePendingEntries();
    }

    /// <summary>
    /// Enqueues multiple notifications in deterministic order.
    /// </summary>
    /// <param name="entries">The notification entries to enqueue.</param>
    public void EnqueueRange(IReadOnlyList<HudNotificationEntry> entries)
    {
        ArgumentNullException.ThrowIfNull(entries);
        foreach (HudNotificationEntry entry in entries)
        {
            Enqueue(entry);
        }
    }

    /// <summary>
    /// Advances the notification presentation clock.
    /// </summary>
    /// <param name="deltaSeconds">The elapsed real presentation time.</param>
    public void Advance(double deltaSeconds)
    {
        if (deltaSeconds <= 0d)
        {
            return;
        }

        List<QueuedNotification> remainingEntries = new(activeEntries.Count);
        foreach (QueuedNotification entry in activeEntries)
        {
            if (entry.Notification.DurationSeconds <= 0d)
            {
                remainingEntries.Add(entry);
                continue;
            }

            double remainingDuration = entry.RemainingSeconds - deltaSeconds;
            if (remainingDuration > 0d)
            {
                remainingEntries.Add(new QueuedNotification(entry.Notification, remainingDuration));
            }
        }

        activeEntries.Clear();
        activeEntries.AddRange(remainingEntries);
        PromotePendingEntries();
    }

    /// <summary>
    /// Returns the active visible notifications in deterministic display order.
    /// </summary>
    /// <returns>The active visible notifications.</returns>
    public IReadOnlyList<HudNotificationEntry> GetActive()
    {
        List<HudNotificationEntry> entries = new(activeEntries.Count);
        foreach (QueuedNotification activeEntry in activeEntries)
        {
            entries.Add(activeEntry.Notification);
        }

        return entries.AsReadOnly();
    }

    /// <summary>
    /// Returns the retained notification history in newest-first order.
    /// </summary>
    /// <returns>The retained notification history.</returns>
    public IReadOnlyList<HudNotificationEntry> GetHistory()
    {
        return historyEntries.AsReadOnly();
    }

    private void PromotePendingEntries()
    {
        while (activeEntries.Count < activeLimit && pendingEntries.Count > 0)
        {
            QueuedNotification next = pendingEntries[0];
            pendingEntries.RemoveAt(0);
            activeEntries.Add(new QueuedNotification(next.Notification, next.Notification.DurationSeconds));
            historyEntries.Insert(0, next.Notification);
            if (historyEntries.Count > historyLimit)
            {
                historyEntries.RemoveAt(historyEntries.Count - 1);
            }
        }
    }

    private static int CompareQueuedNotifications(QueuedNotification left, QueuedNotification right)
    {
        int priorityComparison = right.Notification.Priority.CompareTo(left.Notification.Priority);
        if (priorityComparison != 0)
        {
            return priorityComparison;
        }

        int timestampComparison = left.Notification.Timestamp.CompareTo(right.Notification.Timestamp);
        if (timestampComparison != 0)
        {
            return timestampComparison;
        }

        return string.CompareOrdinal(left.Notification.NotificationId, right.Notification.NotificationId);
    }

    private sealed record QueuedNotification(HudNotificationEntry Notification, double RemainingSeconds);
}
