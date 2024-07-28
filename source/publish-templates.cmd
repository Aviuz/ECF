dotnet build ./ECF.Templates/ECFTemplates.csproj
dotnet pack ./ECF.Templates/ECFTemplates.csproj
move .\ECF.Templates\bin\Release\ECFTemplates*.nupkg ..\packages\
