using Auth.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Patient.Application.Patient.Commands.AddMedicalHistoryRecord;
using Patient.Application.Patient.Commands.RegisterPatient;
using Patient.Application.Patient.Commands.RemoveMedicalHistoryRecord;
using Patient.Application.Patient.Commands.UpdatePatientProfile;
using Patient.Application.Patient.Queries;
using Patient.Domain.ValueObjects;
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
            return Ok(Result.Success("patient profile registered successfully."));
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdatePatientProfileCommand request)
        {
            var result = await _mediator.Send(request);
            if (result.IsFailure)
                return BadRequest(result);
            return Ok(Result.Success("patient profile updated successfully."));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPatientDetails(Guid id)
        {
            return Ok(await _mediator.Send(new GetPatientDataQuery(new PatientId(id))));
        }

        [HttpPost("AddMedicalHistoryRecord")]
        public async Task<IActionResult> AddMedicalHistoryRecord([FromBody] AddMedicalHistoryRecordCommand request)
        {
            var result = await _mediator.Send(request);
            if (result.IsFailure)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpDelete("RemoveMedicalHistoryRecord")]
        public async Task<IActionResult> RemoveMedicalHistoryRecord([FromBody] RemoveMedicalHistoryRecordCommand request)
        {
            var result = await _mediator.Send(request);
            if(result.IsFailure)
                return BadRequest(result);
            return Ok(result);
        }
    }
}
