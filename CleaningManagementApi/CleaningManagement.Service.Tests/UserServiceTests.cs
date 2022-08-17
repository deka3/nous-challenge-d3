using CleaningManagement.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using CleaningManagement.DAL.Repositories;
using CleaningManagement.DAL.Models;

namespace CleaningManagement.Service.Tests
{
    public class UserServiceTests
    {
        private const string ActiveUserName = "activeuser";
        private readonly UserService _userService;

        public UserServiceTests()
        {
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.GetUserByUsernameAndPassword(It.IsAny<string>(), It.IsAny<string>()))
                              .Returns((string username, string password) =>
                              {
                                  if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
                                  {
                                      if (username == ActiveUserName)
                                      {
                                          return new User()
                                          {
                                              Username = username,
                                              Id = Guid.NewGuid(),
                                              IsActive = true,
                                              Email = "a@b.c"
                                          };
                                      }

                                      return null;
                                  }

                                  return null;
                              });

            this._userService = new UserService(userRepositoryMock.Object);
        }

        [Fact]
        public void UserService_IfRepositoryIsNull_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => new UserService(null));
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("", null)]
        [InlineData(null, "")]
        public void ValidateCredentials_IfInputIsNull_ReturnsFalse(string username, string password) 
        {
            bool res = _userService.ValidateCredentials(username, password);
            
            Assert.False(res);
        }

        [Theory]
        [InlineData("someuser", "password1")]
        [InlineData(ActiveUserName, "password2")]
        public void ValidateCredentials_IfInputIsValid_ReturnsValidationResult(string username, string password)
        {
            bool res = _userService.ValidateCredentials(username, password);

            Assert.Equal((ActiveUserName == username), res);
        }
    }
}
