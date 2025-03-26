using CRM.Core.DataAccess.Concrete.EntityFrameworkCore.Infrastructure.Linq;
using CRM.DataAccess.Abstract;
using CRM.DataAccess.Conctrete.EntityFrameworkCore.Context;
using CRM.Entity.Concrete;

namespace CRM.DataAccess.Conctrete {
    public class CustomerRepo : RepoBase<Customer, CrmDbContext>, ICustomerRepo {
        public CustomerRepo(CrmDbContext context) : base(context) {
        }       
    }
}