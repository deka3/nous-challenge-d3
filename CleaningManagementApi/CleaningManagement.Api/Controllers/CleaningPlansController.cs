using CleaningManagement.Service.Dto;
using CleaningManagement.Service.Infrastructure.Result;
using CleaningManagement.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace CleaningManagement.Api.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Authorize]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CleaningPlansController : ControllerBase
    {
        private readonly ICleaningManagementService _cleaningManagementService;
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CleaningPlansController"/> class.
        /// </summary>
        /// <param name="cleaningManagementService">The cleaning management service.</param>
        /// <param name="logger">The logger.</param>
        public CleaningPlansController(ICleaningManagementService cleaningManagementService, ILogger<CleaningPlansController> logger)
        {
            this._cleaningManagementService = cleaningManagementService;
            this.logger = logger;
        }

        /// <summary>
        /// Gets the cleaning plan by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Cleaning plan.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(CleaningPlan), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public IActionResult GetCleaningPlanById(Guid id)
        {
            var res = _cleaningManagementService.GetCleaningPlanById(id);

            if (res?.EventType == ResultEventType.NotFound)
            {
                return NotFound();
            }

            return Ok(res.Value);
        }

        /// <summary>
        /// Gets the cleaning plans by customery identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Collection cleaning plans for given customer.
        /// If customer doean't exist empty collection is returned.</returns>
        [HttpGet]
        [Route("customer/{id}")]
        [ProducesResponseType(typeof(IEnumerable<CleaningPlan>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCleaningPlansByCustomeryId(int id, int skip = 0, int take = 0)
        {
            var res = await this._cleaningManagementService.GetCleaningPlansByCustomerIdAsync(id, skip, take);

            return Ok(res.Value);
        }

        /// <summary>
        /// Creates the cleaning plans by customery identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Created result with location of new plan and new plan.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(CleaningPlan), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public IActionResult CreateCleaningPlan([FromBody]CleaningPlan cleaningPlan)
        {
            if (cleaningPlan == null)
            {
                return BadRequest($"Parameter {nameof(cleaningPlan)} cannot be null.");
            }

            try
            {
                var result = this._cleaningManagementService.CreateCleaningPlan(cleaningPlan);

                if (result == null)
                {
                    return BadRequest();
                }

                if (!result.IsSuccess)
                {
                    return BadRequest(result.Message);
                }

                string route = this.Url.Link(null, null);

                return Created($"{route}/{result.Value.Id}", result.Value);
            }
            catch (ArgumentException argex)
            {
                return BadRequest(argex.Message);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error while attempting to create new cleaning plan.");

                return this.StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Deletes the cleaning plan.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>No content if successful, otherwhise bad request.</returns>
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public IActionResult DeleteCleaningPlan(Guid id)
        {
            try
            {
                var res = this._cleaningManagementService.DeleteCleaningPlan(id);

                return res.IsSuccess ? Ok(res.Value) : (res.EventType == ResultEventType.NotFound ? NotFound() : BadRequest(res.Message));
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error while attempting to delete plan.");

                return this.StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Updates the cleaning plan.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="cleaningPlan">The cleaning plan.</param>
        /// <returns>Ok is successfull with updated plan, Bad request if not found.</returns>
        [HttpPut]
        [ProducesResponseType(typeof(CleaningPlan), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public IActionResult UpdateCleaningPlan(Guid id, [FromBody]CleaningPlan cleaningPlan)
        {
            try
            {
                var res = this._cleaningManagementService.UpdateCleaningPlan(id, cleaningPlan);

                if (res?.EventType == ResultEventType.NotFound)
                {
                    return NotFound();
                }

                return res.IsSuccess ? Ok(res.Value) : BadRequest(res.Message);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error while attempting to delete plan.");

                return this.StatusCode((int)HttpStatusCode.InternalServerError, ex.Message); ;
            }
        }
    }
}
