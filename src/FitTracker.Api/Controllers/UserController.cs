using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FitTracker.Api.Abstractions;
using FitTracker.Application.Services.Users.Login;
using FitTracker.Application.Services.Users.RefreshTokens;
using FitTracker.Application.Services.Users.Register;
using FitTracker.Application.Services.Users.Invite;
using FitTracker.Application.Services.Users.CompleteRegistration;
using FitTracker.Domain.Repositories;
using FitTracker.Domain.Shared;
using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.RateLimiting;

namespace FitTracker.Api.Controllers
{
    [Route("api/[controller]")]
    public class UserController : ApiController
    {
        public UserController(ISender sender)
            : base(sender)
        {
        }

        [HttpPost("login")]
        [EnableRateLimiting("auth")]
        public async Task<IActionResult> LoginUser([FromBody]LoginRequest request, CancellationToken cancellationToken)
        {
            var command = new LoginCommand(request.Document, request.Password);

            Result<LoginResponse> tokenResult = await Sender.Send(command, cancellationToken);

            if (tokenResult.IsFailure)
            {
                return HandleFailure(tokenResult);
            }

            return Ok(tokenResult.Value);
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterRequest request, CancellationToken cancellationToken)
        {
            var command = new RegisterCommand(request.Document, request.Password, request.Name, request.Email, request.Phone, request.Role);

            Result<RegisterResponse> result = await Sender.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return HandleFailure(result);
            }

            return Ok(result.Value);
        }

        [Authorize]
        [HttpGet]
        public ActionResult Get()
        {
            return StatusCode((int)HttpStatusCode.OK);
        }

        [AllowAnonymous]
        [HttpPost("refresh-tokens")]
        [EnableRateLimiting("auth")]
        public async Task<IActionResult> GetRefreshTokens([FromBody] RefreshTokensRequest request, CancellationToken cancellationToken)
        {
            var command = new RefreshTokensCommand(request.Token, request.RefreshToken);

            Result<RefreshTokensResponse> tokenResult = await Sender.Send(command, cancellationToken);

            if (tokenResult.IsFailure)
            {
                return HandleFailure(tokenResult);
            }

            return Ok(tokenResult.Value);
        }

        [Authorize(Roles = "Personal")]
        [HttpPost("invite-student")]
        public async Task<IActionResult> InviteStudent([FromBody] InviteStudentRequest request, CancellationToken cancellationToken)
        {
            var command = new InviteStudentCommand(request.Email, request.Name, request.Phone);

            Result<InviteStudentResponse> result = await Sender.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return HandleFailure(result);
            }

            return Ok(result.Value);
        }

        [AllowAnonymous]
        [HttpPost("complete-registration")]
        public async Task<IActionResult> CompleteRegistration([FromBody] CompleteRegistrationRequest request, CancellationToken cancellationToken)
        {
            var command = new CompleteRegistrationCommand(request.Token, request.Document, request.Password);

            Result result = await Sender.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return HandleFailure(result);
            }

            return Ok();
        }
    }
}
