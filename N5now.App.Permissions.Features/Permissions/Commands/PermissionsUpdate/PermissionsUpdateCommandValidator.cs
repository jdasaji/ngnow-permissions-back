
using FluentValidation;
using N5now.App.Permissions.Domain;
using System;
using System.Linq.Expressions;

#nullable enable
namespace N5now.App.Permissions.Features.Permissions.Commands.PermissionsUpdate
{
  public class PermissionsUpdateCommandValidator : AbstractValidator<PermissionsDomain>
  {
    public PermissionsUpdateCommandValidator()
    {
      this.RuleFor<string>((Expression<Func<PermissionsDomain, string>>) (x => x.NombreEmpleado)).NotNull<PermissionsDomain, string>().NotEmpty<PermissionsDomain, string>().WithMessage<PermissionsDomain, string>("Nombre requerido.");
      this.RuleFor<string>((Expression<Func<PermissionsDomain, string>>) (x => x.ApellidoEmpleado)).NotNull<PermissionsDomain, string>().NotEmpty<PermissionsDomain, string>().WithMessage<PermissionsDomain, string>("Apellido requerido.");
      this.RuleFor<int>((Expression<Func<PermissionsDomain, int>>) (x => x.TipoPermiso)).GreaterThan<PermissionsDomain, int>(0).WithMessage<PermissionsDomain, int>("El id debe ser mayor a 0.");
      this.RuleFor<DateTime>((Expression<Func<PermissionsDomain, DateTime>>) (x => x.FechaPermiso)).NotNull<PermissionsDomain, DateTime>().NotEmpty<PermissionsDomain, DateTime>().WithMessage<PermissionsDomain, DateTime>("Fecha requerido.");
    }
  }
}
