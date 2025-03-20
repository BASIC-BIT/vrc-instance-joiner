using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VRChatInstanceJoiner.Models;

namespace VRChatInstanceJoiner.Services
{
    public interface IVRChatApiService
    {
        bool IsAuthenticated { get; }
        
        Task<bool> AuthenticateAsync(string username, string password);
        Task<bool> VerifyTwoFactorAuthAsync(string code);
        Task<bool> AuthenticateWithTokenAsync(string token);
        Task LogoutAsync();
        
        Task<List<VRChatGroup>> GetGroupsAsync();
        Task<VRChatGroup> GetGroupAsync(string groupId);
        
        Task<List<VRChatInstance>> GetGroupInstancesAsync(string groupId);
        Task<bool> JoinInstanceAsync(string worldId, string instanceId);
    }
}