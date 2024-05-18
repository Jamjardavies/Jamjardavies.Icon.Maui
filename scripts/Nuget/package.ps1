$Config = "Release"
$Artefacts = "..\..\Artefacts"
$Sln = "..\..\src\Jamjardavies.Icon.Maui.sln"

if (Test-Path -Path $Artefacts)
{
    Remove-Item -Path "$Artefacts\*" -Recurse
}

dotnet pack $Sln -c $Config --artifacts-path $Artefacts --nologo -p:IncludeSymbols=true -p:SymbolPackageFormat=snupkg