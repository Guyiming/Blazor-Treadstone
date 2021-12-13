using Microsoft.EntityFrameworkCore;
using OpsMain.StorageLayer.Entity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OpsMain.StorageLayer
{
    public class TreadstoneMainContext : DbContext
    {
        public TreadstoneMainContext(DbContextOptions<TreadstoneMainContext> options) : base(options)
        {
            this.Database.EnsureCreated();
        }
        public DbSet<SysUser> Users { get; set; }
        public DbSet<SysRole> Roles { get; set; }
        public DbSet<SysMenu> Menus { get; set; }
        public DbSet<R_RoleUser> R_RoleUsers { get; set; }
        public DbSet<R_RoleMenu> R_RoleMenus { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<SysRole>().ToTable("SysRole");
            builder.Entity<SysUser>().ToTable("SysUser");
            builder.Entity<SysMenu>().ToTable("SysMenu").HasOne(t=>t.ParentMenu).WithMany(r=>r.SubMenus).HasForeignKey(t=>t.ParentId);

            builder.Entity<R_RoleUser>().ToTable("R_RoleUser").HasKey(t => new { t.RoleId, t.UserId });
            builder.Entity<R_RoleUser>().HasOne(t => t.Role).WithMany(r => r.R_RoleUsers).HasForeignKey(t => t.RoleId);
            builder.Entity<R_RoleUser>().HasOne(t => t.User).WithMany(r => r.R_RoleUsers).HasForeignKey(t => t.UserId);

            builder.Entity<R_RoleMenu>().ToTable("R_RoleMenu").HasKey(t => new { t.RoleId, t.MenuId });
            builder.Entity<R_RoleMenu>().HasOne(t => t.Role).WithMany(r => r.R_RoleMenus).HasForeignKey(t => t.RoleId);
            builder.Entity<R_RoleMenu>().HasOne(t => t.Menu).WithMany(r => r.R_RoleMenus).HasForeignKey(t => t.MenuId);

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.LogTo(str =>
            {
                var preColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(str);
                Console.ForegroundColor = preColor;
            },Microsoft.Extensions.Logging.LogLevel.Information);
        }

        public override int SaveChanges()
        {
            UpdateTime();
            return base.SaveChanges();
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTime();
            return base.SaveChangesAsync(cancellationToken);
        }
        private void UpdateTime()
        {
            var modifiedData = ChangeTracker.Entries().Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));
            foreach (var entry in modifiedData)
            {
                var d = entry.Entity as BaseEntity;
                if (entry.State == EntityState.Added)
                {
                    d.CreateTime = DateTime.Now;
                }
                d.UpdateTime = DateTime.Now;
            }
        }
    }
}
