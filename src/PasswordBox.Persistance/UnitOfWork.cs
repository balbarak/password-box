﻿using PasswordBox.Persistance.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordBox.Persistance
{
    public class UnitOfWork : IDisposable
    {
        public readonly PasswordBoxDbContext Context = new PasswordBoxDbContext();

        private GenericRepository _genericRepository;

        public GenericRepository GenericRepository
        {
            get
            {
                if (_genericRepository == null)
                    _genericRepository = new GenericRepository(Context);

                return _genericRepository;
            }

        }

        public int Commit()
        {
            return Context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    Context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        void IDisposable.Dispose()
        {
            this.Dispose();
        }
    }
}
