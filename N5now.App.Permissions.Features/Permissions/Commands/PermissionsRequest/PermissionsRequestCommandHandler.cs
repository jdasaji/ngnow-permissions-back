
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
using N5now.App.Permissions.Repository.Base;
using N5now.App.Permissions.Repository.Interfaces;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace N5now.App.Permissions.Features.Permissions.Commands.PermissionsRequest
{
  public class PermissionsRequestCommandHandler : 
    CommandHandlerBase<PermissionsDomain, int>,
    IRequestHandler<PermissionsRequestCommand, ResultResponse<object>>
  {
    private IEventProducer _eventPrpducer;
    private IElasticsearchService _elasticsearchService;
    private IUnitOfWork<PermissionsContext> _IUnitOfWork;

    private IMapper _mapper { get; set; }

    public PermissionsRequestCommandHandler(
      IUnitOfWork<PermissionsContext> unitOfWork,
      IElasticsearchService elasticsearchService,
      IMapper mapper,
      IPermissionsRepository permissionsRepository,
      IEventProducer eventPrpducer)
      : base((IRepository<PermissionsDomain, int>) permissionsRepository)
    {
      this._IUnitOfWork = unitOfWork;
      this._mapper = mapper;
      this._eventPrpducer = eventPrpducer;
      this._elasticsearchService = elasticsearchService;
    }

    public async Task<ResultResponse<object>> Handle(
      PermissionsRequestCommand request,
      CancellationToken cancellationToken)
    {
      PermissionsRequestCommandHandler requestCommandHandler = this;
      requestCommandHandler._IUnitOfWork.BeginTransaction();
      PermissionsDomain data = requestCommandHandler._mapper.Map<PermissionsDomain>((object) request);
      ValidationResult validationResult = await requestCommandHandler.AddAsync(data, (IValidator<PermissionsDomain>) new PermissionsRequestCommandValidator(EnumTypeOperationsCrudCommon.ADD));
      if (!validationResult.IsValid)
        throw new ExceptionController(validationResult.ErrorMessageToList());
      await requestCommandHandler._IUnitOfWork.SaveChangesAsync();
      var message = new
      {
        Id = Guid.NewGuid(),
        NameOperation = nameof (request)
      };
      await requestCommandHandler._elasticsearchService.IndexDocumentAsync(data);
      requestCommandHandler._eventPrpducer.Produce("permissions_topic", JsonConvert.SerializeObject((object) message));
      message = null;
      ResultResponse<object> resultResponse = new ResultResponse<object>((object) data, "Se ha registado correctamente");
      data = (PermissionsDomain) null;
      return resultResponse;
    }
  }
}
