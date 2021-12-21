using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BikeStoresApi.Models.Contracts;
using BikeStoresApi.Models.Repositories;
using System.IO;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using BikeStoresApi.Configuration.Logger;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Diagnostics;

namespace BikeStoresApi.Configuration
{
    public static class IoC
    {
        public static IServiceCollection RegisterServicesBikeStore(this IServiceCollection services, IConfiguration configuration)
        {
            RegisterDbContext(services, configuration);
            RegisterLoggerService(services);
            RegisterRepositoryBikeStore(services);
            RegisterServicesJwt(services, configuration);
            RegisterServicesSwagger(services, configuration);
           
            return services;
        }

        public static IApplicationBuilder RegisterApplicationBikeStore(this IApplicationBuilder app, IConfiguration configuration, ILoggerManager logger)
        {
            RegisterApplicationLogger(app, logger);
            RegisterApplicationSwagger(app, configuration);
           
            return app;
        }

        private static IServiceCollection RegisterServicesSwagger(IServiceCollection services, IConfiguration configuration)
        {
            string jsonRoute = configuration.GetSection("Swagger:JsonRoute").Value;
            string description = configuration.GetSection("Swagger:Description").Value; 
            string UIEndpoint = configuration.GetSection("Swagger:UIEndpoint").Value; 
            string version = configuration.GetSection("Swagger:Version").Value; 
            string xml = configuration.GetSection("Swagger:Xml").Value; 
            string title = configuration.GetSection("Swagger:Title").Value;

            var basepath = System.AppDomain.CurrentDomain.BaseDirectory;
            var xmlPath = Path.Combine(basepath, xml);
            services.AddSwaggerGen(options => {
                var apiinfo = new OpenApiInfo
                {
                    Title = title,
                    Version = version,
                    Description = description,
                };
                
                OpenApiSecurityScheme securityDefinition = new OpenApiSecurityScheme()
                {
                    Name = "Bearer",
                    BearerFormat = "JWT",
                    Scheme = "bearer",
                    Description = "Specify the authorization token.",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                };
                
                 
                OpenApiSecurityScheme securityScheme = new OpenApiSecurityScheme()
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };

                 OpenApiSecurityRequirement securityRequirements = new OpenApiSecurityRequirement()
                {
                    { securityScheme, new string[] { }},
                };
                 

                options.SwaggerDoc(version, apiinfo);
                options.IncludeXmlComments(xmlPath);
                options.AddSecurityDefinition("Bearer", securityDefinition);
                // Make sure swagger UI requires a Bearer token to be specified
                options.AddSecurityRequirement(securityRequirements);
            });
            return services;
        }

        private static IServiceCollection RegisterServicesJwt(IServiceCollection services, IConfiguration configuration)
        {
            string secret = configuration.GetSection("Jwt:Key").Value;

            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
           .AddJwtBearer(jwt => {
               var key = Encoding.ASCII.GetBytes(secret);

               jwt.SaveToken = true;
               jwt.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuerSigningKey = true, // this will validate the 3rd part of the jwt token using the secret that we added in the appsettings and verify we have generated the jwt token
                   IssuerSigningKey = new SymmetricSecurityKey(key), // Add the secret key to our Jwt encryption
                   ValidateIssuer = false,
                   ValidateAudience = false,
                   RequireExpirationTime = false,
                   ValidateLifetime = true
               };

               jwt.Events = new JwtBearerEvents
               {
                   OnAuthenticationFailed = context =>
                   {
                       if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                       {
                           context.Response.Headers.Add("Token-Expired", "true");
                       }
                       return Task.CompletedTask;
                   }
               };
           });

            return services;
        }

        private static IServiceCollection RegisterDbContext(IServiceCollection services, IConfiguration configuration)
        {
            //services.AddDbContext<BikeStoresContext>(options => options.UseSqlServer("Server=BikeStores.mssql.somee.com;Database=BikeStores;User Id=patoskhy3485;Password=Pa18Ro09Ag19Va85"));
            var stringConnection = configuration.GetConnectionString("BikeStores");
            var timeout = 6000;
            services.AddDbContext<BikeStoresContext>(options => options.UseSqlServer(stringConnection, sqlServerOptions => sqlServerOptions.CommandTimeout(timeout)));

            return services;
        }

        private static IServiceCollection RegisterRepositoryBikeStore(IServiceCollection services)
        {
            services.AddTransient<IBikeStoresContext, BikeStoresContext>();
            services.AddTransient<IBrand, BrandRepository>();
            services.AddTransient<ICategory, CategoryRepository>();
            services.AddTransient<ICustomer, CustomerRepository>();
            services.AddTransient<IOrderItem, OrderItemRepository>();
            services.AddTransient<IOrder, OrderRepository>();
            services.AddTransient<IProduct, ProductRepository>();
            services.AddTransient<IStaff, StaffRepository>();
            services.AddTransient<IStock, StockRepository>();
            services.AddTransient<IStore, StoreRepository>();
            services.AddTransient<IUserInfo, UserInfoRepository>();
            return services;
        }

        private static IApplicationBuilder RegisterApplicationSwagger(IApplicationBuilder app, IConfiguration configuration)
        {
            string jsonRoute = configuration.GetSection("Swagger:JsonRoute").Value;
            string description = configuration.GetSection("Swagger:Description").Value;
            string UIEndpoint = configuration.GetSection("Swagger:UIEndpoint").Value;
            string version = configuration.GetSection("Swagger:Version").Value;
            string xml = configuration.GetSection("Swagger:Xml").Value;
            string title = configuration.GetSection("Swagger:Title").Value;

            app.UseSwagger(option => {
                option.RouteTemplate = jsonRoute;
            });
            app.UseSwaggerUI(config => {
                config.SwaggerEndpoint(version + UIEndpoint, description);
                //config.RoutePrefix = String.Empty;
            });
            return app;
        }

        private static IServiceCollection RegisterLoggerService(IServiceCollection services)
        {
            return services.AddSingleton<ILoggerManager, LoggerManager>();
        }

        private static IApplicationBuilder RegisterApplicationLogger(IApplicationBuilder app, ILoggerManager logger)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        logger.LogError($"Something went wrong: {contextFeature.Error}");
                        var res = new ErrorDetails()
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = "Internal Server Error." + contextFeature.Error.Message
                        }.ToString();
                        await context.Response.WriteAsync(res);
                    }
                });
            });

            return app;
        }
    }
}
