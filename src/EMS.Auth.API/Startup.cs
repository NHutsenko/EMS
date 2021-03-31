using System;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using EMS.Auth.API.DAL;
using EMS.Auth.API.DAL.Repositories;
using EMS.Auth.API.Interfaces;
using EMS.Auth.API.Services;
using EMS.Common.Logger;
using EMS.Common.Utils.DateTimeUtil;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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

			services.AddControllers();
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "EMS.Auth.API", Version = "v1" });
			});
            string connectionString = Environment.GetEnvironmentVariable("DbConnectionString");
            if (string.IsNullOrEmpty(connectionString))
            {
                // for manual db migration creating
                connectionString = Configuration.GetConnectionString("DbConnectionString");
            }
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

            services.AddSingleton(typeof(IEMSLogger<>), typeof(EMSLogger<>));
            services.AddSingleton<IDateTimeUtil, DateTimeUtil>();
            services.AddSingleton<JwtSecurityTokenHandler>();
            services.AddTransient<IApplicationDbContext, ApplicationDbContext>();
            services.AddTransient<IUsersRepository, UsersRepository>();
            services.AddTransient<ITokenRepository, TokenRepository>();
            services.AddTransient<IAuthService, AuthService>();
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
