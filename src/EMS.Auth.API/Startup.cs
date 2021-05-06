using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using EMS.Auth.API.DAL;
using EMS.Auth.API.DAL.Repositories;
using EMS.Auth.API.Enums;
using EMS.Auth.API.Interfaces;
using EMS.Auth.API.Models;
using EMS.Auth.API.Services;
using EMS.Common.Logger;
using EMS.Common.Utils.DateTimeUtil;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace EMS.Auth.API
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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "EMS.Auth.API", Version = "v1" });
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

            string connectionString = Environment.GetEnvironmentVariable("DbConnectionString");
            if (string.IsNullOrEmpty(connectionString))
            {
                // for manual db migration creating
                connectionString = Configuration.GetConnectionString("DbConnectionString");
            }
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

            services.AddSingleton(typeof(IEMSLogger<>), typeof(EMSLogger<>));
            services.AddSingleton<JwtSecurityTokenHandler>();
            services.AddTransient<IDateTimeUtil, Common.Utils.DateTimeUtil.DateTimeUtil>();
            services.AddTransient<IApplicationDbContext, ApplicationDbContext>();
            services.AddTransient<IUsersRepository, UsersRepository>();
            services.AddTransient<ITokenRepository, TokenRepository>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IUsersService, UsersService>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                   .AddJwtBearer(options =>
                   {
                       options.RequireHttpsMetadata = false;
                       options.TokenValidationParameters = new TokenValidationParameters
                       {
                            // укзывает, будет ли валидироваться издатель при валидации токена
                            ValidateIssuer = true,
                            // строка, представляющая издателя
                            ValidIssuer = AuthOptions.Issuer,

                            // будет ли валидироваться потребитель токена
                            ValidateAudience = true,
                            // установка потребителя токена
                            ValidAudience = AuthOptions.Audience,
                            // будет ли валидироваться время существования
                            ValidateLifetime = true,

                            // установка ключа безопасности
                            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(TokenType.Access),
                            // валидация ключа безопасности
                            ValidateIssuerSigningKey = true,
                       };
                   });

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EMS.Auth.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            using IServiceScope serviceScope = app.ApplicationServices
               .GetRequiredService<IServiceScopeFactory>()
               .CreateScope();

            DbContext context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
            context.Database.EnsureCreated();
        }
    }
}
