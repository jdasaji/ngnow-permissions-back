
using N5now.App.Permissions.DataLayer;
using N5now.App.Permissions.Domain;
using N5now.App.Permissions.Repository.Base;

#nullable enable
namespace N5now.App.Permissions.Repository.Interfaces
{
  public class PermissionsRepository : 
    N5now.App.Permissions.Repository.Base.Repository<PermissionsContext, PermissionsDomain, int>,
    IPermissionsRepository,
    IRepository<PermissionsDomain, int>
  {
    public PermissionsRepository(PermissionsContext dbcontext)
      : base(dbcontext)
    {
    }
  }
}
