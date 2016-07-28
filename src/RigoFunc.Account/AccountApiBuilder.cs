using Microsoft.Extensions.DependencyInjection;
using RigoFunc.Account.Services;

namespace RigoFunc.Account {
    /// <summary>
    /// Helper functions for configuring account api.
    /// </summary>
    public class AccountApiBuilder {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccountApiBuilder"/> class.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to attach to.</param>
        public AccountApiBuilder(IServiceCollection services) {
            Services = services;
        }

        /// <summary>
        /// Gets the <see cref="IServiceCollection" /> services are attached to.
        /// </summary>
        /// <value>The <see cref="IServiceCollection" /> services are attached to.</value>
        public IServiceCollection Services { get; }

        /// <summary>
        /// Adds an <see cref="IAccountService"/>.
        /// </summary>
        /// <typeparam name="TService">The type of the t service.</typeparam>
        /// <returns>The current <see cref="AccountApiBuilder"/> instance.</returns>
        public virtual AccountApiBuilder AddAccountService<TService>() where TService : class, IAccountService {
            Services.AddScoped<IAccountService, TService>();
            return this;
        }

        /// <summary>
        /// Adds an <see cref="IAccessTokenProvider"/>.
        /// </summary>
        /// <typeparam name="TProvider">The type of the t provider.</typeparam>
        /// <returns>The current <see cref="AccountApiBuilder"/> instance.</returns>
        public virtual AccountApiBuilder AddAccessTokenProvider<TProvider>() where TProvider : class, IAccessTokenProvider {
            Services.AddScoped<IAccessTokenProvider, TProvider>();
            return this;
        }

        /// <summary>
        /// Adds an <see cref="IUserFactory{TUser}"/>.
        /// </summary>
        /// <typeparam name="TUser">The type of the t user.</typeparam>
        /// <typeparam name="TFactory">The type of the t factory.</typeparam>
        /// <returns>The current <see cref="AccountApiBuilder"/> instance.</returns>
        public virtual AccountApiBuilder AddUserFactory<TUser, TFactory>() where TFactory : class, IUserFactory<TUser> where TUser : class {
            Services.AddSingleton<IUserFactory<TUser>, TFactory>();
            return this;
        }
    }
}
