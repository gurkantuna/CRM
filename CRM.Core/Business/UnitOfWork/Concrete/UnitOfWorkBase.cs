using CRM.Core.Business.UnitOfWork.Abstract;
using CRM.Core.DataAccess.Abstract.EntityFrameworkCore.Context;
using CRM.Core.Entity.Abstract;
using Microsoft.EntityFrameworkCore;

namespace CRM.Core.Business.UnitOfWork.Concrete {
    public abstract class UnitOfWorkBase<TContext> : IUnitOfWork<TContext> where TContext : DbContextWithDotNetIdentity, new() {

        protected UnitOfWorkBase() {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; }

        public TContext Context { get; } = new();

        public bool HasChanges() {
            return Context.ChangeTracker.HasChanges();
        }

        public int Save() {
            return Context.SaveChanges();
        }

        public async Task<int> SaveAsync() {
            return await Context.SaveChangesAsync();
        }

        public void ChangeState<TEntity>(TEntity entity, EntityState entityState) where TEntity : EntityBase {
            Context.Set<TEntity>().Entry(entity).State = entityState;
        }

        public async Task<IEnumerable<string>> GetPendingMigrationsAsync() {
            return await Context.Database.GetPendingMigrationsAsync();
        }

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing) {
            if(!_disposed && disposing) {
                Context.Dispose();
            }
            _disposed = true;
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}