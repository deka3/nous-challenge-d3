using CleaningManagement.DAL.Models;
using CleaningManagement.DAL.Repositories;
using CleaningManagement.Service.Infrastructure.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleaningManagement.Service.Services
{
    /// <summary>
    /// Cleaning management service.
    /// </summary>
    /// <seealso cref="CleaningManagement.Service.ICleaningManagementService" />
    public class CleaningManagementService : BaseService, ICleaningManagementService
    {
        private readonly ICleaningPlanRepository _cleaningPlanRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CleaningManagementService"/> class.
        /// </summary>
        /// <param name="cleaningPlanRepository">The cleaning plan repository.</param>
        /// <exception cref="System.ArgumentNullException">cleaningPlanRepository</exception>
        public CleaningManagementService(ICleaningPlanRepository cleaningPlanRepository) :
            base(cleaningPlanRepository)
        {
            if (cleaningPlanRepository == null)
            {
                throw new ArgumentNullException(nameof(cleaningPlanRepository));
            }

            this._cleaningPlanRepository = cleaningPlanRepository;
        }

        /// <inheritdoc/>
        public Result<Dto.CleaningPlan> GetCleaningPlanById(Guid id)
        {
            var dbPlan = this._cleaningPlanRepository.GetCleaningPlanById(id);
            if (dbPlan == null)
            {
                return Result<Dto.CleaningPlan>.Fail(ResultEventType.NotFound);
            }

            var plan = new Dto.CleaningPlan()
            {
                Id = dbPlan.Id,
                CustomerId = dbPlan.CustomerId,
                Title = dbPlan.Title,
                CreatedAt = dbPlan.CreatedAt,
                Description = dbPlan.Description,
            };

            return plan.ToResult();
        }

        /// <inheritdoc/>
        /// <exception cref="System.ArgumentNullException">cleaningPlanRepository</exception>
        public Result<Dto.CleaningPlan> CreateCleaningPlan(Dto.CleaningPlan cleaningPlan)
        {
            if (cleaningPlan == null)
            {
                throw new ArgumentNullException(nameof(cleaningPlan));
            }

            ValidateCleaningPlan(cleaningPlan);

            var dbPlan = this._mapper.Map<CleaningPlan>(cleaningPlan);

            var newPlanId = this._cleaningPlanRepository.CreateNewCleaningPlan(dbPlan);
            var newPlan = this._cleaningPlanRepository.GetCleaningPlanById(newPlanId);

            return Result<Dto.CleaningPlan>.Succes(this._mapper.Map<Dto.CleaningPlan>(newPlan), ResultEventType.Created);
        }

        /// <inheritdoc/>
        /// <exception cref="System.ArgumentNullException">cleaningPlanRepository</exception>
        public Result<Dto.CleaningPlan> UpdateCleaningPlan(Guid id, Dto.CleaningPlan cleaningPlan)
        {
            if (cleaningPlan == null)
            {
                throw new ArgumentNullException(nameof(cleaningPlan));
            }

            var dbPlan = this._cleaningPlanRepository.GetCleaningPlanById(id);
            if (dbPlan == null)
            {
                return Result<Dto.CleaningPlan>.Fail(ResultEventType.NotFound);
            }

            ValidateCleaningPlan(cleaningPlan);

            dbPlan.Description = cleaningPlan.Description;
            dbPlan.Title = cleaningPlan.Title;
            dbPlan.CustomerId = cleaningPlan.CustomerId;

            if (this._cleaningPlanRepository.UpdateCleaningPlan(dbPlan))
            {
                return cleaningPlan.ToResult();
            }

            return Result<Dto.CleaningPlan>.Fail(ResultEventType.BadRequest);
        }

        /// <inheritdoc/>
        public async Task<Result<IEnumerable<Dto.CleaningPlan>>> GetCleaningPlansByCustomerIdAsync(int customerId, int skip = 0, int take = 0)
        {
            try
            {
                var task = this._cleaningPlanRepository.GetCleaningPlansByCustomerIdAsync(customerId, skip, take)
                                                           .ContinueWith(t =>
                                                           {
                                                               if (t.IsFaulted || t.IsCanceled)
                                                               {
                                                                   return Result<IEnumerable<Dto.CleaningPlan>>.Fail(ResultEventType.BadRequest, t.IsCanceled ? "Canceled" : string.Empty, t.Exception);
                                                               }

                                                               return t.Result.Select(x => new Dto.CleaningPlan()
                                                               {
                                                                   Id = x.Id,
                                                                   CustomerId = x.CustomerId,
                                                                   Title = x.Title,
                                                                   CreatedAt = x.CreatedAt,
                                                                   Description = x.Description,
                                                               }).ToResult();
                                                           });

                return await task;
            }
            catch (AggregateException aex)
            {
                return Result<IEnumerable<Dto.CleaningPlan>>.Fail(ResultEventType.InternalServerError, null, aex);
            }
        }

        /// <inheritdoc/>
        public Result<bool> DeleteCleaningPlan(Guid id)
        {
            var dbPlan = this._cleaningPlanRepository.GetCleaningPlanById(id);
            if (dbPlan == null)
            {
                return Result<bool>.Fail(ResultEventType.NotFound);
            }

            return this._cleaningPlanRepository.DeleteCleaningPlan(id)
                                               .ToResult();
        }

        private static void ValidateCleaningPlan(Dto.CleaningPlan cleaningPlan)
        {
            if (string.IsNullOrWhiteSpace(cleaningPlan.Title))
            {
                throw new ArgumentException($"Parameter {nameof(CleaningPlan.Title)} cannot be null, empty or whitespace.");
            }

            if (cleaningPlan.CustomerId < 1)
            {
                throw new ArgumentException($"Parameter {nameof(CleaningPlan.CustomerId)} must be provided.");
            }

            if (cleaningPlan.Title.Length > CleaningPlan.TitleMaxLength)
            {
                throw new ArgumentException($"{nameof(CleaningPlan.Title)} is limited to {CleaningPlan.TitleMaxLength} characters");
            }

            if (cleaningPlan.Description?.Length > CleaningPlan.DescriptionMaxLength)
            {
                throw new ArgumentException($"{nameof(CleaningPlan.Description)} is limited to {CleaningPlan.DescriptionMaxLength} characters");
            }
        }
    }
}
