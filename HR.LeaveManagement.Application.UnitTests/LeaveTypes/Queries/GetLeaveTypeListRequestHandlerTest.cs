using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.DTOs.LeaveType;
using HR.LeaveManagement.Application.Features.LeaveTypes.Handlers.Queries;
using HR.LeaveManagement.Application.UnitTests.Mocks;
using HR.LeaveManagement.Domain.Profiles;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace HR.LeaveManagement.Application.UnitTests.LeaveTypes.Queries
{
    public class GetLeaveTypeListRequestHandlerTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<ILeaveTypeRepository> _mockRepo;

        public GetLeaveTypeListRequestHandlerTest()
        {
            _mockRepo = MockLeaveTypeRepository.GetLeaveTypeRepository();

            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });

            _mapper = mapperConfig.CreateMapper();
        }

        [Fact]
        public async Task GetLeaveTypeListTest()
        {
            var handler = new GetLeaveTypeListRequestHandler(_mockRepo.Object, _mapper);
            var result = await handler.Handle(new Features.LeaveTypes.Requests.Queries.GetLeaveTypeListRequest(), CancellationToken.None);
            result.ShouldBeOfType<List<LeaveTypeDto>>();
            result.Count.ShouldBeGreaterThan(1);
        }
    }
}
