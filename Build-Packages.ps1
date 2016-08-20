param(

    [Parameter(Mandatory=$true)]
    [string]$OutputDirectory,
    
    [Parameter(Mandatory=$true)]
    [string]$BuildSourceDirectory,
    
    [Parameter(Mandatory=$true)]
    [string]$BuildConfiguration,

    [Parameter(Mandatory=$true)]
    [string]$Version,

    [bool]$UpdateVersion,

    [string]$NuGetExe

)

# output version
Write-Host "Version: $Version"

if ($OutputDirectory -and !(Test-Path $OutputDirectory))
{
    Write-Host "Output directory used but doesn't exists, creating it"
    New-Item $OutputDirectory -type directory
}

# search for nuspec files
$NuSpecFiles = Get-Item **\*.nuspec

# display found files
Write-Host "Found nuspec files: $($NuSpecFiles.Count)"
foreach ($NuSpecFile in $NuSpecFiles)
{
    Write-Host "--File: `"$NuSpecFile`""
}

# discover location of NuGet.exe from agent if not specified
if ([string]::IsNullOrWhiteSpace($NuGetExe))
{
    $NuGetExe = "$($env:AGENT_HOMEDIRECTORY)\externals\nuget\nuget.exe"
}

# test nuget
Write-Host "Path to NuGet.exe: $($NuGetExe)"
if (!(Test-Path $NuGetExe))
{
    Write-Error "Could not locate NuGet.exe"
    exit 1
}

# run packaging
foreach ($i in $NuSpecFiles)
{
    $f = [System.IO.Path]::ChangeExtension($i.FullName, ".csproj")
    if (Test-Path $f)
    {
        # manually update version to support $version$ elsewhere in file
        if ($UpdateVersion -eq $true) {

            # read packages file
            $pkg = [xml](Get-Content ([System.io.Path]::Combine(([System.IO.Path]::GetDirectoryName($i)), "packages.config")))

            $nsm = @{ "n" = "http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd" }
            $xml = [xml](Get-Content $i)

            # replace package version
            $ver = Select-Xml -Xml $xml -Namespace $nsm -XPath '//n:package/n:metadata/n:version'
            $ver.Node.InnerText = $Version

            # for each $version$ dependency, set to currently referenced version
            foreach ($p in Select-Xml -Xml $xml -Namespace $nsm -XPath '//n:package/n:metadata/n:dependencies/n:dependency') {
                $n = $p.Node.GetAttribute("id")
                $v = $p.Node.GetAttribute("version")
                if ($v -eq '$version$') {
                    $a = $pkg.SelectSingleNode("//packages/package[@id='" + $n + "']")
                    if ($a) {
                        $p.Node.SetAttribute('version', $a.GetAttribute('version'))
                    }
                }
            }

            # replace nuspec content
            $xml.OuterXml | Set-Content $i
        }

        & $NuGetExe pack -OutputDirectory `"$OutputDirectory`" -Version `"$Version`" -Properties Configuration=$BuildConfiguration `"$f`"
    }
}
