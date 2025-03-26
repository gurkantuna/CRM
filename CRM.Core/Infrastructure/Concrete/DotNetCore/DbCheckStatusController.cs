using CRM.Core.Managers.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRM.Core.Infrastructure.Concrete.DotNetCore {

    public class DbCheckStatusController<TContext> : Controller where TContext : DbContext, new() {

        public DbCheckStatusController(IDbControlManager<TContext> dbControlManager) {
            _dbControlManager = dbControlManager;
            if(!_checkedDbStatus) {
                _dbControlManager.CheckDbState(dbCreate: true);
                _checkedDbStatus = true;
            }
        }

        private readonly IDbControlManager<TContext> _dbControlManager;
        private static volatile bool _checkedDbStatus;
    }
}