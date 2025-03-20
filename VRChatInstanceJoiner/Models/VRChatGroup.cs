using System;
using System.Collections.Generic;

namespace VRChatInstanceJoiner.Models
{
    public class VRChatGroup
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string OwnerId { get; set; } = string.Empty;
        public int MemberCount { get; set; }
        public bool IsJoinRequestEnabled { get; set; }
        public string IconUrl { get; set; } = string.Empty;
        public List<string> Tags { get; set; } = new List<string>();
    }
}