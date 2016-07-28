# RigoFunc.Account

[![Join the chat at https://gitter.im/xyting/RigoFunc.Account](https://badges.gitter.im/xyting/RigoFunc.Account.svg)](https://gitter.im/xyting/RigoFunc.Account?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

Account Api abstraction library that uses ASP.NET Core Identity as its identity management.

# Feature
- [x] Use phone number to register(include send code). see `SendCode` and `Register` API
- [x] Login with user name and password. see `Login` API
- [x] Use `CODE` to login(include the logic to determine whether COULD and ON/OFF or not). see `SendCode` and `VerifyCode` API
- [ ] Support the international phone number.

# Install
[nuget package](https://www.nuget.org/packages/RigoFunc.Account/)

`Install-Package RigoFunc.Account`

# Usage

```csharp
// Startup.cs
public void ConfigureServices(IServiceCollection services) {
    // ...
    var builder = services.AddAccountService<ApplicationUser>();
    // ...
}
```

POST: api/account/register
```JSON
{
    Username:"username",
    Password:"password"
}
```

# USE **Love.Net.Help** to view api
- Configure [here](https://github.com/lovedotnet/Love.Net.Help)
- Run Host Project
- Open [http://localhost:57567/api/help/ui](http://localhost:57567/api/help/ui)
