
using N5now.App.Permissions.Domain;
using N5now.App.Permissions.Repository.Base;

#nullable enable
namespace N5now.App.Permissions.Repository.Interfaces
{
  public interface IPermissionsRepository : IRepository<PermissionsDomain, int>
  {
  }
}
