﻿
using Restaurants.Domain.Exceptions;

namespace Restaurants.API.Middelwares;

public class ErrorHandelingMiddleware(ILogger<ErrorHandelingMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try {
            await next.Invoke(context);
        }
        catch(NotFoundException notFound)
        {
           
            context.Response.StatusCode = 404;
            await context.Response.WriteAsync(notFound.Message);
            logger.LogWarning(notFound.Message);
        }
        catch (ForbidException)
        {

            context.Response.StatusCode = 403;
            await context.Response.WriteAsync("Access Forbiden");

        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("Something went wrong");
        }
    }
}