using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheckItOut.Payments.Application.CommandHandlers;
using CheckItOut.Payments.Application.QueryHandlers;
using CheckItOut.Payments.Domain;
using CheckItOut.Payments.Domain.Interfaces;
using CheckItOut.Payments.Domain.Interfaces.Repository;
using CheckItOut.Payments.Domain.Queries;
using CheckItOut.Payments.Infrastructure.Persistence.EntityFramework;
using CheckItOut.Payments.Infrastructure.Persistence.InMemory;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CheckItOut.Payments.Api
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
            services.AddControllers();
            
            services.AddDbContext<CheckItOutContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("CheckItOut"));
            });

            //comands
            services.AddTransient<IPaymentsCommandHandler, PaymentsCommandHandler>();

            //queries
            services.AddTransient<IQueryPayments, QueryPaymentHandler>();

            services.AddTransient<IPaymentRepository, PaymentRepository>();
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
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
