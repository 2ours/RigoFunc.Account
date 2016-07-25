dotnet restore

dotnet build src/RigoFunc.Account
dotnet build src/Host

dotnet test test/RigoFunc.Account.IntegrationTests
dotnet test test/RigoFunc.Account.UnitTests

dotnet pack src/RigoFunc.Account

nuget push src\RigoFunc.Account\bin\Debug\RigoFunc.Account.%1.nupkg -source nuget -apikey %NUGET_API_KEY%