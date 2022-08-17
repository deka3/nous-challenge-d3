using CleaningManagement.DAL.Models;
using CleaningManagement.Service.Infrastructure;
using System;
using Xunit;

namespace CleaningManagement.Service.Tests
{
    public class AutoMapperTests
    {
        [Fact]
        public void AutoMapperUtility_ReturnsMapper()
        {
            var mapper = AutoMapperUtility.GetAutomapper();

            Assert.NotNull(mapper);
        }

        [Fact]
        public void CleaningPlanMapsCorrectlyTest()
        {
            var dtoCleaningPlan = new Dto.CleaningPlan()
            {
                CreatedAt = DateTime.Now,
                CustomerId = 1,
                Description = "Test description",
                Id = Guid.NewGuid(),
                Title = "Test plan"
            };
            
            var mapper = AutoMapperUtility.GetAutomapper();

            var dbCleaningPlan = mapper.Map<CleaningPlan>(dtoCleaningPlan);

            Assert.NotNull(dbCleaningPlan);
            Assert.Equal(dtoCleaningPlan.Id, dbCleaningPlan.Id);
            Assert.Equal(dtoCleaningPlan.Description, dbCleaningPlan.Description);
            Assert.Equal(dtoCleaningPlan.CreatedAt, dbCleaningPlan.CreatedAt);
            Assert.Equal(dtoCleaningPlan.CustomerId, dbCleaningPlan.CustomerId);
            Assert.Equal(dtoCleaningPlan.Title, dbCleaningPlan.Title);

            var newDtoCleaningPlan = mapper.Map<Dto.CleaningPlan>(dbCleaningPlan);

            Assert.NotNull(newDtoCleaningPlan);
            Assert.Equal(dbCleaningPlan.Id, newDtoCleaningPlan.Id);
            Assert.Equal(dbCleaningPlan.Description, newDtoCleaningPlan.Description);
            Assert.Equal(dbCleaningPlan.CreatedAt, newDtoCleaningPlan.CreatedAt);
            Assert.Equal(dbCleaningPlan.CustomerId, newDtoCleaningPlan.CustomerId);
            Assert.Equal(dbCleaningPlan.Title, newDtoCleaningPlan.Title);
        }
    }
}
