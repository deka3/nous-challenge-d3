using CleaningManagement.DAL.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleaningManagement.DAL.Repositories
{
    /// <summary>
    /// CleaningPlan repository interface.
    /// </summary>
    public interface ICleaningPlanRepository : IDisposable
    {
        /// <summary>
        /// Creates the new cleaning plan.
        /// </summary>
        /// <param name="cleaningPlan">The cleaning plan.</param>
        /// <returns>Id of new cleaning plan.</returns>
        Guid CreateNewCleaningPlan(CleaningPlan cleaningPlan);

        /// <summary>
        /// Deletes the cleaning plan.
        /// </summary>
        /// <param name="cleaningPlan">The cleaning plan.</param>
        /// <returns>True if successful.</returns>
        bool DeleteCleaningPlan(CleaningPlan cleaningPlan);

        /// <summary>
        /// Deletes the cleaning plan.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>True if successful.</returns>
        bool DeleteCleaningPlan(Guid id);

        /// <summary>
        /// Gets the cleaning plan by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Cleaning plan.</returns>
        CleaningPlan GetCleaningPlanById(Guid id);

        /// <summary>
        /// Gets the cleaning plans by customer identifier asynchronuosly.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="skip">The skip.</param>
        /// <param name="take">The take.</param>
        /// <returns>Collection of cleaning plans for given customer identifier.</returns>
        Task<IEnumerable<CleaningPlan>> GetCleaningPlansByCustomerIdAsync(int id, int skip = 0, int take = 0);

        /// <summary>
        /// Updates the cleaning plan.
        /// </summary>
        /// <param name="cleaningPlan">The cleaning plan.</param>
        /// <returns>True if successful.</returns>
        bool UpdateCleaningPlan(CleaningPlan cleaningPlan);
    }
}