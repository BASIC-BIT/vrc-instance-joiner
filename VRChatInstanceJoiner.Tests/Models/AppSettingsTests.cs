using System;
using Xunit;
using FluentAssertions;
using VRChatInstanceJoiner.Models;

namespace VRChatInstanceJoiner.Tests.Models
{
    public class AppSettingsTests
    {
        [Fact]
        public void AppSettings_DefaultValues_ShouldBeCorrect()
        {
            // Arrange & Act
            var settings = new AppSettings();

            // Assert
            settings.DarkModeEnabled.Should().BeTrue();
            settings.PollIntervalSeconds.Should().Be(5);
            settings.AutoJoinEnabled.Should().BeFalse();
            settings.NotificationsEnabled.Should().BeTrue();
            settings.NotificationSound.Should().Be("default");
            settings.LastSelectedGroupId.Should().BeEmpty();
            settings.SelectionAlgorithm.Should().Be(InstanceSelectionAlgorithm.MostRecentlyCreated);
        }
    }
}