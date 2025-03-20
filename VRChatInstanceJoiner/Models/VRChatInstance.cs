using System;

namespace VRChatInstanceJoiner.Models
{
    public class VRChatInstance
    {
        public string WorldId { get; set; } = string.Empty;
        public string InstanceId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int UserCount { get; set; }
        public int Capacity { get; set; }
        public DateTime CreatedAt { get; set; }
        public InstanceType Type { get; set; }
        public string FullInstanceId => $"{WorldId}:{InstanceId}";
    }

    public enum InstanceType
    {
        Public,
        Friends,
        FriendsPlus,
        Group,
        GroupPlus,
        GroupPublic,
        Invite,
        InvitePlus
    }
}