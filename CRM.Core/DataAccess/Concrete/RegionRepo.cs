using CRM.Core.DataAccess.Concrete.EntityFrameworkCore.Infrastructure.Linq;
using CRM.Core.Entity.Concrete;
using Microsoft.EntityFrameworkCore;

namespace CRM.Core.DataAccess.Concrete {
    public class RegionRepo<TContext> : RepoBase<RegionBase, TContext> where TContext : DbContext, new() {
        public RegionRepo(TContext context) : base(context) {
        }
    }
}