using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceAPI.Extensions
{
    public static class SwaggerServiceExtention
    {
        public static IServiceCollection AddSwaggerServices(this IServiceCollection services) {
           services.AddSwaggerGen();
          
           return services;
         }

        public static IApplicationBuilder UseSwaggerDocumention(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            return app;
        }
    }

}