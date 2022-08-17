using System;

namespace CleaningManagement.DAL.Repositories
{
    /// <summary>
    /// Base repository.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public abstract class BaseRepository : IDisposable
    {
        protected readonly CleaningManagementDbContext _dbContext;
        protected bool _isDisposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public BaseRepository(CleaningManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
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
