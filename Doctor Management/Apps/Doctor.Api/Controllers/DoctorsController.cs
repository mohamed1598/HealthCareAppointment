using Doctor.Application.Doctor.Commands.RegisterDoctor;
using Doctor.Application.Doctor.Commands.UpdateDoctor;
using Doctor.Application.Doctor.Queries;
using Doctor.Domain.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Result;

namespace Patient.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DoctorsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegisterDoctorCommand request)
    {
        var result = await _mediator.Send(request);
        if (result.IsFailure)
            return BadRequest(result);
        return Ok(Result.Success("Doctor profile registered successfully."));
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateDoctorCommand request)
    {
        var result = await _mediator.Send(request);
        if (result.IsFailure)
            return BadRequest(result);
        return Ok(Result.Success("Doctor profile updated successfully."));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetDoctorDetails(Guid id)
    {
        return Ok(await _mediator.Send(new GetDoctorDetailsQuery(new DoctorId(id))));
    }
}
