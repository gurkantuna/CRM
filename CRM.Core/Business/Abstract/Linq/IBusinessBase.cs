using CRM.Core.DataAccess.Abstract.Infrastructure.Linq;
using CRM.Core.Entity.Abstract;

namespace CRM.Core.Business.Abstract.Linq {
    public interface IBusinessBase<TEntity> : IRepoBase<TEntity> where TEntity : EntityBase {
    }

    public interface IBusinessBase {
    }
}
