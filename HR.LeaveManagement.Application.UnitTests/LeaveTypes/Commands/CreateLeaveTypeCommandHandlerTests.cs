using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.DTOs.LeaveType;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Features.LeaveTypes.Handlers.Commands;
using HR.LeaveManagement.Application.Responses;
using HR.LeaveManagement.Application.UnitTests.Mocks;
using HR.LeaveManagement.Domain;
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

namespace HR.LeaveManagement.Application.UnitTests.LeaveTypes.Commands
{
    public class CreateLeaveTypeCommandHandlerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<ILeaveTypeRepository> _mockRepo;
        private readonly CreateLeaveTypeDto _leaveTypeDto;
        public CreateLeaveTypeCommandHandlerTests()
        {
            _mockRepo = MockLeaveTypeRepository.GetLeaveTypeRepository();

            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });

            _mapper = mapperConfig.CreateMapper();

            _leaveTypeDto = new CreateLeaveTypeDto
            {
                DefaultDays = 15,
                Name = "Test Leave Day"
            };
        }

        [Fact]
        public async Task CreateLeaveType()
        {
            var handler = new CreateLeaveTypeCommandHandler(_mockRepo.Object, _mapper);
            var result = await handler.Handle(new Features.LeaveTypes.Requests.Commands.CreateLeaveTypeCommand()
            {
                LeaveTypeDto = _leaveTypeDto
            }, cancellationToken: CancellationToken.None);

            var leaveTypes = await _mockRepo.Object.GetAllAsync();
            result.ShouldBeOfType<BaseCommandResponse<int>>();
            leaveTypes.Count.ShouldBeGreaterThan(2);
        }

        [Fact]
        public async Task InvalidLeaveTypeAdded()
        {
            var handler = new CreateLeaveTypeCommandHandler(_mockRepo.Object, _mapper);
            _leaveTypeDto.DefaultDays = -1;

            /*ValidationException ex = await Should.ThrowAsync<ValidationException>(
                async () => await handler.Handle(new Features.LeaveTypes.Requests.Commands.CreateLeaveTypeCommand()
                {
                    LeaveTypeDto = _leaveTypeDto
                }, CancellationToken.None));*/

            var result = await handler.Handle(new Features.LeaveTypes.Requests.Commands.CreateLeaveTypeCommand()
            {
                LeaveTypeDto = _leaveTypeDto
            }, CancellationToken.None);

            var leaveTypes = await _mockRepo.Object.GetAllAsync();

            Assert.IsType<bool>(result.Success);
            Assert.False(result.Success);
        }
    }
}
