namespace CleaningManagement.Service.Infrastructure.Result
{
    /// <summary>
    /// Result event types.
    /// </summary>
    public enum ResultEventType
    {
        /// <summary>
        /// The not set.
        /// </summary>
        NotSet = 0,

        /// <summary>
        /// The ok.
        /// </summary>
        Ok = 200,

        /// <summary>
        /// The created
        /// </summary>
        Created = 201,

        /// <summary>
        /// The bad request.
        /// </summary>
        BadRequest = 400,

        /// <summary>
        /// The not found.
        /// </summary>
        NotFound = 404,

        /// <summary>
        /// The internal server error.
        /// </summary>
        InternalServerError = 500
    }
}
