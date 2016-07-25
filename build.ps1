dotnet restore

dotnet build src/RigoFunc.Account
dotnet build src/Host

dotnet test test/RigoFunc.Account.IntegrationTests
dotnet test test/RigoFunc.Account.UnitTests
