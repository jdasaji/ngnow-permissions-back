// Decompiled with JetBrains decompiler
// Type: N5now.App.Permissions.DataLayer.Configuration.PermissionsTypeConfig
// Assembly: N5now.App.Permissions.DataLayer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 05375A67-1362-4A10-96BA-435B870E80C4
// Assembly location: C:\Users\jsdan\OneDrive\Fuentes_Trabajo\n5now-dofuscator\compilado\N5now.App.Permissions.DataLayer.dll

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
