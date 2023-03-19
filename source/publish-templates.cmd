dotnet build ./ECF.Templates/ECF.Templates.csproj
nuget pack ./ECF.Templates/ECF.Templates.csproj
move ./ECF.Templates*.nupkg ../packages/