using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HR.LeaveManagement.Persistence.Repositories
{
    public class LeaveRequestRepository : GenericRepository<LeaveRequest>, ILeaveRequestRepository
    {
        private readonly LeaveManagementDbContext _context;

        public LeaveRequestRepository(LeaveManagementDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task ChangeApprovalStatus(LeaveRequest leaveRequest, bool approved)
        {
            leaveRequest.Approved = approved;
            _context.Entry(leaveRequest).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<List<LeaveRequest>> GetLeaveRequestListWithDetailsAsync()
        {
            var leaveRequests = await _context.LeaveRequests
                .Include(l => l.LeaveType)
                .ToListAsync();
            return leaveRequests;
        }

        public async Task<LeaveRequest> GetLeaveRequestWithDetailsAsync(int id)
        {
            var leaveRequest = await _context.LeaveRequests
                .Include(l => l.LeaveType)
                .FirstOrDefaultAsync(l => l.Id == id);
            return leaveRequest;
        }
    }

    public class LeaveAllocationRepository : GenericRepository<LeaveAllocation>, ILeaveAllocationRepository
    {
        private readonly LeaveManagementDbContext _context;
        public LeaveAllocationRepository(LeaveManagementDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<LeaveAllocation>> GetLeaveAllocationListWithDetailsAsync()
        {
            var leaveAllocations = await _context.LeaveAllocations
                .Include(l => l.LeaveType)
                .ToListAsync();

            return leaveAllocations;
        }

        public async Task<LeaveAllocation> GetLeaveAllocationWithDetailsAsync(int id)
        {
            var leaveAllocation = await _context.LeaveAllocations
                .Include(l => l.LeaveType)
                .FirstOrDefaultAsync(l => l.Id == id);

            return leaveAllocation;
        }
    }
}
