dotnet build ./ECF/ECF.csproj -c Release
nuget pack ./ECF/.nuspec
move ./ECF*.nupkg ../packages/