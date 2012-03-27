param(
	[Parameter(Mandatory=$true)][string] $version
)
$spacer = new-Object System.String('-', 80)

write-host "Building Simple.Validation in Release mode."
$cmd = "msbuild /p:Configuration=Release"
write-host $cmd
Invoke-Expression $cmd

write-host $spacer

write-host "Deleting existing packages"

$packFiles = Get-ChildItem *nupkg
if ($packFiles -ne $null)
{
	$packFiles | ForEach-Object {
		$fileName= $_.FullName
		write-host "Deleting file $fileName"
		[System.IO.File]::Delete($fileName)
	}
}
else
{
	write-host "No existing package files found."
}
write-host $spacer

write-host "Building NuGet Packages"
$nuspecFiles = Get-ChildItem *.nuspec

if ($nuspecFiles -ne $null)
{
	$nuSpecFiles | ForEach-Object {
		$fileName = $_.FullName
		$packCmd = "nuget pack $fileName -version $version"
		write-host $packCmd
		Invoke-Expression $packCmd
	}
}
write-host $spacer