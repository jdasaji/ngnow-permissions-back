
using FluentValidation;
using N5now.App.Permissions.Common;
using N5now.App.Permissions.Domain;
using System;
using System.Linq.Expressions;

#nullable enable
namespace N5now.App.Permissions.Features.Permissions.Commands.PermissionsRequest
{
  public class PermissionsRequestCommandValidator : AbstractValidator<PermissionsDomain>
  {
    public PermissionsRequestCommandValidator(EnumTypeOperationsCrudCommon type)
    {
      if (type != EnumTypeOperationsCrudCommon.ADD && type != EnumTypeOperationsCrudCommon.UPDATE)
        return;
      this.RuleFor<string>((Expression<Func<PermissionsDomain, string>>) (x => x.NombreEmpleado)).NotNull<PermissionsDomain, string>().NotEmpty<PermissionsDomain, string>().WithMessage<PermissionsDomain, string>("Nombre requerido.");
      this.RuleFor<string>((Expression<Func<PermissionsDomain, string>>) (x => x.ApellidoEmpleado)).NotNull<PermissionsDomain, string>().NotEmpty<PermissionsDomain, string>().WithMessage<PermissionsDomain, string>("Apellido requerido.");
      this.RuleFor<DateTime>((Expression<Func<PermissionsDomain, DateTime>>) (x => x.FechaPermiso)).NotNull<PermissionsDomain, DateTime>().NotEmpty<PermissionsDomain, DateTime>().WithMessage<PermissionsDomain, DateTime>("Fecha requerido.");
      if (type != EnumTypeOperationsCrudCommon.UPDATE)
        return;
      this.RuleFor<int>((Expression<Func<PermissionsDomain, int>>) (x => x.Id)).GreaterThanOrEqualTo<PermissionsDomain, int>(0).WithMessage<PermissionsDomain, int>("Debe ingresar un Id.");
    }
  }
}
