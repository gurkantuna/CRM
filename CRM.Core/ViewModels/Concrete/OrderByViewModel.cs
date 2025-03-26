using System.Linq.Expressions;
using CRM.Core.Entity.Abstract;

namespace CRM.Core.ViewModels.Concrete {
    public class OrderByViewModel<TEntity> where TEntity : EntityBase {

        public OrderDirection Direction { get; set; }

        public Expression<Func<TEntity, object>> KeySelector { get; set; }
    }

    public enum OrderDirection {
        Asc,
        Desc
    }
}