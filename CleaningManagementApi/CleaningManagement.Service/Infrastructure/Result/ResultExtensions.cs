using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleaningManagement.Service.Infrastructure.Result
{
    /// <summary>
    /// Result extensions.
    /// </summary>
    public static class ResultExtensions
    {
        public static Result<T> ToResult<T>(this T value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return Result<T>.Succes(value);
        }
    }
}
