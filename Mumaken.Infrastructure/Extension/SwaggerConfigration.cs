using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Mumaken.Infrastructure.Extension
{
    public class SwaggerConfigration : IConfigureOptions<SwaggerGenOptions>
    {
        public void Configure(SwaggerGenOptions c)
        {
            c.SwaggerDoc("AuthAPI", new OpenApiInfo { Title = "Auth API", Version = "v1" });
            c.SwaggerDoc("UserAPI", new OpenApiInfo { Title = "User API", Version = "v1" });
            c.SwaggerDoc("MoreApi", new OpenApiInfo { Title = "More API", Version = "v1" });
            c.SwaggerDoc("GeneralApi", new OpenApiInfo { Title = "General API", Version = "v1" });
            c.SwaggerDoc("OrderClient", new OpenApiInfo { Title = "OrderClient API", Version = "v1" });
            c.SwaggerDoc("ChatApi", new OpenApiInfo { Title = "ChatApi", Version = "v1" });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme."
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}


                    }
                });
            string xmlPath1 = System.IO.Path.Combine(Environment.CurrentDirectory, "Mumaken.Domain.xml");
            string xmlPath2 = System.IO.Path.Combine(Environment.CurrentDirectory, "Mumaken.xml");

            c.IncludeXmlComments(xmlPath1);
            c.IncludeXmlComments(xmlPath2);

        }
    }

    public class SwaggerUIConfiguration : IConfigureOptions<SwaggerUIOptions>
    {
        public void Configure(SwaggerUIOptions options)
        {

            options.RoutePrefix = "SwaggerPlus";
            options.SwaggerEndpoint("/swagger/AuthAPI/swagger.json", "Auth API V1");
            options.SwaggerEndpoint("/swagger/UserAPI/swagger.json", "User API V1");
            options.SwaggerEndpoint("/swagger/MoreApi/swagger.json", "More API V1");
            options.SwaggerEndpoint("/swagger/GeneralApi/swagger.json", "General API V1");
            options.SwaggerEndpoint("/swagger/OrderClient/swagger.json", "Order Client API V1");
            options.SwaggerEndpoint("/swagger/ChatApi/swagger.json", "Chat API V1");

        }
    }
}
