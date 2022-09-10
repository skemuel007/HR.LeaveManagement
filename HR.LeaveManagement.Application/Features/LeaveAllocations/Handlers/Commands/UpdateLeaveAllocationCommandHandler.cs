using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.DTOs.LeaveAllocation.Validators;
using HR.LeaveManagement.Application.Features.LeaveAllocations.Request.Commands;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Features.LeaveAllocations.Handlers.Commands
{
    public class UpdateLeaveAllocationCommandHandler : IRequestHandler<UpdateLeaveAllocationCommand, Unit>
    {
        private readonly ILeaveAllocationRepository _leaveAllocationRepository;
        private readonly IMapper _mapper;
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        public UpdateLeaveAllocationCommandHandler(
            ILeaveAllocationRepository leaveAllocationRepository,
            ILeaveTypeRepository leaveTypeRepository,
            IMapper mapper)
        {
            _leaveAllocationRepository = leaveAllocationRepository ?? throw new ArgumentNullException(nameof(leaveAllocationRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _leaveTypeRepository = leaveTypeRepository ?? throw new ArgumentNullException(nameof(leaveTypeRepository)); 
        }

        public async Task<Unit> Handle(UpdateLeaveAllocationCommand command, CancellationToken cancellationToken)
        {

            var validator = new UpdateLeaveAllocationDtoValidator(_leaveTypeRepository);
            var validationResult = await validator.ValidateAsync(command.UpdateLeaveAllocationDto);

            if (validationResult.IsValid == false)
                throw new Exception();

            var leaveType = await _leaveAllocationRepository.GetAsync(command.UpdateLeaveAllocationDto.Id);
            _mapper.Map(command.UpdateLeaveAllocationDto, leaveType);
            await _leaveAllocationRepository.UpdateAsync(leaveType);
            return Unit.Value;
        }
    }
}
