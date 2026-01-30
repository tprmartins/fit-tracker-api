using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FitTracker.Api.Abstractions;
using FitTracker.Application.Services.Users.GetByRole;
using FitTracker.Application.Services.Users.Login;
using FitTracker.Domain.Enums;
using FitTracker.Domain.Shared;

namespace FitTracker.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class StudentController : ApiController
    {
        public StudentController(ISender sender) : base(sender)
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStudents(CancellationToken cancellationToken)
        {
            var query = new GetUsersByRoleQuery((int)UserRole.Student);
            
            Result<List<UserResponse>> result = await Sender.Send(query, cancellationToken);

            if (result.IsFailure)
            {
                return HandleFailure(result);
            }

            return Ok(result.Value);
        }
    }
}
