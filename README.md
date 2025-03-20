# VRChat Instance Joiner

A Windows application that allows VRChat users to quickly join instances as they become available from a group.

## Overview

VRChat Instance Joiner is a desktop application designed to monitor VRChat group instances and help users join them as soon as they become available. This is particularly useful for popular events where instances fill up quickly.

### Key Features

- **Real-time Instance Monitoring**: Continuously checks for new group instances
- **Quick Join Capability**: Join instances as soon as they become available
- **Group Selection**: Choose which VRChat groups to monitor
- **Consistent Instance Selection**: Configurable algorithm to ensure multiple users can coordinate joining the same instance
- **Notifications**: Receive alerts when new instances are detected
- **Modern UI**: Clean, attractive interface with light/dark mode support

## Installation

### Requirements

- Windows 10 or later
- .NET 8.0 or later
- An active VRChat account

### Setup

1. Download the latest release from the [Releases](https://github.com/yourusername/vrchat-instance-joiner/releases) page
2. Extract the ZIP file to your preferred location
3. Run `VRChatInstanceJoiner.exe`

## Usage

### First Launch

1. When you first launch the application, you'll need to authenticate with your VRChat credentials
2. If your account has 2FA enabled, you'll be prompted to enter the verification code
3. The application will securely store your session for future use

### Monitoring Instances

1. Select a group from the dropdown list
2. Configure your instance selection preferences (optional)
3. Click "Start Monitoring"
4. When a new instance is detected, you'll receive a notification
5. Click "Join" to automatically join the instance

### Settings

- **Poll Interval**: Adjust how frequently the application checks for new instances
- **Auto-Join**: Automatically join new instances when detected
- **Instance Selection**: Configure how instances are prioritized
- **Notification Settings**: Customize alerts and sounds
- **Theme**: Choose between light and dark mode

## Security

This application uses the community-maintained VRChat API SDK. Please note:

- Your VRChat credentials are processed securely and not stored in plain text
- Authentication tokens are stored using the Windows Data Protection API
- All API communication is done over HTTPS

## Disclaimer

This application is not officially affiliated with or endorsed by VRChat, Inc. Use at your own risk and in accordance with VRChat's [Terms of Service](https://vrchat.com/home/terms) and [Community Guidelines](https://vrchat.com/home/community-guidelines).

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## Support

If you encounter any issues or have questions, please [open an issue](https://github.com/yourusername/vrchat-instance-joiner/issues) on GitHub.