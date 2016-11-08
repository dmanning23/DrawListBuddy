rm *.nupkg
nuget pack .\DrawlistBuddy.nuspec -IncludeReferencedProjects -Prop Configuration=Release
nuget push *.nupkg