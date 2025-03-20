# VRChat Instance Joiner: Implementation Prompt Plan

This document breaks down the implementation steps for the VRChat Instance Joiner application into manageable chunks, organized to gradually build up functionality.

## Phase 1: Project Setup and Foundation

### Step 1: Project Initialization
- Create a new WPF application using .NET 7/8
- Set up the project structure with appropriate folders (Models, ViewModels, Views, Services)
- Add necessary NuGet packages:
  - MaterialDesignThemes
  - VRChat.API (C# SDK)
  - Newtonsoft.Json
  - Microsoft.Extensions.DependencyInjection
  - Microsoft.Extensions.Logging
- Create the main application window skeleton

**Testing:**
- Create a basic test project
- Set up xUnit as the test framework
- Add Moq, FluentAssertions, and WireMock.NET packages
- Configure test project for proper dependency injection
- Add a simple test to verify project configuration

### Step 2: Data Model Implementation
- Implement core data models:
  - AppSettings class
  - VRChatGroup model
  - VRChatInstance model
  - Enum definitions (InstanceType, InstanceSelectionAlgorithm)
- Implement models for API communication
- Create data storage service (interface and implementation)

**Testing:**
- Unit tests for model serialization/deserialization
- Tests for data validation logic

## Phase 2: API Integration

### Step 3: VRChat API Service
- Create an API service interface (IVRChatApiService)
- Implement basic API wrapper using the VRChat C# SDK
- Implement error handling and rate limiting logic
- Create a method to retrieve authentication status

**Testing:**
- Unit tests for API service with mocked responses
- Test rate limiting and error handling logic

### Step 4: Authentication Implementation
- Create login view and view model
- Implement credential handling
- Build 2FA verification interface
- Implement secure token storage using DPAPI
- Create authentication service for managing logged-in state

**Testing:**
- Unit tests for authentication logic
- Test token storage and retrieval

## Phase 3: Core Functionality

### Step 5: Group Management
- Implement service for querying user's groups
- Create group selection interface
- Add group data caching for improved performance
- Implement UI for displaying group details

**Testing:**
- Unit tests for group retrieval and filtering
- Test caching mechanism

### Step 6: Instance Monitoring
- Create instance monitoring service
- Implement polling mechanism with configurable interval
- Add rate limit handling with exponential backoff
- Develop instance display UI
- Create instance discovery notifications

**Testing:**
- Tests for polling mechanism
- Test instance detection logic
- Test rate limit handling

### Step 7: Instance Joining
- Implement instance joining functionality
- Create instance selection algorithm
- Build manual and automatic join capabilities
- Implement join history tracking

**Testing:**
- Unit tests for instance selection algorithm
- Test joining functionality

## Phase 4: UI Refinement

### Step 8: Material Design Implementation
- Apply Material Design theme to all UI components
- Implement responsive layouts
- Create custom controls for improved user experience
- Implement light/dark theme switching

**Testing:**
- UI tests for responsiveness
- Test theme switching functionality

### Step 9: Settings and Preferences
- Create settings view
- Implement preference persistence
- Build UI for configuring:
  - Poll interval
  - Notification preferences
  - Instance selection algorithm
  - Theme selection

**Testing:**
- Test settings persistence
- Test settings application to app behavior

### Step 10: Notification System
- Implement Windows notification integration
- Add custom in-app notifications
- Create sound notification capabilities
- Build notification preference UI

**Testing:**
- Test notification triggering
- Test notification preferences

## Phase 5: Polish and Packaging

### Step 11: Application Refinement
- Implement system tray functionality
- Add startup with Windows option
- Create about screen with version info
- Add error reporting and logging
- Implement usage statistics (optional)

**Testing:**
- Test system tray functionality
- Test startup behavior

### Step 12: Packaging and Distribution
- Set up application icon and branding
- Create installer package
- Generate self-contained executable
- Prepare distribution package
- Create basic user documentation

**Testing:**
- Test installation and uninstallation
- Test application startup and functionality in packaged form

## Implementation Considerations

### Incremental Development Approach
- Each step should be completed before moving to the next
- Testing should be performed alongside implementation
- Code reviews should be conducted at the end of each phase

### Cross-Cutting Concerns
- Error handling should be implemented throughout
- Logging should be integrated at all levels
- Security considerations should be addressed in each relevant component

### Quality Assurance
- Maintain consistent code style
- Document public APIs and complex functionality
- Ensure all tests are passing before completing a phase