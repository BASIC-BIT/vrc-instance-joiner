using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using FluentAssertions;
using VRChatInstanceJoiner.Models;
using VRChatInstanceJoiner.Services;

namespace VRChatInstanceJoiner.Tests.Services
{
    public class VRChatApiServiceTests
    {
        private readonly Mock<ILogger<VRChatApiService>> _loggerMock;
        private readonly Mock<IDataStorageService> _dataStorageServiceMock;
        private readonly VRChatApiService _service;
        
        public VRChatApiServiceTests()
        {
            _loggerMock = new Mock<ILogger<VRChatApiService>>();
            _dataStorageServiceMock = new Mock<IDataStorageService>();
            _service = new VRChatApiService(_loggerMock.Object, _dataStorageServiceMock.Object);
        }
        
        [Fact]
        public async Task AuthenticateAsync_WithValidCredentials_ShouldReturnTrue()
        {
            // Arrange
            var username = "user@example.com";
            var password = "password";
            
            // Act
            var result = await _service.AuthenticateAsync(username, password);
            
            // Assert
            result.Should().BeTrue();
            _service.IsAuthenticated.Should().BeTrue();
            _dataStorageServiceMock.Verify(x => x.SaveAuthTokenAsync(It.IsAny<string>()), Times.Once);
        }
        
        [Fact]
        public async Task AuthenticateAsync_WithEmptyCredentials_ShouldReturnFalse()
        {
            // Arrange
            var username = "";
            var password = "";
            
            // Act
            var result = await _service.AuthenticateAsync(username, password);
            
            // Assert
            result.Should().BeFalse();
            _service.IsAuthenticated.Should().BeFalse();
            _dataStorageServiceMock.Verify(x => x.SaveAuthTokenAsync(It.IsAny<string>()), Times.Never);
        }
        
        [Fact]
        public async Task AuthenticateAsync_WithTestAccount_ShouldRequireTwoFactorAuth()
        {
            // Arrange
            var username = "test@example.com";
            var password = "password";
            
            // Act
            var result = await _service.AuthenticateAsync(username, password);
            
            // Assert
            result.Should().BeTrue(); // Returns true to indicate 2FA is required
            _service.IsAuthenticated.Should().BeTrue(); // Temporary token is set
            _dataStorageServiceMock.Verify(x => x.SaveAuthTokenAsync(It.IsAny<string>()), Times.Never);
        }
        
        [Fact]
        public async Task VerifyTwoFactorAuthAsync_WithValidCode_ShouldReturnTrue()
        {
            // Arrange
            await _service.AuthenticateAsync("test@example.com", "password");
            var code = "123456";
            
            // Act
            var result = await _service.VerifyTwoFactorAuthAsync(code);
            
            // Assert
            result.Should().BeTrue();
            _service.IsAuthenticated.Should().BeTrue();
            _dataStorageServiceMock.Verify(x => x.SaveAuthTokenAsync(It.IsAny<string>()), Times.Once);
        }
        
        [Fact]
        public async Task VerifyTwoFactorAuthAsync_WithInvalidCode_ShouldReturnFalse()
        {
            // Arrange
            await _service.AuthenticateAsync("test@example.com", "password");
            var code = "12345"; // Not 6 digits
            
            // Act
            var result = await _service.VerifyTwoFactorAuthAsync(code);
            
            // Assert
            result.Should().BeFalse();
            _dataStorageServiceMock.Verify(x => x.SaveAuthTokenAsync(It.IsAny<string>()), Times.Never);
        }
        
        [Fact]
        public async Task AuthenticateWithTokenAsync_WithValidToken_ShouldReturnTrue()
        {
            // Arrange
            var token = "auth-token-12345";
            
            // Act
            var result = await _service.AuthenticateWithTokenAsync(token);
            
            // Assert
            result.Should().BeTrue();
            _service.IsAuthenticated.Should().BeTrue();
        }
        
        [Fact]
        public async Task AuthenticateWithTokenAsync_WithInvalidToken_ShouldReturnFalse()
        {
            // Arrange
            var token = "invalid-token";
            
            // Act
            var result = await _service.AuthenticateWithTokenAsync(token);
            
            // Assert
            result.Should().BeFalse();
            _service.IsAuthenticated.Should().BeFalse();
        }
        
        [Fact]
        public async Task LogoutAsync_ShouldClearAuthenticationAndToken()
        {
            // Arrange
            await _service.AuthenticateAsync("user@example.com", "password");
            
            // Act
            await _service.LogoutAsync();
            
            // Assert
            _service.IsAuthenticated.Should().BeFalse();
            _dataStorageServiceMock.Verify(x => x.ClearAuthTokenAsync(), Times.Once);
        }
        
        [Fact]
        public async Task GetGroupsAsync_WhenAuthenticated_ShouldReturnGroups()
        {
            // Arrange
            await _service.AuthenticateAsync("user@example.com", "password");
            
            // Act
            var groups = await _service.GetGroupsAsync();
            
            // Assert
            groups.Should().NotBeNull();
            groups.Should().HaveCount(2);
            groups[0].Id.Should().Be("grp_12345");
            groups[1].Id.Should().Be("grp_67890");
        }
        
        [Fact]
        public async Task GetGroupsAsync_WhenNotAuthenticated_ShouldReturnEmptyList()
        {
            // Act
            var groups = await _service.GetGroupsAsync();
            
            // Assert
            groups.Should().NotBeNull();
            groups.Should().BeEmpty();
        }
        
        [Fact]
        public async Task GetGroupAsync_WithValidId_ShouldReturnGroup()
        {
            // Arrange
            await _service.AuthenticateAsync("user@example.com", "password");
            
            // Act
            var group = await _service.GetGroupAsync("grp_12345");
            
            // Assert
            group.Should().NotBeNull();
            group.Id.Should().Be("grp_12345");
            group.Name.Should().Be("VRChat Enthusiasts");
        }
        
        [Fact]
        public async Task GetGroupAsync_WithInvalidId_ShouldReturnNull()
        {
            // Arrange
            await _service.AuthenticateAsync("user@example.com", "password");
            
            // Act
            var group = await _service.GetGroupAsync("invalid_id");
            
            // Assert
            group.Should().BeNull();
        }
        
        [Fact]
        public async Task GetGroupInstancesAsync_WithValidId_ShouldReturnInstances()
        {
            // Arrange
            await _service.AuthenticateAsync("user@example.com", "password");
            
            // Act
            var instances = await _service.GetGroupInstancesAsync("grp_12345");
            
            // Assert
            instances.Should().NotBeNull();
            instances.Should().HaveCount(2);
            instances[0].WorldId.Should().Be("wrld_12345");
            instances[1].WorldId.Should().Be("wrld_67890");
        }
        
        [Fact]
        public async Task GetGroupInstancesAsync_WithInvalidId_ShouldReturnEmptyList()
        {
            // Arrange
            await _service.AuthenticateAsync("user@example.com", "password");
            
            // Act
            var instances = await _service.GetGroupInstancesAsync("invalid_id");
            
            // Assert
            instances.Should().NotBeNull();
            instances.Should().BeEmpty();
        }
        
        [Fact]
        public async Task JoinInstanceAsync_WhenAuthenticated_ShouldReturnTrue()
        {
            // Arrange
            await _service.AuthenticateAsync("user@example.com", "password");
            
            // Act
            var result = await _service.JoinInstanceAsync("wrld_12345", "12345");
            
            // Assert
            result.Should().BeTrue();
        }
        
        [Fact]
        public async Task JoinInstanceAsync_WhenNotAuthenticated_ShouldReturnFalse()
        {
            // Act
            var result = await _service.JoinInstanceAsync("wrld_12345", "12345");
            
            // Assert
            result.Should().BeFalse();
        }
    }
}