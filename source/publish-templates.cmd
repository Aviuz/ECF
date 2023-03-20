dotnet build ./ECF.Templates/ECFTemplates.csproj
nuget pack ./ECF.Templates/ECFTemplates.csproj
move ./ECFTemplates*.nupkg ../packages/