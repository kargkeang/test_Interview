using Interview_Test.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Interview_Test.Infrastructure;

public class InterviewTestDbContext : DbContext
{
    public InterviewTestDbContext(DbContextOptions<InterviewTestDbContext> options) : base(options)
    {
    }
    
    public DbSet<UserModel> UserTb { get; set; }
    public DbSet<UserProfileModel> UserProfileTb { get; set; }
    public DbSet<RoleModel> RoleTb { get; set; }
    public DbSet<UserRoleMappingModel> UserRoleMappingTb { get; set; }
    public DbSet<PermissionModel> PermissionTb { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 1. ความสัมพันธ์ User <-> UserRoleMapping (Many-to-Many bridge)
        modelBuilder.Entity<UserRoleMappingModel>(entity =>
        {
            // กำหนด Composite Key สำหรับ Table กลาง
            entity.HasKey(urm => new { urm.UserId, urm.RoleId });

            entity.HasOne(urm => urm.User)
                  .WithMany(u => u.UserRoleMappings)
                  .HasForeignKey(urm => urm.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(urm => urm.Role)
                  .WithMany(r => r.UserRoleMappings)
                  .HasForeignKey(urm => urm.RoleId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // 2. ความสัมพันธ์ Role -> Permission (One-to-Many)
        modelBuilder.Entity<PermissionModel>(entity =>
        {
            entity.HasOne(p => p.Role)
                  .WithMany(r => r.Permissions)
                  .HasForeignKey("RoleId") // อ้างอิง Shadow Property หรือ ForeignKey ใน Model
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}

public class InterviewTestDbContextDesignFactory : IDesignTimeDbContextFactory<InterviewTestDbContext>
{
    public InterviewTestDbContext CreateDbContext(string[] args)
    {
        // หมายเหตุ: ในการใช้งานจริงควรดึงจาก configuration
        string connectionString = "Server=(localdb)\\mssqllocaldb;Database=InterviewTestDb;Trusted_Connection=True;MultipleActiveResultSets=true";
        
        var optionsBuilder = new DbContextOptionsBuilder<InterviewTestDbContext>()
            .UseSqlServer(connectionString, opts => opts.CommandTimeout(600));

        return new InterviewTestDbContext(optionsBuilder.Options);
    }
}