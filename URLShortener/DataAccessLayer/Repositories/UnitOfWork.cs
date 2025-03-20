using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Entities;
using DataAccessLayer.Data;
using Microsoft.EntityFrameworkCore;
using Data_Access_Layer.Repositories;

namespace DataAccessLayer.Repositories
{
    public class UnitOfWork : IDisposable
    {
        private readonly AppDbContext context;

        private GenericRepository<Url> _urlRepository;

        public UnitOfWork(AppDbContext context)
        {
            this.context = context;
        }

        public GenericRepository<Url> UrlRepository
        {
            get
            {
                if (_urlRepository == null)
                {
                    _urlRepository = new GenericRepository<Url>(context);
                }
                return _urlRepository;
            }
        }
        public void Save()
        {
            context.SaveChanges();
        }
        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
