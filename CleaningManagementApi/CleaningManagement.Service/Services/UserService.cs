using CleaningManagement.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleaningManagement.Service.Services
{
    /// <summary>
    /// User service.
    /// </summary>
    /// <seealso cref="CleaningManagement.Service.Services.IUserService" />
    public class UserService : BaseService, IUserService
    {
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        public UserService(IUserRepository userRepository) :
            base(userRepository)
        {
            if (userRepository == null)
            {
                throw new ArgumentNullException(nameof(userRepository));
            }

            _userRepository = userRepository;
        }

        /// <inheritdoc/>
        public bool ValidateCredentials(string username, string password)
        {
            if (username == null || password == null)
            {
                return false;
            }


            return _userRepository.GetUserByUsernameAndPassword(username, password) != null;
        }
    }
}
