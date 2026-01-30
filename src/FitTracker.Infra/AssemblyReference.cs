using System.Reflection;

namespace FitTracker.Infra;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}