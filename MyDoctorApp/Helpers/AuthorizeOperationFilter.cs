using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MyDoctorApp.Helpers
{
    public class AuthorizeOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var authAtrributes = context.MethodInfo
                .GetCustomAttributes(true)
                .OfType<AuthorizeAttribute>()
                .Distinct();

            if (authAtrributes.Any())
            {

                operation.Security = new List<OpenApiSecurityRequirement>();

                var roles = context.MethodInfo
                        .GetCustomAttributes(true)
                        .OfType<AuthorizeAttribute>()
                        .SelectMany(attr => attr.Roles?.Split(',') ?? Array.Empty<string>());

                operation.Security.Add(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Description = "Add token to header",
                            Name = "Authorization",
                            Type = SecuritySchemeType.Http,
                            In = ParameterLocation.Header,
                            Scheme = JwtBearerDefaults.AuthenticationScheme,
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                            }
                        },
                        roles.ToList()
                    }
                });
            }
        }
    }
}
