# Setup project to use C# utilities.
include(CSharpUtilities)

project(BeatLibraryTest)

configure_file(mocks/mock_Settings.cs.in mocks/mock_Settings.cs @ONLY)

set(SOURCE_FILES
    ${CMAKE_SOURCE_DIR}/src/DataClasses.cs
    ${CMAKE_SOURCE_DIR}/src/Scanner.cs
)
set(MOC_SOURCE_FILES
    ${CMAKE_CURRENT_BINARY_DIR}/mocks/mock_Settings.cs
    mocks/mock_App.cs
)

set(TEST_SOURCE_FILES
    Scanner_tests.cs
)

source_group("source" FILES ${SOURCE_FILES})

add_library(${PROJECT_NAME} SHARED ${SOURCE_FILES} ${MOC_SOURCE_FILES} ${TEST_SOURCE_FILES})

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
    BeatSaverSharp_2.0.1
    NUnit_3.13.1
    NUnit.Console_3.12.0
)

# Set CLR assembly properties.
set_target_properties(${PROJECT_NAME} PROPERTIES
	WIN32_EXECUTABLE TRUE
    VS_DOTNET_REFERENCES "${DOTNET_REF}"
    VS_PACKAGE_REFERENCES "${NUGET_PKGS}"
    VS_GLOBAL_ROOTNAMESPACE ${PROJECT_NAME}
)