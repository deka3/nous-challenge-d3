using System;

namespace CleaningManagement.Service.Infrastructure.Result
{
    /// <summary>
    /// Helper class for more detailed results of service methods.
    /// </summary>
    public class Result<T>
    {
        /// <summary>
        /// Gets the exception.
        /// </summary>
        /// <value>
        /// The exception.
        /// </value>
        public Exception Exception { get; private set; }

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance has errors.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has errors; otherwise, <c>false</c>.
        /// </value>
        public bool HasErrors => this.Exception != null;

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public T Value { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is success.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is success; otherwise, <c>false</c>.
        /// </value>
        public bool IsSuccess { get; private set; }

        /// <summary>
        /// Gets the type of the event.
        /// </summary>
        /// <value>
        /// The type of the event.
        /// </value>
        public ResultEventType EventType { get; private set; } = ResultEventType.NotSet;

        /// <summary>
        /// Returns successful version of result.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="resultEventType">Type of the result event.</param>
        /// <returns>Successful version of result.</returns>
        public static Result<T> Succes(T value, ResultEventType resultEventType = ResultEventType.Ok)
        {
            if (resultEventType >= ResultEventType.BadRequest)
            {
                throw new ArgumentException($"{nameof(resultEventType)} suggests unsuccessful outcome.");
            }

            return new Result<T>(value, resultEventType);
        }

        /// <summary>
        /// Returns failed version of result.
        /// </summary>
        /// <param name="resultEventType">Type of the result event.</param>
        /// <param name="message">The message.</param>
        /// <param name="ex">The exception.</param>
        /// <returns>Failed version of result.</returns>
        /// <exception cref="System.ArgumentNullException">message</exception>
        public static Result<T> Fail(ResultEventType resultEventType, string message = null, Exception ex = null)
        {
            return new Result<T>(default(T), resultEventType, message, ex);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Result{T}"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="resultEventType">Type of the result event.</param>
        private Result(T value, ResultEventType resultEventType = ResultEventType.NotSet, string message = null, Exception ex = null)
        {
            Value = value;
            EventType = ex == null ? resultEventType : ResultEventType.InternalServerError;
            IsSuccess = resultEventType < ResultEventType.BadRequest && ex == null;
            Message = message ?? ex?.Message ?? resultEventType.ToString();
            Exception = ex;
        }
    }
}
