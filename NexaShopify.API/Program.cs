
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using NexaShopify.Core.Common.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NexaShopify.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            // builder.Services.AddControllers(); => replaced by (below method) for forcing dev to use ResponseModel
            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<ResponseModelActionFilter>();

            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();
            //builder.Services.AddSwaggerGen(c =>
            //{
            //    c.SchemaFilter<ResponseModelSchemaFilter>();  // Adding the schema filter for ResponseModel
            //});

            builder.Services.AddSwaggerGen(c =>
            {
                // c.SchemaFilter<ResponseModelSchemaFilter>();
                //c.DocumentFilter<EnforceResponseModelDocumentFilter>();
                c.EnableAnnotations();
            });


            #region Midlleware

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();
            app.MapControllerRoute(
            name: "area",
            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            app.Run();
            #endregion

        }


        #region >> imposing a coonstraint with swagger on the type of return, in other word type of return must be only ResponseModel format

        #endregion

        // Define the schema filter to ensure responses are wrapped in ResponseModel<T>
        public class ResponseModelActionFilter : IActionFilter
        {
            public void OnActionExecuting(ActionExecutingContext context) { }

            public void OnActionExecuted(ActionExecutedContext context)
            {
                if (context.Result is ObjectResult objectResult)
                {
                    var responseType = objectResult.Value.GetType();
                    var genericType = responseType.GetGenericTypeDefinition();

                    if (genericType == typeof(ResponseModel<>))
                    {
                        // La réponse est déjà enveloppée dans ResponseModel<T>
                        return;
                    }

                    // Lever une exception si la réponse n'est pas enveloppée dans ResponseModel<T>
                    throw new InvalidOperationException($"La méthode {context.ActionDescriptor.DisplayName} ne retourne pas un type enveloppé dans ResponseModel<T>.");
                }
            }
        }


    }
}
