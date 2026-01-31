using FitTracker.Api.Options;
using FitTracker.Infra.Options;
using FitTracker.Application.Behaviors;
using FitTracker.Infra.Context;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Scrutor;
using Serilog;
using FitTracker.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

// builder.Services.AddSingleton(sp => SecretManagerServiceClient.Create());

builder.Services.Scan(
    selector => selector
        .FromAssemblies(FitTracker.Infra.AssemblyReference.Assembly)
        .AddClasses(classes => classes
            .Where(type => !typeof(IHostedService).IsAssignableFrom(type)))
        .UsingRegistrationStrategy(RegistrationStrategy.Skip)
        .AsImplementedInterfaces()
        .WithScopedLifetime());

builder.Services.AddHttpContextAccessor();

builder.Services.AddHttpContextAccessor();

builder.Services.ConfigureOptions<DatabaseOptionsSetup>();
builder.Services.Configure<EmailOptions>(builder.Configuration.GetSection("EmailOptions"));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// builder.Services.AddScoped<IUserRepository, UserRepository>();


builder.Services.AddMediatR(x => x.RegisterServicesFromAssemblies(FitTracker.Application.AssemblyReference.Assembly));

//Validation Result
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
builder.Services.AddValidatorsFromAssembly(FitTracker.Application.AssemblyReference.Assembly, includeInternalTypes: true);

// Exception Handling
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Background Services
builder.Services.AddHostedService<FitTracker.Infra.BackgroundJobs.TokenCleanupService>();

// Rate Limiting
builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = System.Threading.RateLimiting.PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
        System.Threading.RateLimiting.RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Request.Path.ToString(),
            factory: partition => new System.Threading.RateLimiting.FixedWindowRateLimiterOptions
            {
                PermitLimit = 100,
                Window = TimeSpan.FromMinutes(1),
                QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst,
                QueueLimit = 0
            }));

    options.AddPolicy("auth", httpContext =>
        System.Threading.RateLimiting.RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: partition => new System.Threading.RateLimiting.FixedWindowRateLimiterOptions
            {
                PermitLimit = 5,
                Window = TimeSpan.FromMinutes(1),
                QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst,
                QueueLimit = 2
            }));
});

builder.Services.AddDbContext<DatabaseContext>(
    (serviceProvider, dbContextOptionsBuilder) =>
    {
        var databaseOptions = serviceProvider.GetService<IOptions<DatabaseOptions>>()!.Value;

        dbContextOptionsBuilder.UseNpgsql(databaseOptions.ConnectionString, npgsqlOptionsAction =>
        {
            npgsqlOptionsAction.EnableRetryOnFailure(databaseOptions.MaxRetryCount);
            npgsqlOptionsAction.CommandTimeout(databaseOptions.CommandTimeout);
        });

        dbContextOptionsBuilder.EnableDetailedErrors(databaseOptions.EnableDetailedErrors);

        dbContextOptionsBuilder.EnableSensitiveDataLogging(databaseOptions.EnableSensitiveDataLogging);
    });

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, _ => { }); // Configurado via JwtBearerOptionsSetup


builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder()
        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
        .RequireAuthenticatedUser()
        .Build();
});

builder.Services.ConfigureOptions<JwtOptionsSetup>();
builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseExceptionHandler();

app.UseMiddleware<CorrelationIdMiddleware>();

app.UseHttpsRedirection();

app.UseRouting();

app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseCors(builder => builder
       .AllowAnyHeader()
       .AllowAnyMethod()
       .AllowAnyOrigin()
    );

app.Run();

// Make the implicit Program class public for integration tests
public partial class Program { }
