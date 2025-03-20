# VRChat Instance Joiner: Debugging Guide

This document provides guidance on debugging the VRChat Instance Joiner application, including how to access logs and troubleshoot common issues.

## Accessing Application Logs

The application creates two log files in the AppData folder:

1. **App Log**: Contains logs from the main application initialization and lifecycle
   - Location: `%APPDATA%\VRChatInstanceJoiner\app_log.txt`

2. **Window Log**: Contains logs specific to the MainWindow initialization and UI creation
   - Location: `%APPDATA%\VRChatInstanceJoiner\window_log.txt`

### How to Access Logs in PowerShell

```powershell
# View App Log
$appDataPath = [Environment]::GetFolderPath('ApplicationData')
Get-Content "$appDataPath\VRChatInstanceJoiner\app_log.txt"

# View Window Log
Get-Content "$appDataPath\VRChatInstanceJoiner\window_log.txt"

# View just the last 10 lines of logs
Get-Content "$appDataPath\VRChatInstanceJoiner\app_log.txt" -Tail 10
Get-Content "$appDataPath\VRChatInstanceJoiner\window_log.txt" -Tail 10
```

### How to Access Logs in Command Prompt

```cmd
type %APPDATA%\VRChatInstanceJoiner\app_log.txt
type %APPDATA%\VRChatInstanceJoiner\window_log.txt
```

## Common Issues and Solutions

### 1. MaterialDesignThemes Resource Loading Error

**Symptoms:**
- Application builds successfully but UI doesn't appear
- Log shows error: `'Set property 'System.Windows.ResourceDictionary.Source' threw an exception.'`
- Error references line in App.xaml

**Cause:**
The application is unable to load the MaterialDesignThemes resources. This could be due to:
- Missing or incorrect NuGet package reference
- Incompatible version of MaterialDesignThemes
- Incorrect resource path in the XAML

**Solution:**
1. **Use the recommended BundledTheme approach** in App.xaml:
   ```xml
   <ResourceDictionary.MergedDictionaries>
       <materialDesign:BundledTheme BaseTheme="Light" PrimaryColor="DeepPurple" SecondaryColor="Lime" />
       <ResourceDictionary Source="pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesign2.Defaults.xaml" />
   </ResourceDictionary.MergedDictionaries>
   ```

2. Ensure both MaterialDesignThemes and MaterialDesignColors packages are the same version:
   ```xml
   <PackageReference Include="MaterialDesignThemes" Version="5.2.1" />
   <PackageReference Include="MaterialDesignColors" Version="5.2.1" />
   ```

3. If you're still having issues, check the MaterialDesignInXAML GitHub repository for the latest recommended setup: https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit

### 2. XAML Initialization Issues

**Symptoms:**
- Application crashes during startup
- Log shows errors related to InitializeComponent()

**Cause:**
Issues with XAML parsing or initialization, often due to:
- Invalid XAML syntax
- Missing resources or references
- Conflicts between programmatic UI creation and XAML

**Solution:**
1. Check for XAML syntax errors
2. Ensure all referenced resources exist
3. If using programmatic UI creation (as in our MainWindow.xaml.cs), consider removing the XAML content and using a pure code approach

## Debugging Techniques

### 1. Enable More Detailed Logging

Add more detailed logging in critical sections:

```csharp
try
{
    // Critical code
    LogToFile("Critical operation succeeded");
}
catch (Exception ex)
{
    LogToFile($"Critical operation failed: {ex.Message}");
    LogToFile($"Stack trace: {ex.StackTrace}");
    // Consider also logging inner exceptions
    if (ex.InnerException != null)
    {
        LogToFile($"Inner exception: {ex.InnerException.Message}");
        LogToFile($"Inner stack trace: {ex.InnerException.StackTrace}");
    }
}
```

### 2. Visual Studio Debugging

For more interactive debugging:

1. Open the solution in Visual Studio
2. Set breakpoints at critical sections
3. Run the application in debug mode (F5)
4. Use the Immediate Window to inspect variables
5. Check the Output window for additional debug information

### 3. WPF-Specific Debugging

For WPF-specific issues:

1. Enable WPF trace logging by adding to App.config:
   ```xml
   <system.diagnostics>
     <sources>
       <source name="System.Windows.Data" switchName="SourceSwitch" />
       <source name="System.Windows" switchName="SourceSwitch" />
       <source name="System.Windows.DependencyProperty" switchName="SourceSwitch" />
       <source name="System.Windows.Markup" switchName="SourceSwitch" />
     </sources>
     <switches>
       <add name="SourceSwitch" value="All" />
     </switches>
     <trace autoflush="true">
       <listeners>
         <add name="textListener" type="System.Diagnostics.TextWriterTraceListener" initializeData="wpf_trace.log" />
       </listeners>
     </trace>
   </system.diagnostics>
   ```

2. Use Snoop or WPF Inspector tools to examine the visual tree at runtime

## Testing Approach

As outlined in our testing plan, we should implement:

1. **Unit Tests**: Test individual components in isolation
2. **Integration Tests**: Test interactions between components
3. **UI Tests**: Test the UI using FlaUI or similar frameworks

For the current issue, implementing a simple UI test would help catch this problem early:

```csharp
[Fact]
public void ApplicationShouldStartWithoutCrashing()
{
    // This test will fail if the application crashes during startup
    var app = new Application();
    var window = new MainWindow(
        new MockDataStorageService(),
        new MockVRChatApiService());
    
    Assert.NotNull(window);
    // Additional assertions as needed
}
```

## Successful Resolution of MaterialDesignThemes Issue

We successfully resolved the MaterialDesignThemes resource loading issue by:

1. Using the recommended BundledTheme approach in App.xaml:
   ```xml
   <ResourceDictionary.MergedDictionaries>
       <materialDesign:BundledTheme BaseTheme="Light" PrimaryColor="DeepPurple" SecondaryColor="Lime" />
       <ResourceDictionary Source="pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesign2.Defaults.xaml" />
   </ResourceDictionary.MergedDictionaries>
   ```

2. Ensuring the MaterialDesignColors package version matched the MaterialDesignThemes version

The logs now show the application starting up successfully:
```
2025-03-20 15:22:11.693 - Application starting...
2025-03-20 15:22:11.707 - Services registered
2025-03-20 15:22:11.716 - Services configured successfully
2025-03-20 15:22:11.967 - OnStartup called
2025-03-20 15:22:12.087 - MainWindow created
2025-03-20 15:22:14.063 - MainWindow.Show() called
```

And the MainWindow is properly initialized:
```
2025-03-20 15:22:11.986 - MainWindow constructor called
2025-03-20 15:22:11.986 - Services initialized
2025-03-20 15:22:11.987 - Creating UI
2025-03-20 15:22:12.086 - UI elements created
2025-03-20 15:22:12.086 - UI created
2025-03-20 15:22:12.087 - Event handlers set up
2025-03-20 15:22:12.267 - MainWindow_Loaded event fired
2025-03-20 15:22:14.059 - Welcome message displayed
```

## MaterialDesignThemes Best Practices

Based on the official MaterialDesignInXAML GitHub repository, here are some best practices for using MaterialDesignThemes in WPF applications:

1. **Use the BundledTheme approach** for simpler theme setup:
   ```xml
   <materialDesign:BundledTheme BaseTheme="Light" PrimaryColor="DeepPurple" SecondaryColor="Lime" />
   ```

2. **Use MaterialDesign2.Defaults.xaml** for Material Design 2 styling:
   ```xml
   <ResourceDictionary Source="pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesign2.Defaults.xaml" />
   ```

3. **Apply the MaterialDesignWindow style** to your main window:
   ```xml
   <Window Style="{StaticResource MaterialDesignWindow}" ... >
   ```

4. **Keep MaterialDesignThemes and MaterialDesignColors packages in sync** to avoid compatibility issues

5. **Use the built-in controls and styles** provided by the library rather than creating custom ones

6. **Refer to the demo applications** in the GitHub repository for examples of how to use the various controls and features
