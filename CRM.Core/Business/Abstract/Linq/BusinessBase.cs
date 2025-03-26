using CRM.Core.Business.UnitOfWork.Concrete;
using CRM.Core.DataAccess.Abstract.EntityFrameworkCore.Context;
using CRM.Core.DataAccess.Concrete.EntityFrameworkCore.Infrastructure.Linq;
using CRM.Core.Entity.Abstract;

namespace CRM.Core.Business.Abstract.Linq {
    public class BusinessBase<TUnitOfWorkContrete, TEntity, TContext> : RepoBase<TEntity, TContext>
                              where TUnitOfWorkContrete : UnitOfWorkBase<TContext>, new()
                              where TContext : DbContextWithDotNetIdentity, new()
                              where TEntity : EntityBase, new() {
        public BusinessBase(TContext context) : base(context) {
            UOW = new();
        }
        protected TUnitOfWorkContrete UOW { get; }
    }
}