using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using VRChatInstanceJoiner.Models;

namespace VRChatInstanceJoiner.Services
{
    public class DataStorageService : IDataStorageService
    {
        private readonly ILogger<DataStorageService> _logger;
        private readonly string _appDataPath;
        private readonly string _settingsFilePath;
        private readonly string _authTokenFilePath;

        public DataStorageService(ILogger<DataStorageService> logger)
        {
            _logger = logger;
            
            // Create app data directory in user's AppData folder
            _appDataPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "VRChatInstanceJoiner");
            
            if (!Directory.Exists(_appDataPath))
            {
                Directory.CreateDirectory(_appDataPath);
            }
            
            _settingsFilePath = Path.Combine(_appDataPath, "settings.json");
            _authTokenFilePath = Path.Combine(_appDataPath, "auth.dat");
        }

        public async Task<AppSettings> LoadSettingsAsync()
        {
            try
            {
                if (File.Exists(_settingsFilePath))
                {
                    var json = await File.ReadAllTextAsync(_settingsFilePath);
                    var settings = JsonConvert.DeserializeObject<AppSettings>(json);
                    if (settings != null)
                    {
                        return settings;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load settings");
            }
            
            // Return a new instance with default values
            return new AppSettings();
        }

        public async Task SaveSettingsAsync(AppSettings settings)
        {
            try
            {
                var json = JsonConvert.SerializeObject(settings, Formatting.Indented);
                await File.WriteAllTextAsync(_settingsFilePath, json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to save settings");
                throw;
            }
        }

        public async Task<string> LoadAuthTokenAsync()
        {
            try
            {
                if (File.Exists(_authTokenFilePath))
                {
                    var encryptedData = await File.ReadAllBytesAsync(_authTokenFilePath);
                    return Unprotect(encryptedData);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load auth token");
            }
            
            return string.Empty;
        }

        public async Task SaveAuthTokenAsync(string token)
        {
            try
            {
                var encryptedData = Protect(token);
                await File.WriteAllBytesAsync(_authTokenFilePath, encryptedData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to save auth token");
                throw;
            }
        }

        public Task ClearAuthTokenAsync()
        {
            try
            {
                if (File.Exists(_authTokenFilePath))
                {
                    File.Delete(_authTokenFilePath);
                }
                
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to clear auth token");
                throw;
            }
        }

        public async Task<T> LoadDataAsync<T>(string key) where T : class, new()
        {
            try
            {
                var filePath = GetDataFilePath(key);
                
                if (File.Exists(filePath))
                {
                    var json = await File.ReadAllTextAsync(filePath);
                    var data = JsonConvert.DeserializeObject<T>(json);
                    if (data != null)
                    {
                        return data;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to load data for key: {key}");
            }
            
            return new T();
        }

        public async Task SaveDataAsync<T>(string key, T data) where T : class
        {
            try
            {
                var filePath = GetDataFilePath(key);
                var json = JsonConvert.SerializeObject(data, Formatting.Indented);
                await File.WriteAllTextAsync(filePath, json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to save data for key: {key}");
                throw;
            }
        }

        public Task ClearDataAsync(string key)
        {
            try
            {
                var filePath = GetDataFilePath(key);
                
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to clear data for key: {key}");
                throw;
            }
        }

        private string GetDataFilePath(string key)
        {
            // Sanitize the key to make it a valid filename
            var sanitizedKey = string.Join("_", key.Split(Path.GetInvalidFileNameChars()));
            return Path.Combine(_appDataPath, $"{sanitizedKey}.json");
        }

        private byte[] Protect(string data)
        {
            // Use Windows Data Protection API to encrypt the data
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            return ProtectedData.Protect(dataBytes, null, DataProtectionScope.CurrentUser);
        }

        private string Unprotect(byte[] encryptedData)
        {
            // Use Windows Data Protection API to decrypt the data
            byte[] dataBytes = ProtectedData.Unprotect(encryptedData, null, DataProtectionScope.CurrentUser);
            return Encoding.UTF8.GetString(dataBytes);
        }
    }
}