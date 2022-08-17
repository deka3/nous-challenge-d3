using CleaningManagement.Service.Dto;
using CleaningManagement.Service.Infrastructure;
using CleaningManagement.Service.Infrastructure.Result;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleaningManagement.Service.Services
{
    /// <summary>
    /// Cleaning management service interface.
    /// </summary>
    public interface ICleaningManagementService
    {
        /// <summary>
        /// Creates the cleaning plan.
        /// </summary>
        /// <param name="cleaningPlan">The cleaning plan.</param>
        /// <returns>New cleaning plan.</returns>
        Result<CleaningPlan> CreateCleaningPlan(CleaningPlan cleaningPlan);

        /// <summary>
        /// Deletes the cleaning plan.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>True if successful, otherwhise false.</returns>
        Result<bool> DeleteCleaningPlan(Guid id);

        /// <summary>
        /// Gets the cleaning plan by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Cleaning plan.</returns>
        Result<CleaningPlan> GetCleaningPlanById(Guid id);

        /// <summary>
        /// Gets the cleaning plans by customer identifier asynchronuosly.
        /// </summary>
        /// <param name="cutomerId">The cutomer identifier.</param>
        /// <param name="skip">The skip.</param>
        /// <param name="take">The take.</param>
        /// <returns>Collection of cleaning plans, if user doesn't exist returns empty collection.</returns>
        Task<Result<IEnumerable<Dto.CleaningPlan>>> GetCleaningPlansByCustomerIdAsync(int cutomerId, int skip = 0, int take = 0);

        /// <summary>
        /// Updates the cleaning plan.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="cleaningPlan">The cleaning plan.</param>
        /// <returns>Updated cleaning plan, or null if not exists.</returns>
        Result<CleaningPlan> UpdateCleaningPlan(Guid id, CleaningPlan cleaningPlan);
    }
}