using CheckItOut.Payments.Application.CommandHandlers;
using CheckItOut.Payments.Application.QueryHandlers;
using CheckItOut.Payments.Domain.BankSim;
using CheckItOut.Payments.Domain.Interfaces;
using CheckItOut.Payments.Domain.Interfaces.Repository;
using CheckItOut.Payments.Domain.Queries;
using CheckItOut.Payments.Infrastructure.BankSim;
using CheckItOut.Payments.Infrastructure.Persistence.EntityFramework;
using CheckItOut.Payments.Infrastructure.Persistence.InMemory;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
            
            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = "https://localhost:5001";
                    options.RequireHttpsMetadata = false;

                    options.Audience = "CheckoutApi";
                });

            services.AddDbContext<CheckItOutContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("CheckItOut"));
            });

            //comands
            services.AddTransient<IPaymentsCommandHandler, PaymentsCommandHandler>();


            //queries
            services.AddTransient<IQueryPayments, QueryPaymentHandler>();
            services.AddTransient<IQueryMerchants, QueryMerchantsHandler>();

            //repositories
            services.AddTransient<IMerchantRepository, MerchantRepository>();
            services.AddTransient<IPaymentRepository, PaymentRepository>();



            //external
            services.AddTransient<IChargeCard, ChargeCard>();

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

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
