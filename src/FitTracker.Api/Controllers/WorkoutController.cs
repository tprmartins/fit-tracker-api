using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FitTracker.Api.Abstractions;
using FitTracker.Application.Services.Workouts.Create;
using FitTracker.Application.Services.Workouts.GetByStudent;
using FitTracker.Domain.Shared;

namespace FitTracker.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class WorkoutController : ApiController
    {
        public WorkoutController(ISender sender) : base(sender)
        {
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateWorkoutCommand command, CancellationToken cancellationToken)
        {
            Result<Guid> result = await Sender.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return HandleFailure(result);
            }

            return CreatedAtAction(nameof(GetByStudent), new { studentId = command.StudentId }, result.Value);
        }

        [HttpGet("student/{studentId:guid}")]
        public async Task<IActionResult> GetByStudent(Guid studentId, CancellationToken cancellationToken)
        {
            var query = new GetStudentWorkoutsQuery(studentId);
            
            Result<List<WorkoutResponse>> result = await Sender.Send(query, cancellationToken);

            if (result.IsFailure)
            {
                return HandleFailure(result);
            }

            return Ok(result.Value);
        }
    }
}
