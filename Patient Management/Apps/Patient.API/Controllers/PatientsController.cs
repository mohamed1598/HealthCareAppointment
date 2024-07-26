using Auth.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Patient.Application.Patient.Commands.RegisterPatient;
using Patient.Application.Patient.Commands.UpdatePatientProfile;
using Shared.Result;

namespace Patient.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterPatientCommand request)
        {
            var result = await _mediator.Send(request);
            if (result.IsFailure)
                return BadRequest(result);
            return Ok(Result.Success("patinet profile registered successfully."));
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdatePatientProfileCommand request)
        {
            var result = await _mediator.Send(request);
            if (result.IsFailure)
                return BadRequest(result);
            return Ok(Result.Success("patinet profile updated successfully."));
        }
    }
}
