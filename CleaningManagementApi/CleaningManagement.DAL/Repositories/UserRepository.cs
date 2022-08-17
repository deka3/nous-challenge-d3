using CleaningManagement.DAL.Models;
using System;
using System.Linq;
using System.Text;

namespace CleaningManagement.DAL.Repositories
{
    /// <summary>
    /// User repository.
    /// </summary>
    /// <seealso cref="CleaningManagement.DAL.Repositories.IUserRepository" />
    /// <seealso cref="System.IDisposable" />
    public class UserRepository : BaseRepository, IUserRepository
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="cleaningManagementDbContext">The cleaning management database context.</param>
        public UserRepository(CleaningManagementDbContext cleaningManagementDbContext) :
            base(cleaningManagementDbContext)
        {

        }

        /// <inheritdoc/>
        public User GetUserByUsernameAndPassword(string username, string password)
        {
            if (username == null || password == null)
            {
                return null;
            }

            return this._dbContext.Users.SingleOrDefault(u => u.Username == username && u.PasswordHash == GetPasswordHash(password) && u.IsActive == true);
        }

        /// <summary>
        /// Gets the password hash.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <returns>Password hash.</returns>
        /// <exception cref="System.ArgumentNullException">password</exception>
        public static string GetPasswordHash(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentNullException(nameof(password));
            }

            // some hashing logic goes here, for demo purposes conversion to base 64 string is good enough.
            return Convert.ToBase64String(Encoding.Unicode.GetBytes(password));
        }
    }
}
