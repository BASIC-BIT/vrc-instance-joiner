using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Extensions.Logging;
using VRChatInstanceJoiner.Models;
using VRChatInstanceJoiner.Services;

namespace VRChatInstanceJoiner.ViewModels
{
    /// <summary>
    /// ViewModel for managing VRChat groups and group selection.
    /// </summary>
    public class GroupViewModel : ViewModelBase
    {
        private readonly IVRChatApiService _vrchatApiService;
        private readonly IDataStorageService _dataStorageService;
        private readonly ILogger<GroupViewModel> _logger;

        private bool _isLoading;
        private string _searchText;
        private VRChatGroup _selectedGroup;
        private string _statusMessage;
        private bool _hasGroups;
        private bool _hasError;

        /// <summary>
        /// Collection of VRChat groups available to the user.
        /// </summary>
        public ObservableCollection<VRChatGroup> Groups { get; } = new ObservableCollection<VRChatGroup>();

        /// <summary>
        /// Collection of filtered VRChat groups based on search criteria.
        /// </summary>
        public ObservableCollection<VRChatGroup> FilteredGroups { get; } = new ObservableCollection<VRChatGroup>();

        /// <summary>
        /// Indicates whether groups are currently being loaded.
        /// </summary>
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        /// <summary>
        /// Search text for filtering groups.
        /// </summary>
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                {
                    FilterGroups();
                }
            }
        }

        /// <summary>
        /// The currently selected VRChat group.
        /// </summary>
        public VRChatGroup SelectedGroup
        {
            get => _selectedGroup;
            set
            {
                if (SetProperty(ref _selectedGroup, value) && value != null)
                {
                    // Don't auto-save when property is set directly
                    // This will be handled by the SelectGroupAsync method
                }
            }
        }

        /// <summary>
        /// Indicates whether there are any groups available.
        /// </summary>
        public bool HasGroups
        {
            get => _hasGroups;
            set => SetProperty(ref _hasGroups, value);
        }

        /// <summary>
        /// Status message to display to the user.
        /// </summary>
        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        /// <summary>
        /// Indicates whether an error has occurred.
        /// </summary>
        public bool HasError
        {
            get => _hasError;
            set => SetProperty(ref _hasError, value);
        }

        /// <summary>
        /// Command to refresh the list of groups.
        /// </summary>
        public ICommand RefreshGroupsCommand { get; }

        /// <summary>
        /// Command to select a group.
        /// </summary>
        public ICommand SelectGroupCommand { get; }

        /// <summary>
        /// Initializes a new instance of the GroupViewModel class.
        /// </summary>
        /// <param name="vrchatApiService">The VRChat API service.</param>
        /// <param name="dataStorageService">The data storage service.</param>
        /// <param name="logger">The logger.</param>
        public GroupViewModel(
            IVRChatApiService vrchatApiService,
            IDataStorageService dataStorageService,
            ILogger<GroupViewModel> logger)
        {
            _vrchatApiService = vrchatApiService ?? throw new ArgumentNullException(nameof(vrchatApiService));
            _dataStorageService = dataStorageService ?? throw new ArgumentNullException(nameof(dataStorageService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            RefreshGroupsCommand = new RelayCommand(async _ => await LoadGroupsAsync());
            SelectGroupCommand = new RelayCommand(async param => await SelectGroupAsync(param as VRChatGroup));

            // Initialize properties
            _searchText = string.Empty;
            _statusMessage = "Ready to load groups";
            _hasGroups = false;
        }

        /// <summary>
        /// Initializes the ViewModel by loading groups and restoring the last selected group.
        /// </summary>
        public async Task InitializeAsync()
        {
            await LoadGroupsAsync();
            await RestoreSelectedGroupAsync();
        }

        /// <summary>
        /// Loads the list of VRChat groups from the API.
        /// </summary>
        public async Task LoadGroupsAsync()
        {
            try
            {
                IsLoading = true;
                HasError = false;
                StatusMessage = "Loading groups...";

                // Clear existing groups
                Groups.Clear();
                FilteredGroups.Clear();
                HasGroups = false;

                // Check if authenticated
                if (!_vrchatApiService.IsAuthenticated)
                {
                    StatusMessage = "Not authenticated. Please log in first.";
                    HasError = true;
                    return;
                }

                // Load groups from API
                var groups = await _vrchatApiService.GetGroupsAsync();

                // Update collections
                foreach (var group in groups)
                {
                    Groups.Add(group);
                }

                // Apply filtering
                FilterGroups();

                // Update HasGroups property
                HasGroups = Groups.Count > 0;

                StatusMessage = $"Loaded {groups.Count} groups";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading groups");
                StatusMessage = $"Error: {ex.Message}";
                HasError = true;
            }
            finally
            {
                IsLoading = false;
            }
        }

        /// <summary>
        /// Filters the groups based on the search text.
        /// </summary>
        private void FilterGroups()
        {
            FilteredGroups.Clear();

            if (string.IsNullOrWhiteSpace(SearchText))
            {
                // If no search text, show all groups
                foreach (var group in Groups)
                {
                    FilteredGroups.Add(group);
                }
            }
            else
            {
                // Filter groups by name or description
                var searchText = SearchText?.ToLowerInvariant() ?? "";
                var filteredGroups = new List<VRChatGroup>();

                foreach (var group in Groups)
                {
                    if (group.Name.ToLowerInvariant().Contains(searchText) ||
                        (group.Description ?? "").ToLowerInvariant().Contains(searchText) ||
                        (group.Tags != null && group.Tags.Any(t => (t ?? "").ToLowerInvariant().Contains(searchText))))
                    {
                        filteredGroups.Add(group);
                    }
                }

                foreach (var group in filteredGroups)
                {
                    FilteredGroups.Add(group);
                }
            }

            // Update status message
            if (FilteredGroups.Count == 0 && !string.IsNullOrWhiteSpace(SearchText))
            {
                StatusMessage = "No groups match your search criteria";
            }
            else if (FilteredGroups.Count != Groups.Count)
            {
                StatusMessage = $"Showing {FilteredGroups.Count} of {Groups.Count} groups";
            }
            else
            {
                StatusMessage = $"Loaded {Groups.Count} groups";
            }
        }

        /// <summary>
        /// Selects a group and saves the selection.
        /// </summary>
        /// <param name="group">The group to select.</param>
        public async Task SelectGroupAsync(VRChatGroup group)
        {
            if (group != null)
            {
                SelectedGroup = group;
                await SaveSelectedGroupAsync(group.Id);
            }
        }

        /// <summary>
        /// Saves the selected group ID to settings.
        /// </summary>
        /// <param name="groupId">The group ID to save.</param>
        private async Task SaveSelectedGroupAsync(string groupId)
        {
            try
            {
                var settings = await _dataStorageService.LoadSettingsAsync();
                settings.LastSelectedGroupId = groupId;
                await _dataStorageService.SaveSettingsAsync(settings);
                _logger.LogInformation($"Saved selected group: {groupId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving selected group");
            }
        }

        /// <summary>
        /// Restores the last selected group from settings.
        /// </summary>
        private async Task RestoreSelectedGroupAsync()
        {
            try
            {
                var settings = await _dataStorageService.LoadSettingsAsync();
                if (!string.IsNullOrEmpty(settings.LastSelectedGroupId))
                {
                    var groupId = settings.LastSelectedGroupId;
                    var group = Groups.FirstOrDefault(g => g.Id == groupId);

                    if (group == null && _vrchatApiService.IsAuthenticated)
                    {
                        // If the group is not in the loaded list, try to fetch it directly
                        group = await _vrchatApiService.GetGroupAsync(groupId);
                        if (group != null)
                        {
                            // Add to collections if not already present
                            if (!Groups.Any(g => g.Id == group.Id))
                            {
                                Groups.Add(group);
                                FilterGroups();
                            }
                        }
                    }

                    if (group != null)
                    {
                        SelectedGroup = group;
                        _logger.LogInformation($"Restored selected group: {group.Name} ({group.Id})");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error restoring selected group");
            }
        }
    }
}