
using MediatR;
using N5now.App.Permissions.Common.Response;

#nullable enable
namespace N5now.App.Permissions.Features.Permissions.Commands.PermissionsRequest
{
  public class PermissionsRequestCommand : IRequest<ResultResponse<object>>, IBaseRequest
  {
    public string NameEmployee { get; set; }

    public string LastEmployee { get; set; }

    public int PermissionsType { get; set; }

    public string DatePermissions { get; set; }
  }
}
