namespace FitTracker.Application.Abstractions
{
    public interface IPasswordHash
    {
        bool Verify(string hashPassword, string password);
        string Hash(string password);
    }
}
