using CRM.Business.Abstract;
using CRM.Business.Concrete;
using CRM.Core.Business.UnitOfWork.Concrete;
using CRM.DataAccess.Conctrete.EntityFrameworkCore.Context;

namespace CRM.Business.Infrastructer.EntitiyFrameWorkCore {
    public sealed class CRMUnitOfWork : UnitOfWorkBase<CrmDbContext> {

        private ICustomerBusiness _customers;
        private IRegionBusiness _regions;
        private ILogBusiness _logs;

        public ICustomerBusiness Customers => _customers ??= new CustomerBusiness(Context);
        public IRegionBusiness Regions => _regions ??= new RegionBusiness(Context);
        public ILogBusiness Logs => _logs ??= new LogBusiness(Context);
    }
}