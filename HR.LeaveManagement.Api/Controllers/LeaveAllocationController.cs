using HR.LeaveManagement.Application.DTOs.LeaveAllocation;
using HR.LeaveManagement.Application.Features.LeaveAllocations.Request.Commands;
using HR.LeaveManagement.Application.Features.LeaveAllocations.Request.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HR.LeaveManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveAllocationController : ControllerBase
    {
        private readonly IMediator _mediator;
        public LeaveAllocationController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        // GET: api/<LeaveAllocationController>
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var response = await _mediator.Send(new GetLeaveAllocationListRequest());
            return Ok(response);
        }

        // GET api/<LeaveAllocationController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            var response = await _mediator.Send(new GetLeaveAllocationDetailRequest {  Id = id});
            return Ok(response);
        }

        // POST api/<LeaveAllocationController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CreateLeaveAllocationDto request)
        {
            var command = new CreateLeaveAllocationCommand { LeaveAllocationDto = request };
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        // PUT api/<LeaveAllocationController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] UpdateLeaveAllocationDto request)
        {
            var command = new UpdateLeaveAllocationCommand { UpdateLeaveAllocationDto = request };
            var response = await _mediator.Send(command);
            return NoContent();
        }

        // DELETE api/<LeaveAllocationController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var command = new DeleteLeaveAllocationCommand { Id = id };
            var response = await _mediator.Send(command);
            return NoContent();
        }
    }
}
