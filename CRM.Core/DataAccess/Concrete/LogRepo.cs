using CRM.Core.DataAccess.Concrete.EntityFrameworkCore.Infrastructure.Linq;
using CRM.Core.Entity.Concrete;
using Microsoft.EntityFrameworkCore;

namespace CRM.Core.DataAccess.Concrete {
    public class LogRepo<TContext> : RepoBase<LogBase, TContext> where TContext : DbContext, new() {
        public LogRepo(TContext context) : base(context) {
        }         
    }
}