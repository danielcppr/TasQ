using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace TasQ.Projetos.Api.Setup;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class RequireHeadersAttribute : ActionFilterAttribute
{
    public string[] HeaderNames { get; }

    public RequireHeadersAttribute(string[] headerNames)
    {
        HeaderNames = headerNames;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        foreach (var headerName in HeaderNames)
            if (!context.HttpContext.Request.Headers.ContainsKey(headerName))
                context.Result = new BadRequestObjectResult($"Header não informado: {headerName}");
        
        base.OnActionExecuting(context);
    }
}

