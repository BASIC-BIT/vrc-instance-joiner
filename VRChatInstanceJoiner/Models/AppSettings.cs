using System;

namespace VRChatInstanceJoiner.Models
{
    public class AppSettings
    {
        // Explicitly initialize all properties with default values
        public bool DarkModeEnabled { get; set; } = true;
        public int PollIntervalSeconds { get; set; } = 5;
        public bool AutoJoinEnabled { get; set; } = false;
        public bool NotificationsEnabled { get; set; } = true;
        public string NotificationSound { get; set; } = "default";
        public string LastSelectedGroupId { get; set; } = string.Empty;
        public InstanceSelectionAlgorithm SelectionAlgorithm { get; set; } = InstanceSelectionAlgorithm.MostRecentlyCreated;

        // Default constructor to ensure properties are initialized
        public AppSettings()
        {
            DarkModeEnabled = true;
            PollIntervalSeconds = 5;
            AutoJoinEnabled = false;
            NotificationsEnabled = true;
            NotificationSound = "default";
            LastSelectedGroupId = string.Empty;
            SelectionAlgorithm = InstanceSelectionAlgorithm.MostRecentlyCreated;
        }
    }

    public enum InstanceSelectionAlgorithm
    {
        AlphabeticalByWorldId,
        MostRecentlyCreated,
        MostUsers,
        FewestUsers,
        Custom
    }
}