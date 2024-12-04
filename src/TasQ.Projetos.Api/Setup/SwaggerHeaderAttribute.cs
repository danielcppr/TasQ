using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TasQ.Projetos.Api.Setup;

public class SwaggerHeaderAttribute : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var requireHeaderAttributes = context.MethodInfo
            .GetCustomAttributes(true)
            .OfType<RequireHeadersAttribute>();

        var allHeaderNames = requireHeaderAttributes
            .SelectMany(attribute => attribute.HeaderNames);

        foreach (var headerName in allHeaderNames)
        {
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = headerName,
                In = ParameterLocation.Header,
                Required = true,
                Description = $"{headerName}",
                Schema = new OpenApiSchema { Type = "string" }
            });
        }
    }
}
