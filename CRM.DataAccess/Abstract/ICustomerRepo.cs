using CRM.Core.DataAccess.Abstract.Infrastructure.Linq;
using CRM.Entity.Concrete;

namespace CRM.DataAccess.Abstract {
    public interface ICustomerRepo : IRepoBase<Customer> {
    }
}