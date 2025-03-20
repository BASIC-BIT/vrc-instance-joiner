# VRChat Instance Joiner: Implementation To-Do List

This checklist tracks the progress of implementing the VRChat Instance Joiner application. Check off items as they are completed.

## Phase 1: Project Setup and Foundation

### Step 1: Project Initialization
- [ ] Create new WPF application (.NET 7/8)
- [ ] Set up project folder structure
  - [ ] Models
  - [ ] ViewModels
  - [ ] Views
  - [ ] Services
- [ ] Add NuGet packages:
  - [ ] MaterialDesignThemes
  - [ ] VRChat.API (C# SDK)
  - [ ] Newtonsoft.Json
  - [ ] Microsoft.Extensions.DependencyInjection
  - [ ] Microsoft.Extensions.Logging
- [ ] Create main application window skeleton
- [ ] Set up test project
  - [ ] Add xUnit as testing framework
  - [ ] Add Moq, FluentAssertions, and WireMock.NET packages
  - [ ] Set up test project for dependency injection
  - [ ] Create first basic test with proper AAA pattern

### Step 2: Data Model Implementation
- [ ] Implement AppSettings class
- [ ] Create VRChatGroup model
- [ ] Create VRChatInstance model
- [ ] Define required enums
  - [ ] InstanceType
  - [ ] InstanceSelectionAlgorithm
- [ ] Implement API communication models
- [ ] Create data storage service
  - [ ] Interface definition
  - [ ] Implementation
- [ ] Write model unit tests

## Phase 2: API Integration

### Step 3: VRChat API Service
- [ ] Create IVRChatApiService interface
- [ ] Implement API wrapper using VRChat C# SDK
- [ ] Add error handling
- [ ] Implement rate limiting logic
- [ ] Create authentication status method
- [ ] Write API service unit tests

### Step 4: Authentication Implementation
- [ ] Create login view
- [ ] Implement login view model
- [ ] Build credential handling
- [ ] Implement 2FA verification interface
- [ ] Create DPAPI secure token storage
- [ ] Implement authentication service
- [ ] Write authentication unit tests

## Phase 3: Core Functionality

### Step 5: Group Management
- [ ] Implement group query service
- [ ] Create group selection UI
- [ ] Add group data caching
- [ ] Implement group details display
- [ ] Write group management unit tests

### Step 6: Instance Monitoring
- [ ] Create instance monitoring service
- [ ] Implement configurable polling
- [ ] Add rate limit handling with backoff
- [ ] Develop instance display UI
- [ ] Implement instance discovery notifications
- [ ] Write instance monitoring unit tests

### Step 7: Instance Joining
- [ ] Implement instance joining functionality
- [ ] Create instance selection algorithm(s)
- [ ] Build manual join capability
- [ ] Implement auto-join functionality
- [ ] Add join history tracking
- [ ] Write instance joining unit tests

## Phase 4: UI Refinement

### Step 8: Material Design Implementation
- [ ] Apply Material Design theme
- [ ] Implement responsive layouts
- [ ] Create custom controls as needed
- [ ] Implement light/dark theme switching
- [ ] Test UI responsiveness

### Step 9: Settings and Preferences
- [ ] Create settings view
- [ ] Implement preference persistence
- [ ] Build UI for configuring:
  - [ ] Poll interval
  - [ ] Notification preferences
  - [ ] Instance selection algorithm
  - [ ] Theme selection
- [ ] Test settings functionality

### Step 10: Notification System
- [ ] Implement Windows notification integration
- [ ] Add custom in-app notifications
- [ ] Create sound notification capabilities
- [ ] Build notification preference UI
- [ ] Test notification system

## Phase 5: Polish and Packaging

### Step 11: Application Refinement
- [ ] Implement system tray functionality
- [ ] Add startup with Windows option
- [ ] Create about screen
- [ ] Add error reporting and logging
- [ ] Implement usage statistics (optional)
- [ ] Test application refinements

### Step 12: Packaging and Distribution
- [ ] Set up application icon and branding
- [ ] Create installer package
- [ ] Generate self-contained executable
- [ ] Prepare distribution package
- [ ] Create basic user documentation
- [ ] Test installation process

## Final Review

- [ ] Perform final code review
- [ ] Run all tests to ensure passing
- [ ] Verify all features against requirements
- [ ] Check UI for consistency and usability
- [ ] Create final release package