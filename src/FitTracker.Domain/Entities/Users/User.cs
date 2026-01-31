using FitTracker.Domain.Enums;
using FitTracker.Domain.ValueObjects;
using FitTracker.Domain.Errors;
using FitTracker.Domain.Shared;

namespace FitTracker.Domain.Entities.Users
{
    public class User
    {
        public User(UserId id, Email email, Document? document, string name, string? password, string phone, UserRole role, UserStatus status)
        {
            Id = id;
            Email = email;
            Document = document;
            Name = name;
            Password = password;
            Phone = phone;
            Role = role;
            Status = status;
            CreatedAt = DateTime.UtcNow;
        }

        private User() { }

        public UserId Id { get; private set; }

        public Email Email { get; private set; }

        public Document? Document { get; private set; }

        public string Name { get; private set; }

        public string? Password { get; private set; }

        public string Phone { get; private set; } = string.Empty;

        public UserRole Role { get; private set; }

        public UserStatus Status { get; private set; }

        public string? RegistrationToken { get; private set; }

        public DateTime? TokenExpiresAt { get; private set; }

        public bool IsActive { get; private set; } = true;

        public DateTime? BlockedAt { get; private set; }

        public UserId? BlockedBy { get; private set; }

        public bool IsDeleted { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public DateTime? DeletedAt { get; private set; }

        public static User InviteStudent(Email email, string name, string phone)
        {
            return new User(
                new UserId(Guid.NewGuid()),
                email,
                null,
                name,
                null,
                phone,
                UserRole.Student,
                UserStatus.PendingCompletion)
            {
                RegistrationToken = Guid.NewGuid().ToString("N"),
                TokenExpiresAt = DateTime.UtcNow.AddDays(7)
            };
        }

        public Result CompleteRegistration(string passwordHash, Document document)
        {
            if (Status != UserStatus.PendingCompletion)
            {
                return Result.Failure(DomainErrors.User.RegistrationAlreadyCompleted);
            }

            if (TokenExpiresAt < DateTime.UtcNow)
            {
                return Result.Failure(DomainErrors.User.RegistrationTokenExpired);
            }

            Password = passwordHash;
            Document = document;
            Status = UserStatus.Active;
            RegistrationToken = null;
            TokenExpiresAt = null;

            return Result.Success();
        }

        public void Delete()
        {
            IsDeleted = true;
            DeletedAt = DateTime.UtcNow;
        }

        public void Block(UserId blockedBy)
        {
            IsActive = false;
            BlockedAt = DateTime.UtcNow;
            BlockedBy = blockedBy;
        }

        public void Unblock()
        {
            IsActive = true;
            BlockedAt = null;
            BlockedBy = null;
        }
    }
}
