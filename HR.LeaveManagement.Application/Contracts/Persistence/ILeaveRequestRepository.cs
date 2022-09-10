﻿using HR.LeaveManagement.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Application.Contracts.Persistence
{
    public interface ILeaveRequestRepository : IGenericRepository<LeaveRequest>
    {
        Task<LeaveRequest> GetLeaveRequestWithDetailsAsync(int id);
        Task<List<LeaveRequest>> GetLeaveRequestListWithDetailsAsync();
        Task ChangeApprovalStatus(LeaveRequest leaveRequest, bool approved);
    }
}
