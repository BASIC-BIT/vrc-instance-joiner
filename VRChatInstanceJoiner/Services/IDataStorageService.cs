using System.Threading.Tasks;
using VRChatInstanceJoiner.Models;

namespace VRChatInstanceJoiner.Services
{
    public interface IDataStorageService
    {
        Task<AppSettings> LoadSettingsAsync();
        Task SaveSettingsAsync(AppSettings settings);
        Task<string> LoadAuthTokenAsync();
        Task SaveAuthTokenAsync(string token);
        Task ClearAuthTokenAsync();
        Task<T> LoadDataAsync<T>(string key) where T : class, new();
        Task SaveDataAsync<T>(string key, T data) where T : class;
        Task ClearDataAsync(string key);
    }
}