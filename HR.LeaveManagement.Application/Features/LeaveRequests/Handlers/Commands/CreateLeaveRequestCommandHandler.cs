using AutoMapper;
using FluentValidation.Results;
using HR.LeaveManagement.Application.Contracts.Infastructure;
using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Application.DTOs.LeaveRequest.Validators;
using HR.LeaveManagement.Application.Exceptions;
using HR.LeaveManagement.Application.Features.LeaveAllocations.Request.Commands;
using HR.LeaveManagement.Application.Features.LeaveRequests.Requests.Commands;
using HR.LeaveManagement.Application.Models;
using HR.LeaveManagement.Application.Responses;
using HR.LeaveManagement.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Features.LeaveRequests.Handlers.Commands
{
    public  class CreateLeaveRequestCommandHandler : IRequestHandler<CreateLeaveRequestCommand, BaseCommandResponse<int>>
    {
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly IEmailSender _emailSender;
        private readonly IMapper _mapper;
        public CreateLeaveRequestCommandHandler(
            ILeaveRequestRepository leaveRequestRepository,
            ILeaveTypeRepository leaveTypeRepository,
            IEmailSender emailSender,
            IMapper mapper)
        {
            _leaveRequestRepository = leaveRequestRepository ?? throw new ArgumentNullException(nameof(leaveRequestRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _leaveTypeRepository = leaveTypeRepository ?? throw new ArgumentNullException(nameof(leaveTypeRepository)); 
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
        }

        public async Task<BaseCommandResponse<int>> Handle(CreateLeaveRequestCommand command, CancellationToken cancellationToken)
        {

            var response = new BaseCommandResponse<int>();

            var validator = new CreateLeaveRequestDtoValidator(_leaveTypeRepository);
            var validatorResult = await validator.ValidateAsync(command.LeaveRequestDto);

            if (validatorResult.IsValid == false)
            {
                response.Success = false;
                response.Message = "Creation failed";
                response.Errors = validatorResult.Errors.Select(q => q.ErrorMessage).ToList();

                return response;
            }

            var leaveRequest = _mapper.Map<LeaveRequest>(command.LeaveRequestDto);
            leaveRequest = await _leaveRequestRepository.AddAsync(leaveRequest);

            response.Success = true;
            response.Message = "Creation Successful";
            response.Data = leaveRequest.Id;

            var email = new Email
            {
                To = "employee@org.com",
                Body = $"Your leave request for {command.LeaveRequestDto.StartDate:D} to {command.LeaveRequestDto.EndDate:D} has been submitted successfully",
                Subject = "Leave Request Submitted"
            };

            try
            {
                await _emailSender.SendEmail(email);
            } catch(Exception ex)
            {
                // Log or handle error, but don't throw
            }

            return response;
        }
    }
}
