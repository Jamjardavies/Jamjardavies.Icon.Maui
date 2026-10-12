$ErrorActionPreference = "Stop"

$Config = "Release"
$RepositoryRoot = (Resolve-Path (Join-Path $PSScriptRoot "..\..")).Path
$Artefacts = Join-Path $RepositoryRoot "Artefacts"
$SourceRoot = Join-Path $RepositoryRoot "src"
$Projects = @(
    Get-ChildItem -Path $SourceRoot -Filter "Jamjardavies.Icon.Maui*.csproj" -Recurse -File |
        Where-Object { $_.Name -notmatch "\.(Tests?|Generator)\.csproj$" } |
        Sort-Object -Property FullName
)

if ($Projects.Count -eq 0)
{
    throw "No Jamjardavies.Icon.Maui package projects were found in '$SourceRoot'."
}

if (Test-Path -Path $Artefacts)
{
    Get-ChildItem -Path $Artefacts -Force | Remove-Item -Recurse -Force
}

foreach ($Project in $Projects)
{
    Write-Host "Packing $($Project.Name)"

    dotnet pack $Project.FullName -c $Config --artifacts-path $Artefacts --nologo -p:IncludeSymbols=true -p:SymbolPackageFormat=snupkg

    if ($LASTEXITCODE -ne 0)
    {
        throw "Packing '$($Project.FullName)' failed with exit code $LASTEXITCODE."
    }
}
