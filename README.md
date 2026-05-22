# RenCloud

A professional-grade video editing application for Windows with an intuitive timeline-based interface and comprehensive media manipulation capabilities.

## Overview

RenCloud is a Windows-native video editor that combines ease of use with powerful editing features. The application provides a custom-built front-end engine enabling precise video and audio editing with real-time thumbnail generation for video tracks and waveform rendering for audio tracks. Whether you're a casual user trimming clips or a professional managing complex timelines, RenCloud delivers a responsive, isolated editing environment.

## Features

### Core Editing
- **Timeline-based editing** – Arrange and manipulate video and audio segments with precision
- **Video manipulation** – Trim, split, and arrange video clips on a dedicated track
- **Audio manipulation** – Full audio editing capabilities with dedicated waveform visualization
- **Media import/export** – Seamless import and export of media files

### Visual Feedback
- **Thumbnail generation** – Auto-generated thumbnails for video tracks enabling visual navigation
- **Waveform rendering** – Real-time audio waveform display for precise audio editing
- **Live preview** – Smooth, cross-platform media playback via VLC/LibVLC integration

### Technical Excellence
- **FFmpeg integration** – Advanced media processing and codec support
- **Code quality** – Continuous quality analysis via SonarQube integration
- **Extensible architecture** – Designed for future enhancements and plugin support

## Getting Started

### System Requirements
- Windows 10 or later
- .NET Framework (version specified in project)
- 2+ GB RAM recommended
- GPU support recommended for smoother playback

### Installation

**Via Installer (Recommended)**
1. Download the latest installer from [Releases](https://github.com/VladPocris/RenCloud/releases)
2. Run `RenCloud-Installer.msi`
3. Follow the installation wizard

**Manual Build**
1. Clone the repository
2. Open `RenCloud/RenCloud.sln` in Visual Studio
3. Build the solution in Release configuration
4. Run `RenCloud.exe` from the output directory

### Building from Source

#### Prerequisites
- Visual Studio 2022 or later with C# support
- MSBuild
- NuGet

#### Build Steps
```bash
# Restore dependencies
nuget restore ./RenCloud/RenCloud.sln

# Build the solution
msbuild ./RenCloud/RenCloud.sln /p:Configuration=Release

# Output directory
./RenCloud/bin/Release/
```

## Project Structure

```
RenCloud/
├── RenCloud/                 # Main application
│   ├── UserInterfaceForm.cs  # Primary UI and timeline engine
│   ├── LogInForm.cs          # Authentication UI
│   ├── RegisterForm.cs       # Registration UI
│   ├── LoadForm.cs           # Project loading UI
│   ├── FormManager.cs        # Form lifecycle management
│   ├── Media/                # Media handling classes
│   ├── Resources/            # Assets and resources
│   └── RenCloud.csproj       # Project configuration
├── TestRenCloud/             # Unit tests (MSTest framework)
├── Installer/                # WiX installer project
├── docs/                     # Documentation and research
└── References/               # Reference materials
```

## Development

### Testing

Run the test suite:
```bash
msbuild ./TestRenCloud/TestRenCloud.csproj /p:Configuration=Release
dotnet test ./TestRenCloud/TestRenCloud.csproj --configuration Release
```

Tests include coverage for:
- Form lifecycle and state management
- Media handling and processing
- Authentication and registration flows
- UI interactions and timeline operations

### Code Quality

Code quality is monitored via SonarQube integration. Reports are generated automatically on pull requests and merges to main.

### CI/CD Pipeline

The project uses GitHub Actions for continuous integration:
- Automatic builds on push to main and pull requests
- Comprehensive test execution with coverage reporting
- SonarQube analysis for code quality metrics
- Automated installer creation with WiX
- Release artifact packaging and publishing

## Technologies & Dependencies

### Core Technologies
- **Language**: C# (.NET Framework)
- **UI Framework**: Windows Forms
- **Build System**: MSBuild, NuGet
- **Testing**: MSTest with Coverlet coverage

### External Libraries
- **FFmpeg** – Media processing and transcoding
- **VLC/LibVLC** – Media playback engine
- **WiX Toolset** – Installer packaging
- **SonarQube** – Code quality analysis

## Architecture

RenCloud follows a modular, extensible architecture:

### Key Components
- **FormManager** – Orchestrates form lifecycle and navigation
- **UserInterfaceForm** – Timeline engine and primary editing interface
- **Authentication Layer** – Login, registration, and password reset flows
- **Media Layer** – Audio/video processing and manipulation

### Design Principles
- **Separation of Concerns** – UI logic isolated from business logic
- **Extensibility** – Plugin-ready architecture for future features
- **Performance** – Optimized rendering and media handling
- **User Experience** – Intuitive timeline interface with visual feedback

## Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/your-feature`)
3. Commit changes following project conventions
4. Push to your branch
5. Open a pull request against `main`

### Code Style
- Follow Microsoft C# coding conventions
- Use meaningful variable and method names
- Keep methods focused and testable
- Add tests for new functionality

## Releases

Releases are published automatically to [GitHub Releases](https://github.com/VladPocris/RenCloud/releases) and include:
- `RenCloud.zip` – Compiled application binaries
- `Installer.zip` – Windows installer package

## License

This project is open source.

## Support & Documentation

- **Issues**: Report bugs via [GitHub Issues](https://github.com/VladPocris/RenCloud/issues)
- **Documentation**: See `/docs` directory for detailed guides
- **Research**: See `/docs` for technical research and architectural decisions

## Future Roadmap

- Advanced audio filters and effects
- Multi-track video composition
- Batch processing capabilities
- Plugin architecture for community-developed features
- Performance optimizations for 4K+ video handling

## Authors

Vlad Pocris – Project Lead & Developer

## Acknowledgments

- FFmpeg team for media processing capabilities
- VLC project for playback engine
- Microsoft for C# and Windows Forms framework
- SonarQube team for code quality tools
