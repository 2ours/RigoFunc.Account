# RigoFunc.Account

[![Join the chat at https://gitter.im/xyting/RigoFunc.Account](https://badges.gitter.im/xyting/RigoFunc.Account.svg)](https://gitter.im/xyting/RigoFunc.Account?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

Account Api abstraction library that uses ASP.NET Core Identity as its identity management.

# Feature
- [x] 使用手机号码注册(包含发送验证码) SendCode,Register
- [x] 使用用户名密码登录 Login
- [x] 使用验证码登录(包含判断是否可以使用验证码登录,以及该功能的开启设置) SendCode,VerifyCode
- [ ] 支持国际手机号号码

# Install
[nuget package](https://www.nuget.org/packages/RigoFunc.Account/)

`Install-Package RigoFunc.Account`

# Usage

```
services.UseDefaultAccountService();
```
```
POST:
api/account/register
{
    Username:"username",
    Password:"password"
}
```

# Swagger UI to view api

- run Host Project
- open [http://localhost:57567/swagger/ui](http://localhost:57567/swagger/ui)
