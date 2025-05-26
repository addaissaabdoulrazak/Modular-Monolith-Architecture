using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NexaShopify.Core.Apps.Main.Handlers.Security;
using Swashbuckle.AspNetCore.SwaggerUI;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.SignalR;
using NexaShopify.Core.Identity.Handlers.User;

namespace NexaShopify.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            
            var builder = WebApplication.CreateBuilder(args);
            TokensManager.Initialize(builder.Configuration); //  Adda : 2025 -- come back after and review | organize this part of code 
            GetHandler.InitializeExtension(builder.Configuration); // Adda Issa 2025 : refactor  this piece of code too
            var securityKey = TokensManager.GetSecurityKey();
            GetConnectionStringLocal(builder.Configuration);
            //
            var  NexaCustomAllowOrigin = "NexaCustomAllowOrigin";

            // Add services to the container.

            #region>> Configuration JWT boilerplate code (commented)

            //var tokenSecret = builder.Configuration.GetValue<string>("JwtSettings:TokenSecret")
            //    ?? throw new InvalidOperationException("TokenSecret not configured");
            //var securityKey = new SymmetricSecurityKey(Convert.FromBase64String(tokenSecret));

            #endregion


            builder.Services.AddAuthentication(options =>
            {
                // Identity made Cookie authentication the default.
                // However, we want JWT Bearer Auth to be the default.
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
              {
                  options.TokenValidationParameters = new TokenValidationParameters
                  {
                      ValidateIssuerSigningKey = true,
                      IssuerSigningKey = securityKey,
                      ValidateIssuer = true,
                      ValidIssuer = "Issuer",
                      ValidateAudience = true,
                      ValidAudience = "Audience",
                      ValidateLifetime = true,
                      RequireExpirationTime = false,
                      //ClockSkew = TimeSpan.Zero
                  };
              });
            ;

            // builder.Services.AddControllers(); => replaced by (below method) for forcing dev to use ResponseModel
            builder.Services.AddControllers(options =>
            {
                //options.Filters.Add<ResponseModelActionFilter>();

            });

            //CORS (Cross-Origin Resource Sharing)
            builder.Services.AddCors(
            options =>
            {
                options.AddPolicy(NexaCustomAllowOrigin,
                    builder => builder.AllowAnyOrigin()
                                       .AllowAnyMethod()
                                       .AllowAnyHeader()
                                       .SetPreflightMaxAge(TimeSpan.FromHours(2))
                );
            });

            #region>> JSON serialization configuration in .NET 3.0
            // JSON serialization configuration | on controllers because that's apply on AddControllers() 
            // http requests are returned by controller so...
            //builder.Services.AddControllers()
            //    .AddNewtonsoftJson(options =>
            //    {

            //        // Use the default property (Pascal) casing
            //        options.SerializerSettings.ContractResolver = new DefaultContractResolver();

            //    });

            #endregion

            // JSON serialization configuration
            // keeping the PascalCase convention to avoid front-end inconsistencies.
            // standardiser la sortie de l'API
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                // Pascal casing
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            #region>> Comment Swagger SchemaFilter 
            //builder.Services.AddSwaggerGen();
            //builder.Services.AddSwaggerGen(c =>
            //{
            //    c.SchemaFilter<ResponseModelSchemaFilter>();  // Adding the schema filter for ResponseModel
            //});

            // Configuration Swagger avec support JWT
            #endregion

            builder.Services.AddSwaggerGen(c =>
            {
                #region
                // c.SchemaFilter<ResponseModelSchemaFilter>();
                //c.DocumentFilter<EnforceResponseModelDocumentFilter>();
                #endregion

                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Nexa Shop API", Version = "v1" });

                // Ajout du support JWT dans Swagger
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer"
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
                c.CustomSchemaIds(x => x.FullName);
                c.EnableAnnotations();

                // Enable XML comments (if necessary)
                //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //c.IncludeXmlComments(xmlPath);
            });


            #region>> Midlleware pipeline

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(e =>
                {
                    e.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
                    e.DocExpansion(DocExpansion.None);
                    //e.RoutePrefix = string.Empty; // -- adda 2025 : it sucks
                    //e.DocumentTitle = "Documentation de Mon API";

                });

                // Automatically open Swagger after startup
                //Task.Delay(3000).ContinueWith(_ => 
                //{
                //    var url = "http://localhost:5050/swagger";
                //    Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
                //});


            }
            app.UseStaticFiles();


            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(NexaCustomAllowOrigin);

            // IMPORTANT: Authentication before Authorization
            app.UseAuthentication();
            app.UseAuthorization();
      

            app.MapControllers();
            app.MapControllerRoute(
            name: "area",
            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
             name: "default",
             pattern: "api/{controller}/{action}/{id?}"
             );

            app.Run();


            #endregion

        }


        #region Helpers

        private static string GetConnectionString(IConfiguration configuration)
            => configuration.GetConnectionString("ConnectionString");

        private static string GetConnectionStringLocal(IConfiguration configuration)
            => configuration.GetConnectionString("DefaultConnection");

        private static string GetTokenSecret(IConfiguration configuration)
            => configuration.GetValue<string>("TokenSecret");

        private static string GetTempFolderPath()
        {
            var tempFolderPath = Path.Combine(Path.GetTempPath());
            if (!Directory.Exists(tempFolderPath))
                Directory.CreateDirectory(tempFolderPath);

            return tempFolderPath;
        }

        #endregion


    }
}
