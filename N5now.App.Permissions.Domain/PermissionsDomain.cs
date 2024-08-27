
using System;

#nullable enable
namespace N5now.App.Permissions.Domain
{
  public class PermissionsDomain
  {
    public int Id { get; set; }

    public string NombreEmpleado { get; set; }

    public string ApellidoEmpleado { get; set; }

    public int TipoPermiso { get; set; }

    public DateTime FechaPermiso { get; set; }

    public PermissionsTypeDomain PermissionsType { get; set; }
  }
}
