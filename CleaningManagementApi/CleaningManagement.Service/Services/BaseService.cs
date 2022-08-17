using AutoMapper;
using CleaningManagement.Service.Infrastructure;
using System;
using System.Collections.Generic;

namespace CleaningManagement.Service.Services
{
    /// <summary>
    /// Base service class.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    /// <remarks>In this scenario base service class only handles IDisposable pattern and instantiates mapper.
    /// In real world application it would include any common logic or members for inheriting classes.</remarks>
    public abstract class BaseService : IDisposable
    {
        protected readonly IMapper _mapper;
        protected ICollection<IDisposable> _disposables = new List<IDisposable>();
        protected bool _isDisposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseService"/> class.
        /// </summary>
        public BaseService(IDisposable managedResource)
        {
            if (managedResource != null)
            {
                this._disposables.Add(managedResource);
            }

            this._mapper = AutoMapperUtility.GetAutomapper();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseService"/> class.
        /// </summary>
        public BaseService()
        {
            this._mapper = AutoMapperUtility.GetAutomapper();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    if (_disposables != null && _disposables.Count > 0)
                    {
                        foreach (var item in _disposables)
                        {
                            item.Dispose();
                        }
                    }
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
