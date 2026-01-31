using FitTracker.Application.Abstractions;
using FitTracker.Application.Abstractions.Messaging;
using FitTracker.Domain.Entities.Users;
using FitTracker.Domain.Entities.Workouts;
using FitTracker.Domain.Repositories;
using FitTracker.Domain.Shared;

namespace FitTracker.Application.Services.Workouts.Execute
{
    internal sealed class ExecuteWorkoutCommandHandler : ICommandHandler<ExecuteWorkoutCommand>
    {
        private readonly IWorkoutExecutionRepository _workoutExecutionRepository;
        private readonly IUserContext _userContext;
        private readonly IUnitOfWork _unitOfWork;

        public ExecuteWorkoutCommandHandler(
            IWorkoutExecutionRepository workoutExecutionRepository,
            IUserContext userContext,
            IUnitOfWork unitOfWork)
        {
            _workoutExecutionRepository = workoutExecutionRepository;
            _userContext = userContext;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(ExecuteWorkoutCommand request, CancellationToken cancellationToken)
        {
            var userId = _userContext.UserId;

            var execution = new WorkoutExecution(
                Guid.NewGuid(),
                request.WorkoutId,
                new UserId(userId)
            );

            _workoutExecutionRepository.Add(execution);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
