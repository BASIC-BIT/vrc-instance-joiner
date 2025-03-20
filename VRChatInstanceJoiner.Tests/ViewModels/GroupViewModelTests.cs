using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using VRChatInstanceJoiner.Models;
using VRChatInstanceJoiner.Services;
using VRChatInstanceJoiner.ViewModels;
using Xunit;

namespace VRChatInstanceJoiner.Tests.ViewModels
{
    public class GroupViewModelTests
    {
        private readonly Mock<IVRChatApiService> _mockApiService;
        private readonly Mock<IDataStorageService> _mockDataStorageService;
        private readonly Mock<ILogger<GroupViewModel>> _mockLogger;
        private readonly GroupViewModel _viewModel;

        public GroupViewModelTests()
        {
            // Set up mocks
            _mockApiService = new Mock<IVRChatApiService>();
            _mockDataStorageService = new Mock<IDataStorageService>();
            _mockLogger = new Mock<ILogger<GroupViewModel>>();

            // Create the view model with mocked dependencies
            _viewModel = new GroupViewModel(
                _mockApiService.Object,
                _mockDataStorageService.Object,
                _mockLogger.Object);
        }

        [Fact]
        public async Task InitializeAsync_WhenAuthenticated_LoadsGroups()
        {
            // Arrange
            _mockApiService.Setup(s => s.IsAuthenticated).Returns(true);
            
            var mockGroups = new List<VRChatGroup>
            {
                new VRChatGroup
                {
                    Id = "grp_12345",
                    Name = "Test Group 1",
                    Description = "Test Description 1",
                    MemberCount = 10
                },
                new VRChatGroup
                {
                    Id = "grp_67890",
                    Name = "Test Group 2",
                    Description = "Test Description 2",
                    MemberCount = 20
                }
            };
            
            _mockApiService.Setup(s => s.GetGroupsAsync()).ReturnsAsync(mockGroups);
            
            var settings = new AppSettings { LastSelectedGroupId = "grp_12345" };
            _mockDataStorageService.Setup(s => s.LoadSettingsAsync()).ReturnsAsync(settings);

            // Reset verification counts
            _mockDataStorageService.Invocations.Clear();

            // Act
            await _viewModel.InitializeAsync();

            // Assert
            Assert.Equal(2, _viewModel.Groups.Count);
            Assert.Equal(2, _viewModel.FilteredGroups.Count);
            Assert.True(_viewModel.HasGroups);
            Assert.Equal("grp_12345", _viewModel.SelectedGroup?.Id);
            Assert.False(_viewModel.IsLoading);
            Assert.False(_viewModel.HasError);
            
            // Verify API calls
            _mockApiService.Verify(s => s.GetGroupsAsync(), Times.Once);
            _mockDataStorageService.Verify(s => s.LoadSettingsAsync(), Times.Once);
        }

        [Fact]
        public async Task InitializeAsync_WhenNotAuthenticated_ShowsError()
        {
            // Arrange
            _mockApiService.Setup(s => s.IsAuthenticated).Returns(false);

            // Act
            await _viewModel.InitializeAsync();

            // Assert
            Assert.Empty(_viewModel.Groups);
            Assert.Empty(_viewModel.FilteredGroups);
            Assert.False(_viewModel.HasGroups);
            Assert.Null(_viewModel.SelectedGroup);
            Assert.False(_viewModel.IsLoading);
            Assert.True(_viewModel.HasError);
            Assert.Contains("Not authenticated", _viewModel.StatusMessage);
            
            // Verify API calls
            _mockApiService.Verify(s => s.GetGroupsAsync(), Times.Never);
        }

        [Fact]
        public void SearchText_WhenChanged_FiltersGroups()
        {
            // Arrange
            _viewModel.Groups.Add(new VRChatGroup
            {
                Id = "grp_12345",
                Name = "VRChat Group",
                Description = "A group for VRChat users",
                Tags = new List<string> { "social", "gaming" }
            });
            
            _viewModel.Groups.Add(new VRChatGroup
            {
                Id = "grp_67890",
                Name = "Gaming Group",
                Description = "A group for gamers",
                Tags = new List<string> { "gaming" }
            });
            
            // Get the private FilterGroups method using reflection
            var filterGroupsMethod = typeof(GroupViewModel).GetMethod("FilterGroups", 
                BindingFlags.NonPublic | BindingFlags.Instance);
            
            if (filterGroupsMethod == null)
            {
                throw new Exception("Could not find FilterGroups method via reflection");
            }
            
            // Initial filter should show all groups
            Console.WriteLine($"Groups count: {_viewModel.Groups.Count}");
            foreach (var group in _viewModel.Groups)
            {
                Console.WriteLine($"Group: {group.Name}, Tags: {string.Join(", ", group.Tags ?? new List<string>())}");
            }
            
            // Set empty search text to trigger filtering
            _viewModel.FilteredGroups.Clear();
            _viewModel.SearchText = "";
            
            // Call FilterGroups directly
            filterGroupsMethod.Invoke(_viewModel, null);
            
            Assert.Equal(2, _viewModel.FilteredGroups.Count);

            // Act - filter by name
            _viewModel.SearchText = "VRChat";
            filterGroupsMethod.Invoke(_viewModel, null);
            
            // Assert
            Assert.Single(_viewModel.FilteredGroups);
            Assert.Equal("grp_12345", _viewModel.FilteredGroups[0].Id);
            
            // Act - filter by tag
            _viewModel.SearchText = "gaming";
            filterGroupsMethod.Invoke(_viewModel, null);
            
            // Debug
            Console.WriteLine($"Filtered groups count: {_viewModel.FilteredGroups.Count}");
            foreach (var group in _viewModel.FilteredGroups)
            {
                Console.WriteLine($"Filtered Group: {group.Name}, Tags: {string.Join(", ", group.Tags ?? new List<string>())}");
            }
            
            // Assert
            Assert.Equal(2, _viewModel.FilteredGroups.Count);
            
            // Act - filter with no matches
            _viewModel.SearchText = "nonexistent";
            filterGroupsMethod.Invoke(_viewModel, null);
            
            // Assert
            Assert.Empty(_viewModel.FilteredGroups);
            Assert.Contains("No groups match", _viewModel.StatusMessage);
        }

        [Fact]
        public async Task SelectGroupAsync_SavesSelectedGroup()
        {
            // Arrange
            var group = new VRChatGroup
            {
                Id = "grp_12345",
                Name = "Test Group"
            };
            
            _mockDataStorageService.Setup(s => s.LoadSettingsAsync())
                .ReturnsAsync(new AppSettings());
            
            // Reset verification counts
            _mockDataStorageService.Invocations.Clear();
            
            // Act - use the command to select a group
            await _viewModel.SelectGroupAsync(group);
            
            // Assert
            Assert.Equal(group, _viewModel.SelectedGroup);
            
            // Verify settings were saved
            _mockDataStorageService.Verify(s => s.SaveSettingsAsync(
                It.Is<AppSettings>(settings => settings.LastSelectedGroupId == "grp_12345")), 
                Times.Once);
        }
    }
}