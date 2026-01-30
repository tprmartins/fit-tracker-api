using FitTracker.Domain.Shared;

namespace FitTracker.Domain.Errors
{
    public static class DomainErrors
    {
        public static class Document
        {
            public static readonly Error Empty = new(
                "Document.Empty",
                "Document is empty");

            public static readonly Error InvalidFormat = new(
                "Document.InvalidFormat",
                "Document format is invalid");
        }

        public static class User
        {
            public static readonly Error InvalidCredentials = new(
                "User.InvalidCredentials",
                "Invalid credentials");

            public static readonly Error AlreadyExists = new(
                "User.AlreadyExists",
                "User Already Exists");

            public static readonly Error EmailAlreadyExists = new(
                "User.EmailAlreadyExists",
                "User Email Already Exists");

            public static readonly Error Invalid = new(
                "User.Invalid",
                "User is Invalid");

            public static readonly Error NotFound = new(
                "User.NotFound",
                "User Not Found");

            public static readonly Error ActionNotPermitted = new(
                "User.ActionNotPermitted",
                "Action Not Permitted");

            public static readonly Error RegistrationAlreadyCompleted = new(
                "User.RegistrationAlreadyCompleted",
                "User registration is already completed");

            public static readonly Error RegistrationTokenExpired = new(
                "User.RegistrationTokenExpired",
                "Registration token has expired");
        }

        public static class Email
        {
            public static readonly Error Empty = new(
                "Email.Empty",
                "Email is empty");

            public static readonly Error TooLong = new(
                "Email.TooLong",
                "Email is too long");

            public static readonly Error InvalidFormat = new(
                "Email.InvalidFormat",
                "Email format is invalid");
        }
    }
}
