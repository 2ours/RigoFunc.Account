using System;
using System.Reflection;
using RigoFunc.Account.Services;

namespace RigoFunc.Account.Default {
    /// <summary>
    /// Represents the default implementation of <see cref="IUserFactory{TUser}"/> interface.
    /// </summary>
    /// <typeparam name="TUser">The type of the user.</typeparam>
    internal class DefaultUserFactory<TUser> : IUserFactory<TUser> where TUser : class {
        private enum Strategy {
            Unknown,
            Ctor,
            Property,
            Throw,
        }

        private static Strategy _strategy = Strategy.Unknown;

        /// <summary>
        /// Creates a new user by the specified user name.
        /// </summary>
        /// <param name="userName">The name of the user.</param>
        /// <returns>A new instance of type <typeparamref name="TUser" />.</returns>
        public TUser CreateUser(string userName) {
            // have multi-thread issue?
            if(_strategy == Strategy.Unknown) {
                _strategy = Strategy.Throw;

                // check the pre-requirement
                var constructors = typeof(TUser).GetTypeInfo().DeclaredConstructors;
                var found = false;
                foreach (var ctor in constructors) {
                    var parameters = ctor.GetParameters();
                    if (parameters == null || parameters.Length != 1) {
                        continue;
                    }

                    var parameter = parameters[0];
                    if (parameter.ParameterType == typeof(string) && parameter.Name.Equals("userName", StringComparison.CurrentCultureIgnoreCase)) {
                        _strategy = Strategy.Ctor;
                        found = true;
                    }
                }

                // haven't ctor.
                if (!found) {
                    var property = typeof(TUser).GetProperty("UserName", 
                        BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty);
                   
                    if(property != null) {
                        _strategy = Strategy.Property;
                    }
                }
            }
            
            switch (_strategy) {
                case Strategy.Ctor:
                    return Activator.CreateInstance(typeof(TUser), userName) as TUser;
                case Strategy.Property:
                    var userType = typeof(TUser);
                    var user = Activator.CreateInstance(userType, true) as TUser;
                    var property = userType.GetProperty("UserName");
                    property.SetValue(user, userName);
                    return user;
                case Strategy.Unknown:
                case Strategy.Throw:
                default:
                    throw new NotSupportedException($"The TUser: {nameof(TUser)} must define a constructor have and only have one string parameter named 'userName' or have a writable property named 'UserName'");
            }
        }
    }
}
