dotnet build ./ECF/ECF.csproj -c Release
dotnet build ./ECF.Autofac/ECF.Autofac.csproj -c Release
dotnet build ./ECF.Microsoft.DependencyInjection/ECF.Microsoft.DependencyInjection.csproj -c Release
nuget pack ./ECF/.nuspec
nuget pack ./ECF.Autofac/.nuspec
nuget pack ./ECF.Microsoft.DependencyInjection/.nuspec
move ./ECF*.nupkg ../packages/
move ./EasyConsoleFramework*.nupkg ../packages/