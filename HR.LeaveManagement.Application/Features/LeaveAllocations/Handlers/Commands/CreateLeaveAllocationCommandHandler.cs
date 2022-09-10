using AutoMapper;
using FluentValidation.Results;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.DTOs.LeaveAllocation;
using HR.LeaveManagement.Application.DTOs.LeaveAllocation.Validators;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Features.LeaveAllocations.Request.Commands;
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

namespace HR.LeaveManagement.Application.Features.LeaveAllocations.Handlers.Commands
{
    public class CreateLeaveAllocationCommandHandler : IRequestHandler<CreateLeaveAllocationCommand, BaseCommandResponse<int>>
    {
        private readonly ILeaveAllocationRepository _leaveAllocationRepository;
        private readonly ILeaveTypeRepository _leaveTypeRespository;
        private readonly IMapper _mapper;

        public CreateLeaveAllocationCommandHandler(
            ILeaveAllocationRepository leaveAllocationRepository,
            ILeaveTypeRepository leaveTypeRespository,
            IMapper mapper)
        {
            _leaveAllocationRepository = leaveAllocationRepository ?? throw new ArgumentNullException(nameof(leaveAllocationRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _leaveTypeRespository = leaveTypeRespository ?? throw new ArgumentNullException(nameof(_leaveTypeRespository));
        }

        public async Task<BaseCommandResponse<int>> Handle(CreateLeaveAllocationCommand command, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse<int>();

            var validator = new CreateLeaveAllocationDtoValidator(_leaveTypeRespository);
            var validationResult = await validator.ValidateAsync(command.LeaveAllocationDto);

            if (validationResult.IsValid == false)
            {
                response.Success = false;
                response.Message = "Creation failed";
                response.Errors = validationResult.Errors.Select(q => q.ErrorMessage).ToList();

                return response;
                // throw new ValidationException(validationResult);
            }

            var leaveAllocation = _mapper.Map<LeaveAllocation>(command.LeaveAllocationDto);
            leaveAllocation = await _leaveAllocationRepository.AddAsync(leaveAllocation);
            // return leaveAllocation.Id;

            response.Success = true;
            response.Message = "Creation Successful";
            response.Data = leaveAllocation.Id;
            return response;
        }
    }
}
