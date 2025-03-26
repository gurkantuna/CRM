using CRM.Business.Abstract;
using CRM.Business.Infrastructer.EntitiyFrameWorkCore;
using CRM.Core.Business.Abstract.Linq;
using CRM.Core.Entity.Concrete;
using CRM.DataAccess.Conctrete.EntityFrameworkCore.Context;

namespace CRM.Business.Concrete {
    public class LogBusiness : BusinessBase<CRMUnitOfWork, LogBase, CrmDbContext>, ILogBusiness {
        public LogBusiness(CrmDbContext context) : base(context) { }
    }
}