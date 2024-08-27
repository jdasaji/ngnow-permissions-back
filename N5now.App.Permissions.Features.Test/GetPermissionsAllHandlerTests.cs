
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using N5now.App.Infrastructure.Kafka.Interfaces;
using N5now.App.Permissions.Common.Response;
using N5now.App.Permissions.DataLayer;
using N5now.App.Permissions.Domain;
using N5now.App.Permissions.Features.commandhandler;
using N5now.App.Permissions.Features.Permissions.Queries.GetPermissionsAll;
using N5now.App.Permissions.Repository.Base;
using N5now.App.Permissions.Repository.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;



namespace N5now.App.Permissions.Features.Test
{
    public class GetPermissionsAllHandlerTests
    {
        private readonly Mock<IUnitOfWork<PermissionsContext>> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IEventProducer> _mockEventProducer;
        private readonly Mock<IPermissionsRepository> _mockPermissionsRepository;
        private readonly GetPermissionsAllHandler _handler;

        public GetPermissionsAllHandlerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork<PermissionsContext>>();
            _mockMapper = new Mock<IMapper>();
            _mockEventProducer = new Mock<IEventProducer>();
            _mockPermissionsRepository = new Mock<IPermissionsRepository>();

            _handler = new GetPermissionsAllHandler(
                _mockUnitOfWork.Object,
                _mockMapper.Object,
                _mockPermissionsRepository.Object,
                _mockEventProducer.Object
            );
        }
        [Fact]
        public async Task Handle_Should_Return_ResultResponse_With_Correct_Data()
        {
            // Arrange
            var query = new GetPermissionsAllQuery
            {
                Id = 0 // or set to a specific ID if needed
            };

            var permissionsDomains = new List<PermissionsDomain>
            {
                new PermissionsDomain { Id = 1, PermissionsType = new PermissionsTypeDomain() },
                new PermissionsDomain { Id = 2, PermissionsType = new PermissionsTypeDomain() }
            };

            var dtoList = new List<GetPermissionsAllDTO>
            {
                new GetPermissionsAllDTO { /* initialize properties */ },
                new GetPermissionsAllDTO { /* initialize properties */ }
            };

            _mockPermissionsRepository.Setup(repo => repo.All(true))
                .Returns(permissionsDomains.AsQueryable());

            _mockMapper.Setup(m => m.Map<List<GetPermissionsAllDTO>>(It.IsAny<List<PermissionsDomain>>()))
                .Returns(dtoList);

            var result = await _handler.Handle(query, CancellationToken.None);


            Assert.True(result.IsValid, result.Messages);
            _mockEventProducer.Verify(ep => ep.Produce("permissions_topic", It.IsAny<string>()), Times.Once);
        }


    }
}
