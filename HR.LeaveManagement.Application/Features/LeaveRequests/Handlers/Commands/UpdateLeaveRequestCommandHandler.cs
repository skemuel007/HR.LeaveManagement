using AutoMapper;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.DTOs.LeaveRequest.Validators;
using HR.LeaveManagement.Application.Features.LeaveRequests.Requests.Commands;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Features.LeaveRequests.Handlers.Commands
{
    public class UpdateLeaveRequestCommandHandler : IRequestHandler<UpdateLeaveRequestCommand, Unit>
    {
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly IMapper _mapper;
        public UpdateLeaveRequestCommandHandler(
            ILeaveRequestRepository leaveRequestRepository,
            ILeaveTypeRepository leaveTypeRepository,
            IMapper mapper)
        {
            _leaveRequestRepository = leaveRequestRepository ?? throw new ArgumentNullException(nameof(leaveRequestRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _leaveTypeRepository = leaveTypeRepository ?? throw new ArgumentNullException(nameof(leaveTypeRepository));
        }

        public async Task<Unit> Handle(UpdateLeaveRequestCommand command, CancellationToken cancellationToken)
        {
            
            var leaveRequest = await _leaveRequestRepository.GetAsync(command.Id);
            if (command.LeaveRequestDto != null )
            {
                var validator = new UpdateLeaveRequestDtoValidator(_leaveTypeRepository);
                var validatorResult = await validator.ValidateAsync(command.LeaveRequestDto);

                if (validatorResult.IsValid == false)
                    throw new Exception();

                _mapper.Map(command.LeaveRequestDto, leaveRequest);
                await _leaveRequestRepository.UpdateAsync(leaveRequest);
                
            } else if (command.ChangeLeaveRequestApproval != null )
            {
                await _leaveRequestRepository.ChangeApprovalStatus(leaveRequest, (bool)command.ChangeLeaveRequestApproval.Approved);
            }
            return Unit.Value;
        }
    }
}
