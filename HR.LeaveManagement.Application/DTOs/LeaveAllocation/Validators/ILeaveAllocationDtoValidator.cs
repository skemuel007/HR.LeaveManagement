using FluentValidation;
using HR.LeaveManagement.Application.Contracts.Persistence;
using System;

namespace HR.LeaveManagement.Application.DTOs.LeaveAllocation.Validators
{
    public class ILeaveAllocationDtoValidator : AbstractValidator<ILeaveAllocationDto>
    {
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        public ILeaveAllocationDtoValidator(ILeaveTypeRepository leaveTypeRepository)
        {
            _leaveTypeRepository = leaveTypeRepository;
            RuleFor(la => la.Period)
                .GreaterThan(DateTime.Now.Year).WithMessage("{PropertyName} must be before {ComparisonValue}");

            RuleFor(la => la.NumberOfDays)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0");


            RuleFor(la => la.LeaveTypeId).GreaterThan(0)
                .MustAsync(async (id, token) =>
                {
                    var leaveTypeExists = await _leaveTypeRepository.AnyAsyc(x => x.Id == id);
                    return !leaveTypeExists;

                }).WithMessage("{PropertyName} does not exits.");
        }
    }
}
