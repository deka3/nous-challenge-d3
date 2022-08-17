using CleaningManagement.DAL.Repositories;
using CleaningManagement.Service.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CleaningManagement.Service.Tests
{
    public  class CleaningManagementServiceTests:IDisposable
    {
        private const string DefaultPlanName = "Test plan 101";
        private const string DefaultPlanDescription = "Lorem ipsum";

        private bool _isDisposed;
        private CleaningManagementService _service;

        public CleaningManagementServiceTests()
        {
            var repositoryMock = new Mock<ICleaningPlanRepository>();
            repositoryMock.Setup(x => x.GetCleaningPlanById(It.IsAny<Guid>()))
                          .Returns((Guid id) =>
                          {
                              if (id != Guid.Empty)
                              {
                                  return new DAL.Models.CleaningPlan()
                                  {
                                      Id = id,
                                      CreatedAt = DateTime.Now,
                                      CustomerId = 1,
                                      Description = DefaultPlanDescription,
                                      Title = DefaultPlanName
                                  };
                              }

                              return null;
                          });

            repositoryMock.Setup(x => x.GetCleaningPlansByCustomerIdAsync(It.IsAny<int>(), 0, 0))
                          .Returns((int id, int skip, int take) =>
                          {
                              IEnumerable<DAL.Models.CleaningPlan> planCollection = Enumerable.Empty<DAL.Models.CleaningPlan>();

                              if (id > 0)
                              {
                                  planCollection =
                                   new List<DAL.Models.CleaningPlan>()
                                   {
                                        new DAL.Models.CleaningPlan()
                                        {
                                                Id = Guid.NewGuid(),
                                                CreatedAt = DateTime.Now,
                                                CustomerId = id,
                                                Description = "Lorem ipsum",
                                                Title = "Test plan 101"
                                        },
                                        new DAL.Models.CleaningPlan()
                                        {
                                                Id = Guid.NewGuid(),
                                                CreatedAt = DateTime.Now,
                                                CustomerId = id,
                                                Description = string.Empty,
                                                Title = "Test plan 102"
                                        },
                                   };
                              }

                              return Task.FromResult(planCollection);
                          });

            repositoryMock.Setup(x => x.DeleteCleaningPlan(It.IsAny<Guid>()))
                          .Returns((Guid id) => id != Guid.Empty);

            repositoryMock.Setup(x => x.CreateNewCleaningPlan(It.IsAny<DAL.Models.CleaningPlan>()))
                          .Returns(Guid.NewGuid());

            repositoryMock.Setup(x => x.UpdateCleaningPlan(It.IsAny<DAL.Models.CleaningPlan>()))
                          .Returns(true);

            this._service = new CleaningManagementService(repositoryMock.Object);
        }

        [Fact]
        public void CleaningManagementService_IfRepositoryIsNull_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => new CleaningManagementService(null));
        }

        [Fact]
        public void CreateCleaningPlan_IfInputIsInvalid_ThrowsException()
        {
            Dto.CleaningPlan cleaningPlan = null;
            Assert.Throws<ArgumentNullException>(() => this._service.CreateCleaningPlan(cleaningPlan));

            cleaningPlan = new Dto.CleaningPlan()
            {
                Title = null
            };
            Assert.Throws<ArgumentException>(() => this._service.CreateCleaningPlan(cleaningPlan));

            cleaningPlan.Title = string.Empty;
            Assert.Throws<ArgumentException>(() => this._service.CreateCleaningPlan(cleaningPlan));

            cleaningPlan.Title = "      ";
            Assert.Throws<ArgumentException>(() => this._service.CreateCleaningPlan(cleaningPlan));

            cleaningPlan.Title = "Valid title";
            Assert.Throws<ArgumentException>(() => this._service.CreateCleaningPlan(cleaningPlan));

            cleaningPlan.CustomerId = 1;

            var sb = new StringBuilder();
            for (int i = 0; i < 16; i++)
            {
                sb.Append("ABCDEFGHIJKLMNOP");
            }

            sb.Append("THE END");

            cleaningPlan.Title = sb.ToString();
            Assert.Throws<ArgumentException>(() => this._service.CreateCleaningPlan(cleaningPlan));

            sb.Append(cleaningPlan.Title);
            cleaningPlan.Title = "OK Title";
            cleaningPlan.Description = sb.ToString();
            Assert.Throws<ArgumentException>(() => this._service.CreateCleaningPlan(cleaningPlan));
        }

        [Fact]
        public void UpdateCleaningPlan_IfInputIsInvalid_ThrowsException()
        {
            Dto.CleaningPlan cleaningPlan = null;
            Assert.Throws<ArgumentNullException>(() => this._service.UpdateCleaningPlan(Guid.Empty, cleaningPlan));

            cleaningPlan = new Dto.CleaningPlan()
            {
                Title = null
            };
            Assert.Throws<ArgumentException>(() => this._service.UpdateCleaningPlan(Guid.NewGuid(), cleaningPlan));

            cleaningPlan.Title = string.Empty;
            Assert.Throws<ArgumentException>(() => this._service.UpdateCleaningPlan(Guid.NewGuid(), cleaningPlan));

            cleaningPlan.Title = "      ";
            Assert.Throws<ArgumentException>(() => this._service.UpdateCleaningPlan(Guid.NewGuid(), cleaningPlan));

            cleaningPlan.Title = "Valid title";
            Assert.Throws<ArgumentException>(() => this._service.UpdateCleaningPlan(Guid.NewGuid(), cleaningPlan));
        }

        [Fact]
        public void GetCleaningPlanById_IfPlanDoesntExists_ReturnsNull()
        {
            var res = this._service.GetCleaningPlanById(Guid.Empty);
            Assert.NotNull(res);
            Assert.False(res.IsSuccess);
            Assert.Null(res.Value);
        }

        [Fact]
        public void GetCleaningPlanById_IfPlanExists_ReturnsPlanDto()
        {
            var res = this._service.GetCleaningPlanById(Guid.NewGuid());

            Assert.NotNull(res);
            Assert.True(res.IsSuccess);
            Assert.NotNull(res.Value);
        }

        [Theory]
        [InlineData(-5)]
        [InlineData(0)]
        [InlineData(int.MinValue)]
        public async void GetCleaningPlansByCustomerId_IfUserDoesntExists_ReturnsEmptyCollection(int customerId)
        {
            var t = this._service.GetCleaningPlansByCustomerIdAsync(customerId);

            var res = await t;

            Assert.NotNull(res);
            Assert.NotNull(res.Value);
            Assert.Empty(res.Value);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(8)]
        [InlineData(int.MaxValue)]
        public async void GetCleaningPlansByCustomerId_IfUserExists_ReturnsCollection(int customerId)
        {
            var t = this._service.GetCleaningPlansByCustomerIdAsync(customerId);

            var res = await t;

            Assert.NotNull(res);
            Assert.NotNull(res.Value);
            Assert.NotEmpty(res.Value);
        }

        [Fact]
        public void DeleteCleaningPlan_Test()
        {
            var res = this._service.DeleteCleaningPlan(Guid.Empty);

            Assert.NotNull(res);
            Assert.False(res.Value);

            res = this._service.DeleteCleaningPlan(Guid.NewGuid());

            Assert.NotNull(res);
            Assert.True(res.Value);
        }

        [Fact]
        public void CreateCleaningPlan_IfInputIsValid_ReturnsNewPlanDto()
        {
            var inputPlan = new Dto.CleaningPlan()
            {
                CustomerId = 1,
                Description = DefaultPlanDescription,
                Title = DefaultPlanName
            };

            var res = this._service.CreateCleaningPlan(inputPlan);

            Assert.NotNull(res);
            Assert.NotNull(res.Value);
            Assert.NotEqual(Guid.Empty, res.Value.Id);
            Assert.Equal(inputPlan.CustomerId, res.Value.CustomerId);
            Assert.Equal(inputPlan.Description, res.Value.Description);
            Assert.Equal(inputPlan.Title, res.Value.Title);
        }

        [Fact]
        public void UpdateCleaningPlan_IfInputIsValid_ReturnsNewPlanDto()
        {
            var inputPlan = new Dto.CleaningPlan()
            {
                CustomerId = 1,
                Description = "ABC",
                Title = "New plan",
                Id = Guid.NewGuid()
            };

            var res = this._service.UpdateCleaningPlan(inputPlan.Id, inputPlan);

            Assert.NotNull(res);
            Assert.Equal(inputPlan.Id, res.Value.Id);
            Assert.Equal(inputPlan.CustomerId, res.Value.CustomerId);
            Assert.Equal(inputPlan.Description, res.Value.Description);
            Assert.Equal(inputPlan.Title, res.Value.Title);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    this._service.Dispose();
                }

                _isDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
