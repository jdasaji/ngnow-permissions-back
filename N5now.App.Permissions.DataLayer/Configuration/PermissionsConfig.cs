
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using N5now.App.Permissions.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

#nullable enable
namespace N5now.App.Permissions.DataLayer.Configuration
{
  public class PermissionsConfig : IEntityTypeConfiguration<PermissionsDomain>
  {
    public void Configure(EntityTypeBuilder<PermissionsDomain> builder)
    {
      builder.ToTable<PermissionsDomain>("Permissions");
      builder.HasKey((Expression<Func<PermissionsDomain, object>>) (x => (object) x.Id));
      builder.Property<int>((Expression<Func<PermissionsDomain, int>>) (e => e.Id)).ValueGeneratedOnAdd();
      builder.Property<string>((Expression<Func<PermissionsDomain, string>>) (e => e.NombreEmpleado)).IsRequired(true);
      builder.Property<string>((Expression<Func<PermissionsDomain, string>>) (e => e.ApellidoEmpleado)).IsRequired(true);
      builder.HasOne<PermissionsTypeDomain>((Expression<Func<PermissionsDomain, PermissionsTypeDomain>>) (x => x.PermissionsType)).WithMany((Expression<Func<PermissionsTypeDomain, IEnumerable<PermissionsDomain>>>) (x => x.Permissions)).HasForeignKey((Expression<Func<PermissionsDomain, object>>) (x => (object) x.TipoPermiso)).IsRequired(true);
      builder.Property<DateTime>((Expression<Func<PermissionsDomain, DateTime>>) (e => e.FechaPermiso)).IsRequired(true);
    }
  }
}
