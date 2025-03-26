using CRM.Core.DataAccess.Abstract.EntityFrameworkCore.Context;
using CRM.Core.Entity.Abstract;
using Microsoft.EntityFrameworkCore;

namespace CRM.Core.Business.UnitOfWork.Abstract {
    public interface IUnitOfWork<TContext> : IDisposable where TContext : DbContextWithDotNetIdentity, new() {

        TContext Context { get; }

        bool HasChanges();

        int Save();

        Task<int> SaveAsync();

        void ChangeState<TEntity>(TEntity entity, EntityState entityState) where TEntity : EntityBase;

        Task<IEnumerable<string>> GetPendingMigrationsAsync();
    }
}