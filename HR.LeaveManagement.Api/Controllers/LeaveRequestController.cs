using HR.LeaveManagement.Application.DTOs.LeaveRequest;
using HR.LeaveManagement.Application.Features.LeaveRequests.Requests.Commands;
using HR.LeaveManagement.Application.Features.LeaveRequests.Requests.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HR.LeaveManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveRequestController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LeaveRequestController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        // GET: api/<LeaveRequestController>
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var response = await _mediator.Send(new GetLeaveRequestListRequest());
            return Ok(response);
        }

        // GET api/<LeaveRequestController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            var response = await _mediator.Send(new GetLeaveRequestDetailRequest{ Id = id });
            return Ok(response);
        }

        // POST api/<LeaveRequestController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CreateLeaveRequestDto request)
        {
            var command = new CreateLeaveRequestCommand { LeaveRequestDto = request };
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        // PUT api/<LeaveRequestController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] UpdateLeaveRequestDto request)
        {
            var command = new UpdateLeaveRequestCommand { LeaveRequestDto = request, Id = id };
            var response = await _mediator.Send(command);
            return NoContent();
        }


        [HttpPut("change_approval/{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] ChangeLeaveRequestApproval request)
        {
            var command = new UpdateLeaveRequestCommand { ChangeLeaveRequestApproval = request, Id = id };
            var response = await _mediator.Send(command);
            return NoContent();
        }

        // DELETE api/<LeaveRequestController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var command = new DeleteLeaveRequestCommand { Id = id };
            var response = _mediator.Send(command);
            return NoContent();
        }
    }
}
