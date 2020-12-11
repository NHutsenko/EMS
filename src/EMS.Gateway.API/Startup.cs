using EMS.Gateway.API.DAL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics.CodeAnalysis;

namespace EMS.Gateway.API
{
	[ExcludeFromCodeCoverage]
	public class Startup
	{
		private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
		{
			string connectionString = Environment.GetEnvironmentVariable("DbConnectionString");

			if (string.IsNullOrEmpty(connectionString))
			{
				// for manual db migration creating
				connectionString = Configuration.GetConnectionString("DbConnectionString");
			}
			services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
			services.AddGrpc();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapGrpcService<GreeterService>();

				endpoints.MapGet("/", async context =>
				{
					await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
				});
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
