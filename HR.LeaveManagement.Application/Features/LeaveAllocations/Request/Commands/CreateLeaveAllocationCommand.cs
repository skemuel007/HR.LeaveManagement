using HR.LeaveManagement.Application.DTOs.LeaveAllocation;
using HR.LeaveManagement.Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HR.LeaveManagement.Application.Features.LeaveAllocations.Request.Commands
{
    public class CreateLeaveAllocationCommand : IRequest<BaseCommandResponse<int>>
    {
        public CreateLeaveAllocationDto LeaveAllocationDto { get; set; }
    }
}
