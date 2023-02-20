using Microsoft.EntityFrameworkCore;
using SiecaAPI.Commons;
using SiecaAPI.Data.SQLImpl.Entities;

namespace SiecaAPI.Data.SQLImpl
{
    public class SqlContext: DbContext
    {
        public SqlContext(){ }        

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string conn = AppParamsTools.GetEnvironmentVariable("ConnectionStrings:sqlDB");
            optionsBuilder.UseSqlServer(conn);
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<OrganizationEntity> Organizations { get; set; }
        public DbSet<AccessUserEntity> AccessUsers { get; set; }
        public DbSet<AccessUserPasswordEntity> AccessUsersPassword { get; set; }
        public DbSet<DocumentTypeEntity> DocumentTypes { get; set;}
        public DbSet<RolEntity> Roles { get; set; }
        public DbSet<PermissionEntity> Permissions { get; set; }
        public DbSet<RolPermissionEntity> RolePermissions { get; set; }
        public DbSet<AccessUserRolEntity> AccessUserRoles { get; set; }
        public DbSet<MenuEntity> Menu { get; set; }
        public DbSet<TrainingCenterEntity> TrainingCenters { get; set; }
        public DbSet<CampusEntity> Campuses { get; set; }
        public DbSet<DevelopmentRoomEntity> DevelopmentRooms { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RolPermissionEntity>()
                .HasKey(rp => new { rp.OrganizationId, rp.RolId, rp.PermissionId });
        }
    }
}
