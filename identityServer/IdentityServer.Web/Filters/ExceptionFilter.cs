using System.Net;
using IdentityServer.Application.Exceptions;
using IdentityServer.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace IdentityServer.Web.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var errorResponse = context.Exception switch
        {
            CreateConflictException conflictException => ToErrorResponse(conflictException, HttpStatusCode.BadRequest),
            IncorrectLoginDataException notFoundException => ToErrorResponse(notFoundException, HttpStatusCode.NotFound),
            Exception otherException => ToErrorResponse(otherException, HttpStatusCode.InternalServerError),
        };

        context.Result = errorResponse;

        context.ExceptionHandled = true;
    }

    private static ObjectResult ToErrorResponse(Exception exception, HttpStatusCode statusCode)
    {
        return new(new ErrorModel(exception.Message))
        {
            StatusCode = (int)statusCode,
        };
    }
}