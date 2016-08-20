foreach ($i in Get-Item *.nupkg)
{
    .\.nuget\NuGet.exe push $i 4475571d-eafd-4e4d-b3d6-21c0e4a481a3 -s https://www.myget.org/F/cogito/
}
