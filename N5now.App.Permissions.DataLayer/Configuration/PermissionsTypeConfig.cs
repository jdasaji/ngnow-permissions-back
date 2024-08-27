
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using N5now.App.Permissions.Domain;
using System;
using System.Linq.Expressions;

#nullable enable
namespace N5now.App.Permissions.DataLayer.Configuration
{
  public class PermissionsTypeConfig : IEntityTypeConfiguration<PermissionsTypeDomain>
  {
    public void Configure(EntityTypeBuilder<PermissionsTypeDomain> builder)
    {
      builder.ToTable<PermissionsTypeDomain>("PermissionsType");
      builder.HasKey((Expression<Func<PermissionsTypeDomain, object>>) (x => (object) x.Id));
      builder.Property<int>((Expression<Func<PermissionsTypeDomain, int>>) (e => e.Id)).ValueGeneratedOnAdd();
      builder.Property<string>((Expression<Func<PermissionsTypeDomain, string>>) (e => e.Descripcion)).IsRequired(true);
    }
  }
}
