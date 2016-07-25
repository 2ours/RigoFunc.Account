dotnet restore

dotnet build src/RigoFunc.Account
dotnet build src/Host

dotnet test test/RigoFunc.Account.IntegrationTests
dotnet test test/RigoFunc.Account.UnitTests

dotnet pack src/RigoFunc.Account

$project = Get-Content ".\src\RigoFunc.Account\project.json" | ConvertFrom-Json
$version = $project.version.Trim("-*")
nuget push src\RigoFunc.Account\bin\Debug\RigoFunc.Account.$version.nupkg -source nuget -apikey $env:NUGET_API_KEY