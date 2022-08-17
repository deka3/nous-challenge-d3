using CleaningManagement.DAL.Models;
using System;

namespace CleaningManagement.DAL.Repositories
{
    /// <summary>
    /// User repository interface.
    /// </summary>
    public interface IUserRepository : IDisposable
    {
        /// <summary>
        /// Gets the user by username and password.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>User with given crenedtials, or null user doesn't exist or is inactive.</returns>
        User GetUserByUsernameAndPassword(string username, string password);
    }
}