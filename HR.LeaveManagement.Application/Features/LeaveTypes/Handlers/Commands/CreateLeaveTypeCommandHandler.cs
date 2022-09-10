using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.DTOs.LeaveType;
using HR.LeaveManagement.Application.DTOs.LeaveType.Validators;
using HR.LeaveManagement.Application.Features.LeaveTypes.Requests.Commands;
using HR.LeaveManagement.Application.Responses;
using HR.LeaveManagement.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Features.LeaveTypes.Handlers.Commands
{
    public class CreateLeaveTypeCommandHandler: IRequestHandler<CreateLeaveTypeCommand, BaseCommandResponse<int>>
    {
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly IMapper _mapper;
        public CreateLeaveTypeCommandHandler(
            ILeaveTypeRepository leaveTypeRepository,
            IMapper mapper)
        {
            _leaveTypeRepository = leaveTypeRepository ?? throw new ArgumentNullException(nameof(leaveTypeRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<BaseCommandResponse<int>> Handle(CreateLeaveTypeCommand command, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse<int>();

            var validator = new CreateLeaveTypeDtoValidator();
            var validationResult = await validator.ValidateAsync(command.LeaveTypeDto);

            if (validationResult.IsValid == false)
            {
                response.Success = false;
                response.Message = $"Leavetype creation failed";
                response.Errors = validationResult.Errors.Select(q => q.ErrorMessage).ToList();

                return response;
                // throw new ValidationException((IEnumerable<ValidationFailure>)(validationResult as ValidationResult));
            }

            var leaveType = _mapper.Map<LeaveType>(command.LeaveTypeDto);
            leaveType = await _leaveTypeRepository.AddAsync(leaveType);
            
            response.Success = true;
            response.Message = "Leave type creation succesful";
            return response;
        }
    }
}
