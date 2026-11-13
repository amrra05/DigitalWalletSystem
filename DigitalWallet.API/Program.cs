using DigitalWallet.API.Middleware;
using DigitalWallet.Application.Interfaces;
using DigitalWallet.Application.Services;
using DigitalWallet.Application.Settings;
using DigitalWallet.Application.Validators;
using DigitalWallet.Infrastructure.Data;
using DigitalWallet.Infrastructure.Repositories;
using DigitalWallet.Infrastructure.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Security.Claims;
using System.Text;
using System.Threading.RateLimiting;


Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File(
        "Logs/log-.txt",
        rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();


var allowedOrigins = (
    builder.Configuration["Frontend:AllowedOrigins"]
    ?? "http://localhost:5173"
).Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReact", policy =>
    {
        policy
            .WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});


builder.Services
    .AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = context.ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .SelectMany(x => x.Value.Errors)
                .Select(x => x.ErrorMessage)
                .ToList();


            return new BadRequestObjectResult(new
            {
                Success = false,

                Message = "Validation failed.",

                Errors = errors
            });
        };
    });

builder.Services.AddValidatorsFromAssembly(
    typeof(TransferRequestValidator).Assembly);

builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IWalletRepository, WalletRepository>();

builder.Services.AddScoped<IWalletService, WalletService>();

builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped< IAuditLogRepository, AuditLogRepository>();

builder.Services.AddScoped< IAuditLogService, AuditLogService>();

builder.Services.AddScoped< IRefreshTokenRepository, RefreshTokenRepository>();

builder.Services.AddScoped<IAdminService, AdminService>();

builder.Services.AddScoped< IPasswordResetRepository,PasswordResetRepository>();

builder.Services.AddScoped<IEmailVerificationRepository,EmailVerificationRepository>();

builder.Services.AddScoped<IAdminRepository, AdminRepository>();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("AzureConnection")
    );
});

builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("JwtSettings")
);

builder.Services
    .AddAuthentication(
        JwtBearerDefaults.AuthenticationScheme
    )
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = false,

                ValidateAudience = false,

                ValidateLifetime = true,

                ValidateIssuerSigningKey = true,

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            builder.Configuration
                            .GetSection("JwtSettings")
                            ["SecretKey"]!
                        )
                    )
            };

        // Check whether the user is still active
        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = async context =>
            {
                var userRepository =
                    context.HttpContext.RequestServices
                    .GetRequiredService<IUserRepository>();

                var userId = context.Principal?
                    .FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(userId, out int id))
                {
                    context.Fail("Invalid token.");
                    return;
                }

                var user = await userRepository.GetByIdAsync(id);

                if (user == null || !user.IsActive)
                {
                    context.Fail("Your account has been deactivated.");
                }
            }
        };
    });

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter: Bearer {your JWT token}"
    });


    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings")
);


builder.Services.AddScoped<IEmailService, EmailService>();


builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.OnRejected = async (context, _) =>
    {
        context.HttpContext.Response.ContentType = "application/json";

        await context.HttpContext.Response.WriteAsJsonAsync(new
        {
            success = false,
            message = "Too many requests. Please try again in one minute."
        });
    };

    options.AddFixedWindowLimiter("AuthLimiter", limiterOptions =>
    {
        limiterOptions.PermitLimit = 5;

        limiterOptions.Window = TimeSpan.FromMinutes(1);

        limiterOptions.QueueLimit = 0;
    });
});


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context =
        scope.ServiceProvider
            .GetRequiredService<AppDbContext>();

    await DbSeeder.SeedAsync(context);
}

app.UseSerilogRequestLogging();

var enableSwagger =
    app.Environment.IsDevelopment() ||
    builder.Configuration.GetValue<bool>("Swagger:Enabled");

if (enableSwagger)
{
    app.UseSwagger();

    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseMiddleware<ExceptionMiddleware>();

app.UseCors("AllowReact");

app.UseRateLimiter();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();