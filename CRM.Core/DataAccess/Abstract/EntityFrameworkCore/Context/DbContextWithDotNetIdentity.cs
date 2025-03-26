using CRM.Core.Entity.Abstract;
using CRM.Core.Entity.Concrete;
using CRM.Core.Extensions;
using CRM.Core.Helpers;
using CRM.Core.Utilities.IoC.DotNetCore;
using EntityFrameworkCore.UseRowNumberForPaging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Strings = CRM.Core.Constants.Strings;

namespace CRM.Core.DataAccess.Abstract.EntityFrameworkCore.Context {
    /// <summary>
    /// .NET Identity için gerekli tabloları da içerir.
    /// </summary>
    public abstract class DbContextWithDotNetIdentity : IdentityDbContext<UserBase, IdentityRole<int>, int> {

        protected DbContextWithDotNetIdentity() {

            _configuration = new ConfigurationBuilder()
                                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                                    .Build();

            if(!Enum.TryParse(_configuration.GetSection("RdbmsType")?.Value, true, out _rdbmsType)) {
                throw new InvalidOperationException("RdbmsType tipi içeriğine göre appsettings 'RdbmsType' node belirtilmelidir!");
            }

            _env = CoreServiceTool.ServiceProvider.GetRequiredService<IHostEnvironment>();            
        }

        private readonly IHostEnvironment _env;
        private readonly IConfigurationRoot _configuration;
        private readonly RdbmsType _rdbmsType;

        public DbSet<LogBase> Logs { get; set; }
        public DbSet<RegionBase> Regions { get; set; }

        public abstract bool RemoveAspNetPrefixFromIdentityTable { get; set; }

        public abstract bool RecreateDatabase { get; set; }

        public virtual bool DontThrowExceptionIfSetTrueDbRecreate { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {

            var connectionString = string.Empty;

            var framework = HelperAssembly.GetExecutingTargetFrameworkAndVersion();

            var executeOnDevelopment = Convert.ToBoolean(_configuration.GetSection("ExecuteLocalConnectionStringInDevelopmentEnvoriment")?.Value);

            if(_env?.IsDevelopment() == true && executeOnDevelopment) {
                connectionString = _configuration.GetConnectionString(Strings.Development);
            }
            else {
                connectionString = _configuration.GetConnectionString(Strings.Production);
            }

            if(connectionString.IsNullOrEmptyOrWhiteSpace()) {
                throw new InvalidOperationException("ConnectionStrings: {Development: <connection string> formatında bağlantı bildirmelisiniz.");
            }

            switch(_rdbmsType) {
                case RdbmsType.MsSql:
                    optionsBuilder.UseSqlServer(connectionString,
                        optionsBuilder => {
                            if(!framework.Version.StartsWith('9')) {
                                optionsBuilder.UseRowNumberForPaging();
                                optionsBuilder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                            }
                        }
                    );
                    break;
                case RdbmsType.PostgreSql:
                    optionsBuilder.UseNpgsql(connectionString).UseLowerCaseNamingConvention();
                    AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
                    break;
                case RdbmsType.MySql:
                    break;
            }
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            if(RemoveAspNetPrefixFromIdentityTable) {
                modelBuilder.Entity<UserBase>(eb => {
                    eb.ToTable("users");
                    eb.Property(e => e.Id).HasColumnName("id");
                });

                modelBuilder.Entity<IdentityRole<int>>(eb => {
                    eb.ToTable("roles");
                    eb.Property(e => e.Id).HasColumnName("id");

                    eb.HasData(
                       new IdentityRole<int> { Id = 1, Name = "Admin" },
                       new IdentityRole<int> { Id = 2, Name = "Moderate" }
                   );
                });

                modelBuilder.Entity<IdentityUserClaim<int>>(eb => {
                    eb.ToTable("userclaims");
                    eb.Property(e => e.Id).HasColumnName("id");
                });

                modelBuilder.Entity<IdentityUserLogin<int>>(eb => {
                    eb.ToTable("logins");
                    eb.Property(e => e.LoginProvider).HasColumnName("loginProvider");
                    eb.Property(e => e.ProviderKey).HasColumnName("providerKey");
                });

                modelBuilder.Entity<IdentityRoleClaim<int>>(eb => {
                    eb.ToTable("roleclaims");
                    eb.Property(e => e.Id).HasColumnName("id");
                });

                modelBuilder.Entity<IdentityUserRole<int>>(eb => {
                    eb.ToTable("userroles");
                    eb.Property(e => e.UserId).HasColumnName("userId");
                    eb.Property(e => e.RoleId).HasColumnName("roleId");
                });

                modelBuilder.Entity<IdentityUserToken<int>>(eb => {
                    eb.ToTable("usertokens");
                    eb.Property(e => e.UserId).HasColumnName("userId");
                    eb.Property(e => e.LoginProvider).HasColumnName("loginProvider");
                    eb.Property(e => e.Name).HasColumnName("name");
                });

                modelBuilder.Entity<RegionBase>(eb => {
                    eb.ToTable("regions");
                    eb.Property(e => e.Id).HasColumnName("id");
                    eb.Property(e => e.Name).IsRequired();

                    eb.HasData(
                        new RegionBase { Id = 1, Name = "Asia" },
                        new RegionBase { Id = 2, Name = "Europe" },
                        new RegionBase { Id = 3, Name = "North America" },
                        new RegionBase { Id = 4, Name = "South America" },
                        new RegionBase { Id = 5, Name = "Africa" },
                        new RegionBase { Id = 6, Name = "Australia" },
                        new RegionBase { Id = 7, Name = "Antarctica" }
                    );
                });

                modelBuilder.Entity<LogBase>(eb => {
                    eb.ToTable("logs");
                    eb.Property(e => e.Id).HasColumnName("id");
                });
            }

            foreach(var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys())) {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}
