namespace RigoFunc.Account.Services {
    /// <summary>
    /// Provides the interface(s) for creating user
    /// </summary>
    /// <typeparam name="TUser">The type of the user.</typeparam>
    public interface IUserFactory<TUser> where TUser : class {
        /// <summary>
        /// Creates a new user by the specified user name.
        /// </summary>
        /// <param name="userName">The name of the user.</param>
        /// <returns>A new instance of type <typeparamref name="TUser"/>.</returns>
        TUser CreateUser(string userName);
    }
}
