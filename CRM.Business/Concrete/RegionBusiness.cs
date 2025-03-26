using CRM.Business.Abstract;
using CRM.Business.Infrastructer.EntitiyFrameWorkCore;
using CRM.Core.Business.Abstract.Linq;
using CRM.Core.Entity.Concrete;
using CRM.DataAccess.Conctrete.EntityFrameworkCore.Context;

namespace CRM.Business.Concrete {
    public class RegionBusiness : BusinessBase<CRMUnitOfWork, RegionBase, CrmDbContext>, IRegionBusiness {
        public RegionBusiness(CrmDbContext context) : base(context) { }
    }
}