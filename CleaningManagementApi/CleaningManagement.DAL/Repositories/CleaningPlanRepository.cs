using CleaningManagement.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleaningManagement.DAL.Repositories
{
    /// <summary>
    /// CleaningPlan repository.
    /// </summary>
    /// <seealso cref="CleaningManagement.DAL.Repositories.ICleaningPlanRepository" />
    /// <seealso cref="System.IDisposable" />
    public class CleaningPlanRepository : BaseRepository, ICleaningPlanRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CleaningPlanRepository"/> class.
        /// </summary>
        /// <param name="cleaningManagementDbContext">The cleaning management database context.</param>
        public CleaningPlanRepository(CleaningManagementDbContext cleaningManagementDbContext) :
            base(cleaningManagementDbContext)
        {
        }

        /// <inheritdoc/>
        public CleaningPlan GetCleaningPlanById(Guid id)
        {
            return this._dbContext.Find<CleaningPlan>(id);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<CleaningPlan>> GetCleaningPlansByCustomerIdAsync(int id, int skip = 0, int take = 0)
        {
            var query = this._dbContext.CleaningPlans.Where(x => x.CustomerId == id);

            if (skip > 0)
            {
                query = query.Skip(skip);   
            }

            if (take > 0)
            {
                query = query.Take(take);
            }

            return await query.ToListAsync();
        }

        /// <inheritdoc/>
        public bool DeleteCleaningPlan(Guid id)
        {
            var cleaningPlan = this.GetCleaningPlanById(id);

            return HandleCleaningPlanDeletion(cleaningPlan);
        }

        /// <inheritdoc/>
        public bool DeleteCleaningPlan(CleaningPlan cleaningPlan)
        {
            return HandleCleaningPlanDeletion(cleaningPlan);
        }

        /// <inheritdoc/>
        /// <exception cref="System.ArgumentNullException">cleaningPlan</exception>
        public Guid CreateNewCleaningPlan(CleaningPlan cleaningPlan)
        {
            if (cleaningPlan == null)
            {
                throw new ArgumentNullException(nameof(cleaningPlan));
            }

            cleaningPlan.CreatedAt = DateTime.UtcNow;

            this._dbContext.CleaningPlans.Add(cleaningPlan);

            this._dbContext.SaveChanges();

            return cleaningPlan.Id;
        }

        /// <inheritdoc/>
        /// <exception cref="System.ArgumentNullException">cleaningPlan</exception>
        public bool UpdateCleaningPlan(CleaningPlan cleaningPlan)
        {
            if (cleaningPlan == null)
            {
                throw new ArgumentNullException(nameof(cleaningPlan));
            }

            this._dbContext.Entry(cleaningPlan).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

            return this._dbContext.SaveChanges() > 0;
        }

        private bool HandleCleaningPlanDeletion(CleaningPlan cleaningPlan)
        {
            if (cleaningPlan != null)
            {
                this._dbContext.CleaningPlans.Remove(cleaningPlan);

                return this._dbContext.SaveChanges() > 0;
            }

            return false;
        }
    }
}
