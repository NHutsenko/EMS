using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using EMS.Common.Protos;
using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using EMS.Auth.API.Models;
using EMS.Auth.API.Enums;
using System.Collections.Generic;

namespace EMS.Gateway.API
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "EMS.Gateway.API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer" }
                        }, new List<string>() }
                });
            });

            InjectCoreGrpcClients(services);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                   .AddJwtBearer(options =>
                   {
                       options.RequireHttpsMetadata = false;
                       options.TokenValidationParameters = new TokenValidationParameters
                       {
                           // token issuer validation required
                           ValidateIssuer = true,
                           // token issuer
                           ValidIssuer = AuthOptions.Issuer,

                           // audience validation required
                           ValidateAudience = true,
                           // set aoudience name
                           ValidAudience = AuthOptions.Audience,
                           // set token expiring
                           ValidateLifetime = true,

                           // security key
                           IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(TokenType.Access),
                           // is security key validation needed
                           ValidateIssuerSigningKey = true,
                       };
                   });

            services.AddControllers()
                .AddNewtonsoftJson()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.IgnoreNullValues = true;
                });
        }

        private static void InjectCoreGrpcClients(IServiceCollection services)
        {
            string coreApiUrl = Environment.GetEnvironmentVariable("CoreApiUrl");
            services.AddGrpcClient<DayOffs.DayOffsClient>(o =>
            {
                o.Address = new Uri(coreApiUrl);
            });
            services.AddGrpcClient<Holidays.HolidaysClient>(o =>
            {
                o.Address = new Uri(coreApiUrl);
            });
            services.AddGrpcClient<MotivationModificators.MotivationModificatorsClient>(o =>
            {
                o.Address = new Uri(coreApiUrl);
            });
            services.AddGrpcClient<OtherPayments.OtherPaymentsClient>(o =>
            {
                o.Address = new Uri(coreApiUrl);
            });
            services.AddGrpcClient<People.PeopleClient>(o =>
            {
                o.Address = new Uri(coreApiUrl);
            });
            services.AddGrpcClient<Positions.PositionsClient>(o =>
            {
                o.Address = new Uri(coreApiUrl);
            });
            services.AddGrpcClient<Salary.SalaryClient>(o =>
            {
                o.Address = new Uri(coreApiUrl);
            });
            services.AddGrpcClient<Staffs.StaffsClient>(o =>
            {
                o.Address = new Uri(coreApiUrl);
            });
            services.AddGrpcClient<Teams.TeamsClient>(o =>
            {
                o.Address = new Uri(coreApiUrl);
            });
            services.AddGrpcClient<RoadMaps.RoadMapsClient>(o =>
            {
                o.Address = new Uri(coreApiUrl);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "EMS.Gateway.API v1");
                    c.RoutePrefix = string.Empty;
                });
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
