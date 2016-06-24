using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RigoFunc.Account;
using RigoFunc.Account.Services;

namespace Microsoft.Extensions.DependencyInjection {
    /// <summary>
    /// Contains extension methods to <see cref="IServiceCollection"/> for configuring identity services.
    /// </summary>
    public static class IServiceCollectionExtensions {
        /// <summary>
        /// Configures the account API.
        /// </summary>
        /// <typeparam name="TUser">The type representing a User in the system.</typeparam>
        /// <param name="services">The services available in the application.</param>
        /// <param name="setupAction">An action to configure the <see cref="ApiOptions"/>.</param>
        /// <returns>The services available in the application.</returns>
        public static IServiceCollection ConfigureAccountApi<TUser>(this IServiceCollection services, Action<ApiOptions> setupAction)
            where TUser : class {
            // TODO: have any better way? Such as define an IUser { string UserName { get; set; } } interface? and than with constraint where TUser : IUser
            // check the pre-requirement
            var constructors = typeof(TUser).GetTypeInfo().DeclaredConstructors;
            var found = false; 
            foreach (var ctor in constructors) {
                var parameters = ctor.GetParameters();
                if(parameters == null || parameters.Length != 1) {
                    continue;
                }

                var parameter = parameters[0];
                if(parameter.ParameterType == typeof(string) && parameter.Name.Equals("userName", StringComparison.CurrentCultureIgnoreCase)) {
                    found = true;
                }
            }

            if (!found) {
                throw new NotSupportedException($"The TUser: {nameof(TUser)} must define a constructor have and only have one string parameter with name 'userName'");
            }

            if (setupAction != null) {
                services.Configure(setupAction);
            }
            
            // try add default account service, or use the end-user DI in startup.cs
            services.TryAddTransient<IAccountService, DefaultAccountService<TUser>>();

            return services;
        }
    }
}
