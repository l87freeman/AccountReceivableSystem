using System;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using AccountReceivableSystem.Domain.Exceptions;
using AccountReceivableSystem.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace AccountReceivableSystem.Web.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var errorResponse = context.Exception switch
        {
            CreateInvoiceConflictException conflictException => ToErrorResponse(conflictException, HttpStatusCode.BadRequest),
            InvoiceNotFoundException notFoundException => ToErrorResponse(notFoundException, HttpStatusCode.NotFound),
            NotValidInvoiceException notValidException => ToErrorResponse(notValidException, HttpStatusCode.BadRequest),
            UserIdentityNotFound identityNotFoundException => ToErrorResponse(identityNotFoundException, HttpStatusCode.NotFound),
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