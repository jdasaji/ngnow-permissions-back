
using AutoMapper;
using N5now.App.Permissions.Domain;
using N5now.App.Permissions.Features.Permissions.Commands.PermissionsRequest;
using N5now.App.Permissions.Features.Permissions.Queries.GetPermissionsAll;
using System;
using System.Globalization;
using System.Linq.Expressions;

#nullable enable
namespace N5now.App.Permissions.AutoMappers.Profiles
{
  public class PermissionsProfile : Profile
  {
    public PermissionsProfile()
    {
      this.PermissionsAddCommand();
      this.PermissionsUpdateCommand();
      this.GetPermissionsAll();
    }

    private void PermissionsAddCommand()
    {
      this.CreateMap<PermissionsRequestCommand, PermissionsDomain>().ForMember<string>((Expression<Func<PermissionsDomain, string>>) (p => p.NombreEmpleado), (Action<IMemberConfigurationExpression<PermissionsRequestCommand, PermissionsDomain, string>>) (x => x.MapFrom<string>((Expression<Func<PermissionsRequestCommand, string>>) (d => d.NameEmployee)))).ForMember<string>((Expression<Func<PermissionsDomain, string>>) (p => p.ApellidoEmpleado), (Action<IMemberConfigurationExpression<PermissionsRequestCommand, PermissionsDomain, string>>) (x => x.MapFrom<string>((Expression<Func<PermissionsRequestCommand, string>>) (d => d.LastEmployee)))).ForMember<int>((Expression<Func<PermissionsDomain, int>>) (p => p.TipoPermiso), (Action<IMemberConfigurationExpression<PermissionsRequestCommand, PermissionsDomain, int>>) (x => x.MapFrom<int>((Expression<Func<PermissionsRequestCommand, int>>) (d => d.PermissionsType)))).ForMember<DateTime>((Expression<Func<PermissionsDomain, DateTime>>) (p => p.FechaPermiso), (Action<IMemberConfigurationExpression<PermissionsRequestCommand, PermissionsDomain, DateTime>>) (x => x.MapFrom<DateTime>((Expression<Func<PermissionsRequestCommand, DateTime>>) (d => DateTime.ParseExact(d.DatePermissions, "yyyy-MM-dd", CultureInfo.InvariantCulture))))).ForMember<PermissionsTypeDomain>((Expression<Func<PermissionsDomain, PermissionsTypeDomain>>) (p => p.PermissionsType), (Action<IMemberConfigurationExpression<PermissionsRequestCommand, PermissionsDomain, PermissionsTypeDomain>>) (x => x.Ignore()));
    }

    private void PermissionsUpdateCommand()
    {
      this.CreateMap<N5now.App.Permissions.Features.Permissions.Commands.PermissionsUpdate.PermissionsUpdateCommand, PermissionsDomain>().ForMember<int>((Expression<Func<PermissionsDomain, int>>) (p => p.Id), (Action<IMemberConfigurationExpression<N5now.App.Permissions.Features.Permissions.Commands.PermissionsUpdate.PermissionsUpdateCommand, PermissionsDomain, int>>) (x => x.MapFrom<int>((Expression<Func<N5now.App.Permissions.Features.Permissions.Commands.PermissionsUpdate.PermissionsUpdateCommand, int>>) (d => d.Id)))).ForMember<string>((Expression<Func<PermissionsDomain, string>>) (p => p.NombreEmpleado), (Action<IMemberConfigurationExpression<N5now.App.Permissions.Features.Permissions.Commands.PermissionsUpdate.PermissionsUpdateCommand, PermissionsDomain, string>>) (x => x.MapFrom<string>((Expression<Func<N5now.App.Permissions.Features.Permissions.Commands.PermissionsUpdate.PermissionsUpdateCommand, string>>) (d => d.NameEmployee)))).ForMember<string>((Expression<Func<PermissionsDomain, string>>) (p => p.ApellidoEmpleado), (Action<IMemberConfigurationExpression<N5now.App.Permissions.Features.Permissions.Commands.PermissionsUpdate.PermissionsUpdateCommand, PermissionsDomain, string>>) (x => x.MapFrom<string>((Expression<Func<N5now.App.Permissions.Features.Permissions.Commands.PermissionsUpdate.PermissionsUpdateCommand, string>>) (d => d.LastEmployee)))).ForMember<int>((Expression<Func<PermissionsDomain, int>>) (p => p.TipoPermiso), (Action<IMemberConfigurationExpression<N5now.App.Permissions.Features.Permissions.Commands.PermissionsUpdate.PermissionsUpdateCommand, PermissionsDomain, int>>) (x => x.MapFrom<int>((Expression<Func<N5now.App.Permissions.Features.Permissions.Commands.PermissionsUpdate.PermissionsUpdateCommand, int>>) (d => d.PermissionsType)))).ForMember<DateTime>((Expression<Func<PermissionsDomain, DateTime>>) (p => p.FechaPermiso), (Action<IMemberConfigurationExpression<N5now.App.Permissions.Features.Permissions.Commands.PermissionsUpdate.PermissionsUpdateCommand, PermissionsDomain, DateTime>>) (x => x.MapFrom<DateTime>((Expression<Func<N5now.App.Permissions.Features.Permissions.Commands.PermissionsUpdate.PermissionsUpdateCommand, DateTime>>) (d => DateTime.ParseExact(d.DatePermissions, "yyyy-MM-dd", CultureInfo.InvariantCulture))))).ForMember<PermissionsTypeDomain>((Expression<Func<PermissionsDomain, PermissionsTypeDomain>>) (p => p.PermissionsType), (Action<IMemberConfigurationExpression<N5now.App.Permissions.Features.Permissions.Commands.PermissionsUpdate.PermissionsUpdateCommand, PermissionsDomain, PermissionsTypeDomain>>) (x => x.Ignore()));
    }

    private void GetPermissionsAll()
    {
      this.CreateMap<PermissionsDomain, GetPermissionsAllDTO>().ForMember<int>((Expression<Func<GetPermissionsAllDTO, int>>) (p => p.Id), (Action<IMemberConfigurationExpression<PermissionsDomain, GetPermissionsAllDTO, int>>) (x => x.MapFrom<int>((Expression<Func<PermissionsDomain, int>>) (d => d.Id)))).ForMember<string>((Expression<Func<GetPermissionsAllDTO, string>>) (p => p.NameEmployee), (Action<IMemberConfigurationExpression<PermissionsDomain, GetPermissionsAllDTO, string>>) (x => x.MapFrom<string>((Expression<Func<PermissionsDomain, string>>) (d => d.NombreEmpleado)))).ForMember<string>((Expression<Func<GetPermissionsAllDTO, string>>) (p => p.LastEmployee), (Action<IMemberConfigurationExpression<PermissionsDomain, GetPermissionsAllDTO, string>>) (x => x.MapFrom<string>((Expression<Func<PermissionsDomain, string>>) (d => d.ApellidoEmpleado)))).ForMember<string>((Expression<Func<GetPermissionsAllDTO, string>>) (p => p.PermissionsTypeName), (Action<IMemberConfigurationExpression<PermissionsDomain, GetPermissionsAllDTO, string>>) (x => x.MapFrom<string>((Expression<Func<PermissionsDomain, string>>) (d => d.PermissionsType.Descripcion)))).ForMember<string>((Expression<Func<GetPermissionsAllDTO, string>>) (p => p.DatePermissions), (Action<IMemberConfigurationExpression<PermissionsDomain, GetPermissionsAllDTO, string>>) (x => x.MapFrom<string>((Expression<Func<PermissionsDomain, string>>) (d => d.FechaPermiso.ToString("yyyy-MM-dd"))))).ForMember<string>((Expression<Func<GetPermissionsAllDTO, string>>) (p => p.PermissionsType), (Action<IMemberConfigurationExpression<PermissionsDomain, GetPermissionsAllDTO, string>>) (x => x.MapFrom<int>((Expression<Func<PermissionsDomain, int>>) (d => d.TipoPermiso))));
    }
  }
}
