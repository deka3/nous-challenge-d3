using CleaningManagement.Api.Controllers;
using CleaningManagement.Service.Dto;
using CleaningManagement.Service.Infrastructure.Result;
using CleaningManagement.Service.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CleaningManagement.Api.Tests
{
    public class CleaningPlansControllerTests : IDisposable
    {
        private readonly Guid _defaultId;
        private readonly CleaningPlansController _cleaningPlansController;
        private bool _isDisposed;

        public CleaningPlansControllerTests()
        {
            _defaultId = Guid.Parse("7228f2d2-936c-456b-adfc-332652cfd51d");

            var cleaningManagementServiceeMock = new Mock<ICleaningManagementService>();

            cleaningManagementServiceeMock.Setup(x => x.GetCleaningPlanById(It.IsAny<Guid>()))
                                          .Returns((Guid id) =>
                                          {
                                              if (id == Guid.Empty)
                                              {
                                                  return Result<CleaningPlan>.Fail(ResultEventType.NotFound);
                                              }

                                              return new CleaningPlan()
                                              {
                                                  Id = id,
                                                  Title = "Some title",
                                                  CustomerId = 1,
                                                  CreatedAt = DateTime.Now
                                              }.ToResult();
                                          });

            cleaningManagementServiceeMock.Setup(x => x.GetCleaningPlansByCustomerIdAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                                          .Returns((int id, int skip, int take) =>
                                          {
                                              IEnumerable<CleaningPlan> cleaningPlans = Enumerable.Empty<CleaningPlan>();

                                              if (id > 0)
                                              {
                                                  var list = new List<CleaningPlan>();

                                                  for (int i = 0; i < 50; i++)
                                                  {
                                                      var cp = new CleaningPlan()
                                                      {
                                                          CustomerId = id,
                                                          CreatedAt = DateTime.Now,
                                                          Id = Guid.NewGuid(),
                                                          Title = $"Title {i}"
                                                      };

                                                      list.Add(cp);
                                                  }

                                                  var temp = list.Skip(skip);

                                                  if (take > 0)
                                                  {
                                                      temp = temp.Take(take);
                                                  }

                                                  cleaningPlans = temp;
                                              }

                                              return Task.FromResult(cleaningPlans.ToResult());
                                          });

            cleaningManagementServiceeMock.Setup(x => x.DeleteCleaningPlan(It.IsAny<Guid>()))
                                          .Returns((Guid id) =>
                                          {
                                              if (id == Guid.Empty)
                                              {
                                                  return Result<bool>.Fail(ResultEventType.NotFound);
                                              }
                                              else if(id ==_defaultId)
                                              {
                                                  return Result<bool>.Fail(ResultEventType.BadRequest);
                                              }
                                              else
                                              {
                                                  return true.ToResult();
                                              }
                                          });

            cleaningManagementServiceeMock.Setup(x => x.CreateCleaningPlan(It.IsAny<CleaningPlan>()))
                                          .Returns((CleaningPlan cp) =>
                                          {
                                              if (cp.CustomerId < 1 || string.IsNullOrWhiteSpace(cp.Title))
                                              {
                                                  return Result<CleaningPlan>.Fail(ResultEventType.BadRequest, "Test message.");
                                              }

                                              cp.CreatedAt = DateTime.UtcNow;
                                              cp.Id = Guid.NewGuid();

                                              return cp.ToResult();
                                          });

            cleaningManagementServiceeMock.Setup(x => x.UpdateCleaningPlan(It.IsAny<Guid>(), It.IsAny<CleaningPlan>()))
                                          .Returns((Guid id, CleaningPlan cp) =>
                                          {
                                              if (id == Guid.Empty)
                                              {
                                                  return Result<CleaningPlan>.Fail(ResultEventType.NotFound);
                                              }

                                              if (cp == null || cp.CustomerId < 1 || string.IsNullOrWhiteSpace(cp.Title))
                                              {
                                                  return Result<CleaningPlan>.Fail(ResultEventType.BadRequest, "Test message.");
                                              }

                                              cp.CreatedAt = DateTime.UtcNow;
                                              cp.Id = id;

                                              return cp.ToResult();
                                          });

            var loggerMock = new Mock<ILogger<CleaningPlansController>>();

            this._cleaningPlansController = new CleaningPlansController(cleaningManagementServiceeMock.Object, loggerMock.Object)
            {
                Url = new Mock<IUrlHelper>().Object
            };
        }

        [Fact]
        public void GetCleaningPlanById_IfPlanDoesntExists_ReturnsNotFound()
        {
            var res = this._cleaningPlansController.GetCleaningPlanById(Guid.Empty);

            Assert.NotNull(res);
            Assert.Equal(typeof(NotFoundResult), res.GetType());
        }

        [Fact]
        public void GetCleaningPlanById_IfPlanExists_ReturnsOk()
        {
            var res = this._cleaningPlansController.GetCleaningPlanById(Guid.NewGuid());

            Assert.NotNull(res);
            Assert.Equal(typeof(OkObjectResult), res.GetType());
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-156)]
        public async void GetCleaningPlansByCustomeryId_IfCustomerDoesntExist_ReturnsEmptyCollection(int customerId)
        {
            var t = this._cleaningPlansController.GetCleaningPlansByCustomeryId(customerId);

            var res = await t;

            Assert.NotNull(res);
            Assert.Equal(typeof(OkObjectResult), res.GetType());
            Assert.Empty(((OkObjectResult)res).Value as IEnumerable<CleaningPlan>);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(567)]
        [InlineData(int.MaxValue)]
        public async void GetCleaningPlansByCustomeryId_IfCustomerExists_ReturnsCollection(int customerId)
        {
            var t = this._cleaningPlansController.GetCleaningPlansByCustomeryId(customerId);

            var res = await t;

            Assert.NotNull(res);
            Assert.Equal(typeof(OkObjectResult), res.GetType());
            Assert.NotEmpty(((OkObjectResult)res).Value as IEnumerable<CleaningPlan>);
        }

        [Fact]
        public void DeleteCleaningPlan_IfPlanDoesntExist_ReturnsNotFound()
        {
            var res = this._cleaningPlansController.DeleteCleaningPlan(Guid.Empty);

            Assert.NotNull(res);
            Assert.Equal(typeof(NotFoundResult), res.GetType());
        }

        [Fact]
        public void DeleteCleaningPlan_IfPlanExists_ReturnsNotFound()
        {
            var res = this._cleaningPlansController.DeleteCleaningPlan(Guid.NewGuid());

            Assert.NotNull(res);
            Assert.Equal(typeof(OkObjectResult), res.GetType());
            Assert.True((bool)((OkObjectResult)res).Value);
        }

        [Fact]
        public void CreateCleaningPlan_IfInvalidInput_RetursnBadRequest()
        {
            var res = this._cleaningPlansController.CreateCleaningPlan(null);

            Assert.NotNull(res);
            Assert.Equal(typeof(BadRequestObjectResult), res.GetType());

            res = this._cleaningPlansController.CreateCleaningPlan(new CleaningPlan());

            Assert.NotNull(res);
            Assert.Equal(typeof(BadRequestObjectResult), res.GetType());
        }

        [Fact]
        public void CreateCleaningPlan_IfInvalidInput_RetursnCreated()
        {
            var res = this._cleaningPlansController.CreateCleaningPlan(new CleaningPlan()
            {
                CustomerId = 1,
                Title = "TEST plan"
            });

            Assert.NotNull(res);
            Assert.Equal(typeof(CreatedResult), res.GetType());
        }

        [Fact]
        public void UpdateCleaningPlan_IfPlanDoesntExist_ReturnsNotFound()
        {
            var res = this._cleaningPlansController.UpdateCleaningPlan(Guid.Empty, new CleaningPlan());

            Assert.NotNull(res);
            Assert.Equal(typeof(NotFoundResult), res.GetType());
        }

        [Fact]
        public void UpdateCleaningPlan_IfPlanIsntValid_ReturnsBadRequest()
        {
            var res = this._cleaningPlansController.UpdateCleaningPlan(Guid.NewGuid(), null);

            Assert.NotNull(res);
            Assert.Equal(typeof(BadRequestObjectResult), res.GetType());

            res = this._cleaningPlansController.UpdateCleaningPlan(Guid.NewGuid(), new CleaningPlan());

            Assert.NotNull(res);
            Assert.Equal(typeof(BadRequestObjectResult), res.GetType());
        }

        [Fact]
        public void UpdateCleaningPlan_IfPlanIsValid_ReturnsBadRequest()
        {
            var res = this._cleaningPlansController.UpdateCleaningPlan(Guid.NewGuid(), new CleaningPlan()
            {
                CustomerId = 1,
                Title = "ABC"
            });

            Assert.NotNull(res);
            Assert.Equal(typeof(OkObjectResult), res.GetType());
            Assert.NotNull(((OkObjectResult)res).Value);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
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
