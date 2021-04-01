# Setup project to use C# utilities.
include(CSharpUtilities)

set(SOURCE_FILES
	App.xaml
	App.xaml.cs

    Database.cs
    Settings.cs

    MainWindow.xaml
    MainWindow.xaml.cs
    BeatmapDetailWindow.xaml
    BeatmapDetailWindow.xaml.cs
    SettingsWindow.xaml
    SettingsWindow.xaml.cs
    ScanWindow.xaml
    ScanWindow.xaml.cs
)

# Add executable project.
add_executable(${PROJECT_NAME} ${SOURCE_FILES})

# Set Windows Forms properties
csharp_set_xaml_cs_properties(${SOURCE_FILES})

set_property(SOURCE App.xaml PROPERTY VS_XAML_TYPE "ApplicationDefinition")

set(DOTNET_REF
	System
    System.Core
    System.Data
    System.Data.DataSetExtensions
    System.Drawing
    System.Net.Http
    System.Xaml
    System.Xml.Linq
    Microsoft.CSharp
    PresentationCore
    PresentationFramework
    WindowsBase
)

set(NUGET_PKGS
	Sentry_3.2.0
	MahApps.Metro_2.4.4
    LiteDB_5.0.10
    BeatSaverSharp_2.0.1
    NVorbis_0.10.1
)

# Set CLR assembly properties.
set_target_properties(${PROJECT_NAME} PROPERTIES
	WIN32_EXECUTABLE TRUE
    VS_DOTNET_REFERENCES "${DOTNET_REF}"
    VS_PACKAGE_REFERENCES "${NUGET_PKGS}"
    VS_GLOBAL_ROOTNAMESPACE ${PROJECT_NAME}
)