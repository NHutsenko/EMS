using EMS.Common.Logger;
using EMS.Common.Utils.DateTimeUtil;
using EMS.Core.API.DAL;
using EMS.Core.API.DAL.Repositories;
using EMS.Core.API.DAL.Repositories.Interfaces;
using EMS.Core.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics.CodeAnalysis;

namespace EMS.Core.API
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

            services.AddAuthorization();

            string connectionString = Environment.GetEnvironmentVariable("DbConnectionString");
            if (string.IsNullOrEmpty(connectionString))
            {
                // for manual db migration creating
                connectionString = Configuration.GetConnectionString("DbConnectionString");
            }
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

            services.AddSingleton(typeof(IEMSLogger<>), typeof(EMSLogger<>));
            services.AddSingleton<IDateTimeUtil, DateTimeUtil>();
            services.AddTransient<IApplicationDbContext, ApplicationDbContext>();
            services.AddTransient<IMotivationModificatorRepository, MotivationModificatorRepository>();
            services.AddTransient<IDayOffRepository, DayOffRepository>();
            services.AddTransient<IOtherPaymentsRepository, OtherPaymentsRepository>();
            services.AddTransient<IPeopleRepository, PeopleRepository>();
            services.AddTransient<IPositionsRepository, PositionsRepository>();
            services.AddTransient<IStaffRepository, StaffRepository>();
            services.AddTransient<ITeamsRepository, TeamsRepository>();

            services.AddCors(c => c.AddDefaultPolicy(builder =>
            {
                builder.WithOrigins(Environment.GetEnvironmentVariable("GatewayApiUrl"))
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));

            services.AddCors(c => c.AddPolicy("GatewayCorsPolicy", builder =>
            {
                builder.WithOrigins(Environment.GetEnvironmentVariable("GatewayApiUrl"))
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));

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

            app.UseCors();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<DayOffsService>()
                    .RequireCors("GatewayCorsPolicy");
                endpoints.MapGrpcService<HolidaysService>()
                    .RequireCors("GatewayCorsPolicy");
                endpoints.MapGrpcService<MotivationModificatorsService>()
                    .RequireCors("GatewayCorsPolicy");
                endpoints.MapGrpcService<OtherPaymentsService>()
                    .RequireCors("GatewayCorsPolicy");
                endpoints.MapGrpcService<PeopleService>()
                    .RequireCors("GatewayCorsPolicy");
                endpoints.MapGrpcService<PositionsService>()
                    .RequireCors("GatewayCorsPolicy");
                endpoints.MapGrpcService<SalaryService>()
                    .RequireCors("GatewayCorsPolicy");
                endpoints.MapGrpcService<StaffService>()
                    .RequireCors("GatewayCorsPolicy");
                endpoints.MapGrpcService<TeamsService>()
                    .RequireCors("GatewayCorsPolicy");

                endpoints.MapGet("/alive", async context =>
                {
                    await context.Response.WriteAsync("Core API is alive");
                });
            });

            using IServiceScope serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope();

            DbContext context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
            context.Database.EnsureCreated();
        }
    }
}
