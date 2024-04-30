Import-Module dotenv
Set-DotEnv

$Config = "release"
$NugetLocation = "..\..\Artefacts\package\$Config\"
$Source = "https://api.nuget.org/v3/index.json"

$Packages = Get-ChildItem -Path $NugetLocation -Filter *.nupkg

ForEach ($Package in $Packages)
{
    dotnet nuget push $Packages --api-key $env:ApiKey --source $Source
}

Remove-DotEnv