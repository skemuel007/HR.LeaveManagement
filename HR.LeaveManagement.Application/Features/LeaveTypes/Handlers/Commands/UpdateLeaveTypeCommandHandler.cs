using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.DTOs.LeaveType.Validators;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Features.LeaveTypes.Requests.Commands;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Features.LeaveTypes.Handlers.Commands
{
    public class UpdateLeaveTypeCommandHandler : IRequestHandler<UpdateLeaveTypeCommand, Unit>
    {
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly IMapper _mapper;
        public UpdateLeaveTypeCommandHandler(
            ILeaveTypeRepository leaveTypeRepository,
            IMapper mapper)
        {
            _leaveTypeRepository = leaveTypeRepository ?? throw new ArgumentNullException(nameof(leaveTypeRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Unit> Handle(UpdateLeaveTypeCommand command, CancellationToken cancellationToken)
        {

            var validator = new UpdateLeaveTypeDtoValidator();
            var validationResult = await validator.ValidateAsync(command.LeaveTypeDto);

            if (validationResult != null)
                throw new ValidationException(validationResult);

            var leaveType = await _leaveTypeRepository.GetAsync(command.LeaveTypeDto.Id);
            _mapper.Map(command.LeaveTypeDto, leaveType);
            await _leaveTypeRepository.UpdateAsync(leaveType);
            return Unit.Value;
        }
    }
}
