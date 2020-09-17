using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMS.Gateway.API.DAL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Gateway.API
{
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
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "EMS.Gateway Rest Full API", Version = "v1" });
			});

			string connectionString = Environment.GetEnvironmentVariable("DbConnectionString");

			if (string.IsNullOrEmpty(connectionString))
			{
				// for manual db migration creating
				connectionString = Configuration.GetConnectionString("DbConnectionString");
			}
			services.AddEntityFrameworkSqlServer()
				.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));


			services.AddControllers();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "EMS.Gateway Rest Full API");
			});
			app.UseRouting();


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
			context.Database.EnsureDeleted();
			context.Database.EnsureCreated();
		}
	}
}
