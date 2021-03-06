using System.Linq;
using DNTFrameworkCore.EntityFramework.Auditing;
using DNTFrameworkCore.EntityFramework.Caching;
using DNTFrameworkCore.EntityFramework.Context;
using DNTFrameworkCore.EntityFramework.DataProtection;
using DNTFrameworkCore.EntityFramework.Logging;
using DNTFrameworkCore.EntityFramework.SqlServer.Numbering;
using DNTFrameworkCore.Runtime;
using DNTFrameworkCore.TestAPI.Infrastructure.Mappings.Blogging;
using DNTFrameworkCore.TestAPI.Infrastructure.Mappings.Identity;
using EFSecondLevelCache.Core.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using RFrameworkCore.TestWebApp.Data.Mappings.Tasks;

namespace DNTFrameworkCore.TestAPI.Infrastructure.Context
{
    public class ProjectDbContext : DbContextCore
    {
        public ProjectDbContext(
            IHookEngine hookEngine,
            IUserSession session,
            DbContextOptions<ProjectDbContext> options) : base(hookEngine, session, options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BlogConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new UserTokenConfiguration());
            modelBuilder.ApplyConfiguration(new UserPermissionConfiguration());
            modelBuilder.ApplyConfiguration(new UserClaimConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new RoleClaimConfiguration());
            modelBuilder.ApplyConfiguration(new RolePermissionConfiguration());
            modelBuilder.ApplyConfiguration(new PermissionConfiguration());
            modelBuilder.ApplyConfiguration(new TaskConfiguration());
            modelBuilder.ApplyLogConfiguration();
            modelBuilder.ApplyNumberedEntityConfiguration();
            modelBuilder.ApplyDataProtectionKeyConfiguration();
            modelBuilder.ApplySqlCacheConfiguration();
            modelBuilder.ApplyAuditLogConfiguration();

            base.OnModelCreating(modelBuilder);
        }

        protected override void AfterSaveChanges(SaveChangeContext context)
        {
            this.GetService<IEFCacheServiceProvider>()
                .InvalidateCacheDependencies(context.ChangedEntityNames.ToArray());
        }
    }
}