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
1. **Simplify App.xaml**: Remove MaterialDesignThemes references and use basic WPF styles instead:
   ```xml
   <Application x:Class="VRChatInstanceJoiner.App"
                xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
                xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
                xmlns:local="clr-namespace:VRChatInstanceJoiner">
       <Application.Resources>
           <ResourceDictionary>
               <!-- Basic application styles -->
               <Style x:Key="DefaultTextStyle" TargetType="TextBlock">
                   <Setter Property="FontFamily" Value="Segoe UI" />
                   <Setter Property="FontSize" Value="14" />
               </Style>
               
               <Style x:Key="HeaderTextStyle" TargetType="TextBlock">
                   <Setter Property="FontFamily" Value="Segoe UI" />
                   <Setter Property="FontSize" Value="24" />
                   <Setter Property="FontWeight" Value="Bold" />
               </Style>
               
               <Style x:Key="DefaultButtonStyle" TargetType="Button">
                   <Setter Property="Padding" Value="10,5" />
                   <Setter Property="Margin" Value="5" />
                   <Setter Property="Background" Value="#673AB7" />
                   <Setter Property="Foreground" Value="White" />
               </Style>
           </ResourceDictionary>
       </Application.Resources>
   </Application>
   ```

2. If you need MaterialDesignThemes, ensure both packages are the same version:
   ```xml
   <PackageReference Include="MaterialDesignThemes" Version="5.2.1" />
   <PackageReference Include="MaterialDesignColors" Version="5.2.1" />
   ```

3. If using MaterialDesignThemes, try a simpler theme approach:
   ```xml
   <ResourceDictionary.MergedDictionaries>
       <ResourceDictionary Source="pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Light.xaml" />
       <ResourceDictionary Source="pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Defaults.xaml" />
       <ResourceDictionary Source="pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Primary/MaterialDesignColor.DeepPurple.xaml" />
       <ResourceDictionary Source="pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Accent/MaterialDesignColor.Lime.xaml" />
   </ResourceDictionary.MergedDictionaries>
   ```

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

1. Simplifying the App.xaml file to use basic WPF styles instead of MaterialDesignThemes
2. Ensuring the MaterialDesignColors package version matched the MaterialDesignThemes version
3. Using a more direct approach to UI creation in the MainWindow class

The logs now show the application starting up successfully:
```
2025-03-20 15:17:55.731 - Application starting...
2025-03-20 15:17:55.745 - Services registered
2025-03-20 15:17:55.755 - Services configured successfully
2025-03-20 15:17:55.782 - OnStartup called
2025-03-20 15:17:55.942 - MainWindow created
2025-03-20 15:17:58.000 - MainWindow.Show() called
```

And the MainWindow is properly initialized:
```
2025-03-20 15:17:55.816 - MainWindow constructor called
2025-03-20 15:17:55.816 - Services initialized
2025-03-20 15:17:55.817 - Creating UI
2025-03-20 15:17:55.941 - UI elements created
2025-03-20 15:17:55.941 - UI created
2025-03-20 15:17:55.942 - Event handlers set up
2025-03-20 15:17:56.131 - MainWindow_Loaded event fired
2025-03-20 15:17:57.996 - Welcome message displayed
