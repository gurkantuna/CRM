using CRM.Core.DataAccess.Abstract.EntityFrameworkCore.Context;
using CRM.Entity.Concrete;
using Microsoft.EntityFrameworkCore;

namespace CRM.DataAccess.Conctrete.EntityFrameworkCore.Context {
    public class CrmDbContext : DbContextWithDotNetIdentity {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<User> Users { get; set; }
        public override bool RemoveAspNetPrefixFromIdentityTable { get; set; } = true;
        public override bool RecreateDatabase { get; set; }
    }
}