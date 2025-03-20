# VRChat Instance Joiner: Testing Plan

This document outlines the comprehensive testing strategy for the VRChat Instance Joiner application, detailing the frameworks, methodologies, and approaches to ensure robust and reliable functionality.

## 1. Testing Framework and Tools

### Unit Testing
- **xUnit**: Modern, feature-rich testing framework for .NET
- **Moq**: For mocking dependencies and isolating components
- **FluentAssertions**: For more readable and expressive assertions
- **Microsoft.Extensions.DependencyInjection.Testing**: For testing DI scenarios

### API Testing
- **WireMock.NET**: For mocking the VRChat API responses in a controlled environment
- **Refit.HttpClientTesting**: For testing HTTP client interactions

### UI Testing
- **FlaUI**: Automation framework specifically designed for WPF applications
- **MVVM Testing**: Testing ViewModels directly without UI dependencies

## 2. Testing Methodologies

### Test-Driven Development (TDD)
- Write tests before implementing features
- Red-Green-Refactor cycle:
  1. Write failing test
  2. Implement just enough code to pass
  3. Refactor while keeping tests green

### Stubbing the VRChat API

```csharp
// Example of API stubbing approach
public class FakeVRChatApiService : IVRChatApiService
{
    private readonly Dictionary<string, object> _responses = new();
    
    public void SetupResponse<T>(string endpoint, T response)
    {
        _responses[endpoint] = response;
    }
    
    public Task<TResponse> GetAsync<TResponse>(string endpoint)
    {
        if (_responses.TryGetValue(endpoint, out var response))
        {
            return Task.FromResult((TResponse)response);
        }
        
        throw new TestException($"No setup for endpoint: {endpoint}");
    }
    
    // Implement other interface methods with similar pattern
}

// Usage in tests
[Fact]
public async Task GetGroups_ReturnsGroups()
{
    // Arrange
    var fakeApi = new FakeVRChatApiService();
    var expectedGroups = new List<VRChatGroup> { /* sample data */ };
    fakeApi.SetupResponse("groups", expectedGroups);
    
    var groupService = new GroupService(fakeApi);
    
    // Act
    var groups = await groupService.GetGroupsAsync();
    
    // Assert
    groups.Should().BeEquivalentTo(expectedGroups);
}
```

## 3. Test Categories

### Unit Tests
- Test individual components in isolation
- High coverage for business logic and services
- Fast execution for immediate feedback
- Focus on core algorithms, data processing, and business rules

### Integration Tests
- Test interactions between components
- Test complete workflows without UI
- Example: Auth service + Group service + Instance service
- Verify correct component interaction and data flow

### UI Tests
- Automated tests for critical UI workflows
- Focus on key user journeys:
  1. Authentication flow
  2. Group selection and monitoring
  3. Instance joining
- Verify UI behavior, visual feedback, and user interaction

### End-to-End Tests
- Launch the actual application
- Use FlaUI to interact with the UI
- Verify complete user journeys
- These run slower but validate the entire application
- Test against real or mocked VRChat API endpoints

## 4. Testable Architecture Design

### Interface-Based Design
```csharp
public interface IVRChatApiService { /* methods */ }
public interface IGroupService { /* methods */ }
public interface IInstanceService { /* methods */ }
public interface ISettingsService { /* methods */ }
```

### Dependency Injection
```csharp
// ViewModels dependent on interfaces, not implementations
public class GroupViewModel
{
    private readonly IGroupService _groupService;
    
    public GroupViewModel(IGroupService groupService)
    {
        _groupService = groupService;
    }
    
    // Methods that can be tested by injecting mocks
}
```

### View-ViewModel Separation
- Views have no business logic
- ViewModels are testable without UI
- Commands and properties can be tested directly

## 5. Sample Test Structure

```
Tests/
├── Unit/
│   ├── Services/
│   │   ├── AuthServiceTests.cs
│   │   ├── GroupServiceTests.cs
│   │   └── InstanceServiceTests.cs
│   ├── ViewModels/
│   │   ├── LoginViewModelTests.cs
│   │   ├── GroupViewModelTests.cs
│   │   └── InstanceViewModelTests.cs
│   └── Models/
│       └── ModelTests.cs
├── Integration/
│   ├── AuthGroupIntegrationTests.cs
│   └── InstanceJoinIntegrationTests.cs
└── UI/
    ├── LoginUITests.cs
    ├── GroupSelectionUITests.cs
    └── InstanceMonitoringUITests.cs
```

## 6. Automated Testing in Development Workflow

1. **Local Development**:
   - Run unit tests continuously during development
   - xUnit test explorer integration in Visual Studio
   - Quick feedback on code changes

2. **CI Pipeline** (if implemented):
   - Run all tests on commit/PR
   - Generate test reports
   - Track code coverage
   - Prevent merging of failing code

3. **Test Data Management**:
   - Sample API responses saved as JSON fixtures
   - Test data factories for generating test objects
   - Consistent test data across test suites

## 7. Testing Best Practices

1. **Test Isolation**:
   - Each test should be independent and not rely on other tests
   - Tests should clean up after themselves
   - Use fresh test fixtures for each test

2. **Meaningful Test Names**:
   - Follow the convention: `[MethodUnderTest]_[Scenario]_[ExpectedResult]`
   - Example: `AuthenticateUser_WithInvalidCredentials_ThrowsException`

3. **Arrange-Act-Assert Pattern**:
   - Arrange: Set up the test conditions
   - Act: Execute the method under test
   - Assert: Verify the expected outcomes

4. **Test Doubles**:
   - Use mocks for external dependencies
   - Use stubs for providing test data
   - Use fakes for complex behaviors

5. **Test Coverage**:
   - Aim for high coverage of business-critical code
   - Focus on testing behavior, not implementation details
   - Test edge cases and error paths

## 8. Phase-Specific Testing Focus

### Authentication Testing
- Test valid/invalid credentials scenarios
- Test 2FA flow
- Test token persistence and refresh
- Test session handling

### Group Management Testing
- Test group retrieval
- Test group selection
- Test group caching
- Test UI updates on group change

### Instance Monitoring Testing
- Test instance detection
- Test polling mechanism
- Test rate limiting and backoff
- Test notification triggering

### Settings and Preferences Testing
- Test settings persistence
- Test settings application
- Test UI updates on settings change