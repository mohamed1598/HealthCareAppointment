using Auth.Commands;
using Auth.Dtos;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
        {
            var result = await _mediator.Send(new RegisterUserCommand(request.Email, request.Password));
            if (!result.Succeeded)
            {
                return BadRequest(string.Join(" , ", result.Errors.Select(e => e.Description).ToArray()));
            }

            return Ok("User registered successfully.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var loginResponse = await _mediator.Send(new LoginCommand(request.Email, request.Password));
            if (loginResponse is null)
                return Unauthorized();

            return Ok(loginResponse);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] PasswordResetRequest request)
        {
            var result = await _mediator.Send(new ResetPasswordCommand(request.Email, request.CurrentPassword, request.NewPassword));
            if (!result.Succeeded)
                return BadRequest(string.Join(" , ",result.Errors.Select(e => e.Description))); 

            return Ok("password resetted successfully");
        }
    }
}
