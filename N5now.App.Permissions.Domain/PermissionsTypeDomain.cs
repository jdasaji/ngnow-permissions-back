
using System.Collections.Generic;

#nullable enable
namespace N5now.App.Permissions.Domain
{
  public class PermissionsTypeDomain
  {
    public int Id { get; set; }

    public string Descripcion { get; set; }

    public List<PermissionsDomain> Permissions { get; set; }
  }
}
