
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using N5now.App.Infrastructure.Kafka.Interfaces;
using N5now.App.Permissions.Common.Response;
using N5now.App.Permissions.DataLayer;
using N5now.App.Permissions.Domain;
using N5now.App.Permissions.Features.commandhandler;
using N5now.App.Permissions.Repository.Base;
using N5now.App.Permissions.Repository.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace N5now.App.Permissions.Features.Permissions.Queries.GetPermissionsAll
{
  public class GetPermissionsAllHandler : 
    CommandHandlerBase<PermissionsDomain, int>,
    IRequestHandler<GetPermissionsAllQuery, ResultResponse<object>>
  {
    private IEventProducer _eventPrpducer;
    private IUnitOfWork<PermissionsContext> _IUnitOfWork;

    private IMapper _mapper { get; set; }

    public GetPermissionsAllHandler(
      IUnitOfWork<PermissionsContext> unitOfWork,
      IMapper mapper,
      IPermissionsRepository permissionsRepository,
      IEventProducer eventPrpducer)
      : base((IRepository<PermissionsDomain, int>) permissionsRepository)
    {
      this._IUnitOfWork = unitOfWork;
      this._mapper = mapper;
      this._eventPrpducer = eventPrpducer;
    }

    public async Task<ResultResponse<object>> Handle(
      GetPermissionsAllQuery request,
      CancellationToken cancellationToken)
    {
      GetPermissionsAllHandler permissionsAllHandler = this;
      List<PermissionsDomain> list = permissionsAllHandler.All(true).Where<PermissionsDomain>((Expression<Func<PermissionsDomain, bool>>) (x => request.Id == 0 || x.Id == request.Id)).Include<PermissionsDomain, PermissionsTypeDomain>((Expression<Func<PermissionsDomain, PermissionsTypeDomain>>) (x => x.PermissionsType)).OrderByDescending<PermissionsDomain, int>((Expression<Func<PermissionsDomain, int>>) (X => X.Id)).ToList<PermissionsDomain>();
      List<GetPermissionsAllDTO> data1 = permissionsAllHandler._mapper.Map<List<GetPermissionsAllDTO>>((object) list);
      var data2 = new
      {
        Id = Guid.NewGuid(),
        NameOperation = "Get Permissions"
      };
      permissionsAllHandler._eventPrpducer.Produce("permissions_topic", JsonConvert.SerializeObject((object) data2));
      return new ResultResponse<object>((object) data1);
    }
  }
}
