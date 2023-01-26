using Microsoft.AspNetCore.Mvc;
using RtgsGlobal.TechTest.Api.Exceptions;

namespace RtgsGlobal.TechTest.Api.Middleware;

public class ErrorHandlingMiddleware : IMiddleware
{
	public async Task InvokeAsync(HttpContext context, RequestDelegate next)
	{
		try
		{
			await next(context);
		}
		catch (NotFoundException exception)
		{
			var problemDetails = new ProblemDetails {Status = StatusCodes.Status404NotFound, Title = "The specified resource was not found.", Type = "https://www.rfc-editor.org/rfc/rfc7231#section-6.5.4", Detail = exception.Message};

			context.Response.StatusCode = problemDetails.Status.Value;
			context.Response.ContentType = "application/json";
			await context.Response.WriteAsJsonAsync(problemDetails);
		}
	}
}
