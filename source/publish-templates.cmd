dotnet build ./ECF.Templates/ECFTemplates.csproj
nuget pack ./ECF.Templates/ECFTemplates.csproj
move ./ECF.Templates*.nupkg ../packages/