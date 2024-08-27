
using Microsoft.EntityFrameworkCore;
using N5now.App.Permissions.DataLayer.Configuration;
using N5now.App.Permissions.Domain;

#nullable enable
namespace N5now.App.Permissions.DataLayer
{
  public class PermissionsContext : DbContext
  {
    public PermissionsContext(DbContextOptions<PermissionsContext> options)
      : base((DbContextOptions) options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<PermissionsTypeDomain>().ToTable<PermissionsTypeDomain>("PermissionsTypes");
      modelBuilder.Entity<PermissionsTypeDomain>().HasData(new PermissionsTypeDomain[2]
      {
        new PermissionsTypeDomain()
        {
          Id = 1,
          Descripcion = "Administrador"
        },
        new PermissionsTypeDomain()
        {
          Id = 2,
          Descripcion = "Invitado"
        }
      });
      base.OnModelCreating(modelBuilder);
      modelBuilder.ApplyConfiguration<PermissionsTypeDomain>((IEntityTypeConfiguration<PermissionsTypeDomain>) new PermissionsTypeConfig());
      modelBuilder.ApplyConfiguration<PermissionsDomain>((IEntityTypeConfiguration<PermissionsDomain>) new PermissionsConfig());
    }

    public DbSet<PermissionsDomain> Permissions { get; set; }

    public DbSet<PermissionsTypeDomain> PermissionsTypeDomains { get; set; }
  }
}
