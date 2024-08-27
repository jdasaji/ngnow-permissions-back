using AutoMapper;
using FluentValidation;
using Moq;
using Newtonsoft.Json;
using N5now.App.Permissions.Common.Exceptions;
using N5now.App.Permissions.Common.Response;
using N5now.App.Permissions.DataLayer;
using N5now.App.Permissions.Domain;
using N5now.App.Permissions.Features.Permissions.Commands.PermissionsRequest;
using N5now.App.Permissions.Repository.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using System.ComponentModel.DataAnnotations;
using System.Timers;
using N5now.App.Permissions.Repository.Base;
using N5now.App.Infrastructure.Elasticsearch.Interfaces;
using N5now.App.Infrastructure.Kafka.Interfaces;
using N5now.App.Permissions.Features.Permissions.Commands.PermissionsUpdate;

namespace N5now.App.Permissions.Features.Test
{
    public class PermissionsUpdateCommandHandlerTest
    {

        private readonly Mock<IUnitOfWork<PermissionsContext>> _mockUnitOfWork;
        private readonly Mock<IElasticsearchService> _mockElasticsearchService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IEventProducer> _mockEventProducer;
        private readonly Mock<IPermissionsRepository> _mockPermissionsRepository;
        private readonly PermissionsUpdateCommandHandler _handler;

        public PermissionsUpdateCommandHandlerTest()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork<PermissionsContext>>();
            _mockElasticsearchService = new Mock<IElasticsearchService>();
            _mockMapper = new Mock<IMapper>();
            _mockEventProducer = new Mock<IEventProducer>();
            _mockPermissionsRepository = new Mock<IPermissionsRepository>();

            _handler = new PermissionsUpdateCommandHandler(
                _mockUnitOfWork.Object,
                _mockMapper.Object,
                _mockPermissionsRepository.Object,
                                _mockElasticsearchService.Object,
                _mockEventProducer.Object
            );
        }

        [Fact]
        public async Task Handle_Should_Return_ResultResponse_With_Error_Message()
        {
            var command = new PermissionsUpdateCommand
            {
            };

            var permissionsDomain = new PermissionsDomain { Id = 1,NombreEmpleado="ss",ApellidoEmpleado="d",TipoPermiso=1 };

            _mockMapper.Setup(m => m.Map<PermissionsDomain>(It.IsAny<object>())).Returns(permissionsDomain);
            _mockUnitOfWork.Setup(uow => uow.BeginTransaction());
            _mockUnitOfWork.Setup(uow => uow.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Se ha actualizado correctamente", result.Messages);
            _mockUnitOfWork.Verify(uow => uow.BeginTransaction(), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
            _mockElasticsearchService.Verify(es => es.UpdateDocumentAsync(permissionsDomain.Id, permissionsDomain), Times.Once);
            _mockEventProducer.Verify(ep => ep.Produce("permissions_topic", It.IsAny<string>()), Times.Once);
        }

    }
}
