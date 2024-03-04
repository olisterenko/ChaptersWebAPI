using System.Net;
using Chapters.Entities;
using Chapters.Exceptions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Chapters.Filters;

public class UserExceptionFilter : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        if (context.Exception is EntityNotFoundException<User>)
        {
            var result = new ContentResult
            {
                StatusCode = (int)HttpStatusCode.BadRequest
            };
            context.Result = result;
        }
    }
}