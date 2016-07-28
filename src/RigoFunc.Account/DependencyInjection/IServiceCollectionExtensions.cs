using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RigoFunc.Account;
using RigoFunc.Account.Default;
using RigoFunc.Account.Services;

namespace Microsoft.Extensions.DependencyInjection {
    /// <summary>
    /// Contains extension methods to <see cref="IServiceCollection"/> for configuring account services.
    /// </summary>
    public static class IServiceCollectionExtensions {
        /// <summary>
        /// Add the default account service to <see cref="IServiceCollection"/> in the application.
        /// </summary>
        /// <typeparam name="TUser">The type representing a User in the system.</typeparam>
        /// <param name="services">The services available in the application.</param>
        /// <param name="setupAction">An action to configure the <see cref="ApiOptions"/>.</param>
        /// <returns>An instance of <see cref="AccountApiBuilder"/> for further configuration account service.</returns>
        public static AccountApiBuilder AddAccountService<TUser>(this IServiceCollection services, Action<ApiOptions> setupAction = null)
            where TUser : class {
            if (setupAction != null) {
                services.Configure(setupAction);
            }

            services.TryAddSingleton<IUserFactory<TUser>, DefaultUserFactory<TUser>>();
            services.TryAddScoped<IAccountService, DefaultAccountService<TUser>>();
            services.TryAddScoped<IAccessTokenProvider, DefaultAccessTokenProvider>();

            return new AccountApiBuilder(services);
        }
    }
}
