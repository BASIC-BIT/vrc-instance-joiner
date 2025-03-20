# .NET Upgrade Plan for VRChat Instance Joiner

## Current State Analysis

### Current .NET Version
- The project is currently using .NET 7.0-windows
- .NET 7 is not a Long-Term Support (LTS) version and is out of support as of May 2024

### Package Compatibility Issues
- Several Microsoft.Extensions packages (version 9.0.3) are showing compatibility warnings with .NET 7
- Warning message: "Microsoft.Extensions.* 9.0.3 doesn't support net7.0-windows and has not been tested with it. Consider upgrading your TargetFramework to net8.0 or later."

### Dependencies Analysis
- VRChat.API (1.19.1): Supports multiple .NET versions including net7.0, net8.0, and net9.0
- MaterialDesignThemes (5.2.1): Compatible with newer .NET versions
- Test packages have newer versions available but are not blocking the upgrade

## Recommended Upgrade Path

### Target Version: .NET 8.0
- **Rationale**: 
  - .NET 8.0 is an LTS (Long-Term Support) version with support until November 2026
  - All project dependencies are compatible with .NET 8.0
  - Will resolve the compatibility warnings with Microsoft.Extensions packages
  - More stable and widely adopted than .NET 9.0 (which is newer)

### Alternative Considered: .NET 9.0
- While .NET 9.0 is also supported by our dependencies, it's newer and may have:
  - Less community support and resources for troubleshooting
  - Fewer third-party libraries fully tested with it
  - Shorter support timeline compared to .NET 8.0 LTS

## Implementation Plan

### 1. Update Project Files
- Update TargetFramework in VRChatInstanceJoiner.csproj from `net7.0-windows` to `net8.0-windows`
- Update TargetFramework in VRChatInstanceJoiner.Tests.csproj from `net7.0-windows` to `net8.0-windows`

### 2. Update Test Package References
- Update test packages to their latest versions:
  - coverlet.collector: 3.2.0 → 6.0.4
  - Microsoft.NET.Test.Sdk: 17.7.1 → 17.13.0
  - xunit: 2.4.2 → 2.9.3
  - xunit.runner.visualstudio: 2.4.5 → 3.0.2

### 3. Testing Strategy
- Run all unit tests to verify functionality after the upgrade
- Verify that the application builds without warnings
- Test the application manually to ensure all features work as expected

### 4. Documentation Updates
- Update README.md to reflect the new .NET version requirement
- Update architecture.md to specify .NET 8 as the target framework
- Update prompt_plan.md to reference .NET 8
- Update todo.md to mark the upgrade as completed

## Risks and Mitigations

### Potential Risks
1. **Breaking Changes**: .NET 8 might introduce breaking changes compared to .NET 7
   - **Mitigation**: Review the [.NET 8 breaking changes documentation](https://learn.microsoft.com/en-us/dotnet/core/compatibility/8.0) and address any issues

2. **WPF Compatibility**: Ensure WPF features used in the application are compatible with .NET 8
   - **Mitigation**: Test all UI functionality thoroughly after the upgrade

3. **Third-party Library Compatibility**: Some libraries might have issues with .NET 8
   - **Mitigation**: The analysis shows all current dependencies support .NET 8, but thorough testing is still required

## Post-Upgrade Verification

1. Verify the application builds without warnings
2. Run all unit tests and ensure they pass
3. Test the application manually, focusing on:
   - Authentication flow
   - Group selection and monitoring
   - Instance joining functionality
   - UI responsiveness and theme switching