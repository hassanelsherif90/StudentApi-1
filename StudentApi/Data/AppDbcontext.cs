using Microsoft.EntityFrameworkCore;
using StudentApi.Data.Entities;


namespace StudentApi.Data;

public partial class AppDbcontext : DbContext
{

    public AppDbcontext(DbContextOptions<AppDbcontext> options)
        : base(options)
    {
    }

    public DbSet<Student> Students { get; set; }

    public DbSet<User> Users { get; set; }

    public DbSet<UserPermission> UserPermissions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Arabic_CI_AS");

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Students__3214EC078BB10685");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<UserPermission>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.PermissionId });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
