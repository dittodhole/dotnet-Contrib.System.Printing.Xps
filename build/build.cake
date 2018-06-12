#tool "nuget:?package=GitVersion.CommandLine"

var configuration = "Release";
var artifactsDirectory = Directory("../artifacts");
var sourceDirectory = Directory("../src");
var solutionFile = sourceDirectory + File("Contrib.System.Printing.Xps.sln");
var projectFile = sourceDirectory + Directory("Contrib.System.Printing.Xps") + File("Contrib.System.Printing.Xps.csproj");
var assemblyInfoFile = sourceDirectory + Directory("Contrib.System.Printing.Xps") +  Directory("Properties") + File("AssemblyInfo.cs");

Task("Build")
  .IsDependentOn("Clean")
  .IsDependentOn("Version")
  .IsDependentOn("Restore")
  .Does(() =>
{
  Information($"Building {MakeAbsolute(solutionFile)}");

  MSBuild(solutionFile,
          settings => settings.SetConfiguration(configuration)
                              .SetPlatformTarget(PlatformTarget.MSIL)
                              .SetMaxCpuCount(0)
                              .SetMSBuildPlatform(MSBuildPlatform.x86));

  NuGetPack(projectFile,
            new NuGetPackSettings
            {
              Properties = new Dictionary<string, string>
              {
                { "Configuration", configuration },
                { "branch", EnvironmentVariable("APPVEYOR_REPO_BRANCH") ?? "develop" }
              },
              IncludeReferencedProjects = true,
              Symbols = true,
              OutputDirectory = artifactsDirectory
            });
});

Task("Clean")
  .Does(() =>
{
  Information($"Cleaning {MakeAbsolute(artifactsDirectory)}");

  if (DirectoryExists(artifactsDirectory))
  {
    DeleteDirectory(artifactsDirectory,
                    new DeleteDirectorySettings
                    {
                      Recursive = true
                    });
  }

  CreateDirectory(artifactsDirectory);
});

Task("Version")
  .Does(() =>
{
  Information($"Versioning {MakeAbsolute(solutionFile)}");

  GitVersion(new GitVersionSettings
             {
               UpdateAssemblyInfo = true,
               UpdateAssemblyInfoFilePath = assemblyInfoFile,
               OutputType = GitVersionOutput.BuildServer
             });
});

Task("Restore")
  .Does(() =>
{
  Information($"Restoring packages for {MakeAbsolute(solutionFile)}");

  NuGetRestore(solutionFile);
});

var targetArgument = Argument("target", "Build");
RunTarget(targetArgument);
