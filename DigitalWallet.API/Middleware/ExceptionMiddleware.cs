using System.Text.Json;
using Microsoft.Data.SqlClient;
using DigitalWallet.Application.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace DigitalWallet.API.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(
    RequestDelegate next,
    ILogger<ExceptionMiddleware> logger)
    {
        _next = next;

        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            // Continue to the next middleware
            await _next(context);
        }
        catch (Exception ex)
        {

            _logger.LogError( ex, "Exception occurred on {Path}", context.Request.Path);

            context.Response.ContentType =
                "application/json";


            if (ex is SqlException sqlException)
            {
                if (sqlException.Number == 2601 ||
                   sqlException.Number == 2627)
                {
                    context.Response.StatusCode = 409;


                    await context.Response.WriteAsJsonAsync(
                        new
                        {
                            Success = false,
                            Message = "Duplicate transaction."
                        });


                    return;
                }
            }

            if (ex is AppException appException)
            {
                context.Response.StatusCode =
                    appException.StatusCode;


                await context.Response.WriteAsJsonAsync(
                    new
                    {
                        Success = false,

                        Message = appException.Message
                    });


                return;
            }


            if (ex is DbUpdateConcurrencyException)
            {
                context.Response.StatusCode = StatusCodes.Status409Conflict;

                await context.Response.WriteAsJsonAsync(
                    new
                    {
                        Success = false,
                        Message = "The wallet was modified by another operation. Please try again."
                    });

                return;
            }


            context.Response.StatusCode = 500;


            await context.Response.WriteAsJsonAsync(
                new
                {
                    Success = false,

                    Message = "Internal server error."
                });
        }
    }
}