
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using N5now.App.Infrastructure.Elasticsearch.Interfaces;
using N5now.App.Infrastructure.Kafka.Interfaces;
using N5now.App.Permissions.Common;
using N5now.App.Permissions.Common.Exceptions;
using N5now.App.Permissions.Common.Response;
using N5now.App.Permissions.DataLayer;
using N5now.App.Permissions.Domain;
using N5now.App.Permissions.Features.commandhandler;
using N5now.App.Permissions.Features.Permissions.Commands.PermissionsUpdate;
using N5now.App.Permissions.Repository.Base;
using N5now.App.Permissions.Repository.Interfaces;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace N5now.App.Permissions.Features.Permissions.Commands.PermissionsRequest
{
  public class PermissionsUpdateCommandHandler : 
    CommandHandlerBase<PermissionsDomain, int>,
    IRequestHandler<PermissionsUpdateCommand, ResultResponse<object>>
  {
    private IEventProducer _eventPrpducer;
    private IElasticsearchService _elasticsearchService;
    private IUnitOfWork<PermissionsContext> _IUnitOfWork;

    private IMapper _mapper { get; set; }

    public PermissionsUpdateCommandHandler(
      IUnitOfWork<PermissionsContext> unitOfWork,
      IMapper mapper,
      IPermissionsRepository permissionsRepository,
      IElasticsearchService elasticsearchService,
      IEventProducer eventPrpducer)
      : base((IRepository<PermissionsDomain, int>) permissionsRepository)
    {
      this._IUnitOfWork = unitOfWork;
      this._mapper = mapper;
      this._eventPrpducer = eventPrpducer;
      this._elasticsearchService = elasticsearchService;
    }

    public async Task<ResultResponse<object>> Handle(
      PermissionsUpdateCommand request,
      CancellationToken cancellationToken)
    {
      PermissionsUpdateCommandHandler updateCommandHandler = this;
      updateCommandHandler._IUnitOfWork.BeginTransaction();
      PermissionsDomain data = updateCommandHandler._mapper.Map<PermissionsDomain>((object) request);
      ValidationResult validationResult = await updateCommandHandler.UpdateAsync(data, (IValidator<PermissionsDomain>) new PermissionsRequestCommandValidator(EnumTypeOperationsCrudCommon.UPDATE));
      if (!validationResult.IsValid)
        throw new ExceptionController(validationResult.ErrorMessageToList());
      await updateCommandHandler._IUnitOfWork.SaveChangesAsync();
      var message = new
      {
        Id = Guid.NewGuid(),
        NameOperation = nameof (request)
      };
      await updateCommandHandler._elasticsearchService.UpdateDocumentAsync(data.Id, data);
      updateCommandHandler._eventPrpducer.Produce("permissions_topic", JsonConvert.SerializeObject((object) message));
      message = null;
      ResultResponse<object> resultResponse = new ResultResponse<object>((object) data, "Se ha actualizado correctamente");
      data = (PermissionsDomain) null;
      return resultResponse;
    }
  }
}
