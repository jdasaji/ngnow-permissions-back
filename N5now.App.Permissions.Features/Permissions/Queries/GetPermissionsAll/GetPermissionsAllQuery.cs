
using MediatR;
using N5now.App.Permissions.Common.Response;

#nullable enable
namespace N5now.App.Permissions.Features.Permissions.Queries.GetPermissionsAll
{
  public class GetPermissionsAllQuery : IRequest<ResultResponse<object>>, IBaseRequest
  {
    public int Id { get; set; }
  }
}
