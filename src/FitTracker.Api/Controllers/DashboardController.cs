using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FitTracker.Api.Abstractions;
using FitTracker.Application.Services.Dashboard;
using FitTracker.Domain.Shared;

namespace FitTracker.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class DashboardController : ApiController
    {
        public DashboardController(ISender sender) : base(sender)
        {
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetStats(CancellationToken cancellationToken)
        {
            var query = new GetPersonalDashboardStatsQuery();

            Result<PersonalDashboardStatsResponse> result = await Sender.Send(query, cancellationToken);

            if (result.IsFailure)
            {
                return HandleFailure(result);
            }

            return Ok(result.Value);
        }

        [HttpGet("activities")]
        public async Task<IActionResult> GetActivities(CancellationToken cancellationToken)
        {
            var query = new GetRecentActivitiesQuery();

            Result<List<RecentActivityResponse>> result = await Sender.Send(query, cancellationToken);

            if (result.IsFailure)
            {
                return HandleFailure(result);
            }

            return Ok(result.Value);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("admin/stats")]
        public async Task<IActionResult> GetAdminStats(CancellationToken cancellationToken)
        {
            var query = new GetAdminDashboardStatsQuery();

            Result<AdminDashboardStatsResponse> result = await Sender.Send(query, cancellationToken);

            if (result.IsFailure)
            {
                return HandleFailure(result);
            }

            return Ok(result.Value);
        }
    }
}
