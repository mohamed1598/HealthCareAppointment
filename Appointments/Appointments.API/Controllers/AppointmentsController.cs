using Appointments.API.Appointments.Commands.CancelAppointment;
using Appointments.API.Appointments.Commands.ScheduleAppointment;
using Appointments.API.Appointments.Queries.GetAppointmentDetails;
using Appointments.API.EventStore;
using Appointments.API.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Result;

namespace Appointments.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AppointmentsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;


    [HttpPost("Schedule")]
    public async Task<IActionResult> Schedule([FromBody] ScheduleAppointmentCommand request)
    {
        var result = await _mediator.Send(request);
        if (result.IsFailure)
            return BadRequest(result);
        return Ok(Result.Success("appointment scheduled successfully."));
    }


    [HttpPost("Cancel")]
    public async Task<IActionResult> Cancel([FromBody] CancelAppointmentDetailsCommand request)
    {
        var result = await _mediator.Send(request);
        if (result.IsFailure)
            return BadRequest(result);
        return Ok(Result.Success("appointment cancelled successfully."));
    }

    [HttpGet]
    public async Task<IActionResult> GetDetails(Guid id)
    {
        return Ok(await _mediator.Send(new GetAppointmentDetailsQuery(id)));
    }
}
