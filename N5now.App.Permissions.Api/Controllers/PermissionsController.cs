
using MediatR;
using Microsoft.AspNetCore.Mvc;
using N5now.App.Permissions.Common.Response;
using N5now.App.Permissions.Features.Permissions.Commands.PermissionsRequest;
using N5now.App.Permissions.Features.Permissions.Commands.PermissionsUpdate;
using N5now.App.Permissions.Features.Permissions.Queries.GetPermissionsAll;
using System.Threading.Tasks;

namespace N5now.App.Permissions.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PermissionsController(IMediator mediator) => this._mediator = mediator;

        [HttpPost]
        [Route("add")]
        public async Task<object> add(PermissionsRequestCommand model)
        {
            return (object)await this._mediator.Send<ResultResponse<object>>((IRequest<ResultResponse<object>>)model);
        }

        [HttpPut]
        [Route("update")]
        public async Task<object> update(PermissionsUpdateCommand model)
        {
            return (object)await this._mediator.Send<ResultResponse<object>>((IRequest<ResultResponse<object>>)model);
        }

        [HttpPost]
        [Route("get-all")]
        public async Task<object> all(GetPermissionsAllQuery? model)
        {
            return (object)await this._mediator.Send<ResultResponse<object>>((IRequest<ResultResponse<object>>)(model ?? new GetPermissionsAllQuery()));
        }
    }
}
