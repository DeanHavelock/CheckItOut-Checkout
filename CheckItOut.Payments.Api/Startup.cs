using CheckItOut.Payments.Application.CommandHandlers;
using CheckItOut.Payments.Application.QueryHandlers;
using CheckItOut.Payments.Domain.BankSim;
using CheckItOut.Payments.Domain.Interfaces;
using CheckItOut.Payments.Domain.Interfaces.Repository;
using CheckItOut.Payments.Domain.MerchantContracts;
using CheckItOut.Payments.Domain.Queries;
using CheckItOut.Payments.Infrastructure.BankSim;
using CheckItOut.Payments.Infrastructure.Merchant;
using CheckItOut.Payments.Infrastructure.Persistence.EntityFramework;
using CheckItOut.Payments.Infrastructure.Persistence.InMemory;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

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
            //services.AddControllers();
            services.AddControllersWithViews();

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("checkitout", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "CheckItOut Payment API",
                    Description = "Check It Out, A Payment Api, Payments Come And Go!",
                });
            });

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
            services.AddTransient<IPrepairPaymentCommandHandler, PrepairPaymentCommandHandler>();
            services.AddTransient<IPaymentsCommandHandler, PaymentsCommandHandler>();


            //queries
            services.AddTransient<IQueryPayments, QueryPaymentHandler>();
            services.AddTransient<IQueryMerchants, QueryMerchantsHandler>();

            //repositories
            services.AddTransient<IMerchantRepository, MerchantRepository>();
            services.AddTransient<IPaymentRepository, PaymentRepository>();
            services.AddTransient<IPaymentRequestRepository, PaymentRequestRepository>();


            //external
            services.AddTransient<IChargeCard, ChargeCard>();
            services.AddTransient<INotifyMerchantPaymentSucceeded, NotifyMerchantPaymentSucceeded>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger(c =>
            {
                c.SerializeAsV2 = true;
            });

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/checkitout/swagger.json", "CheckItOut Payment API V1");
                //c.RoutePrefix = string.Empty;
            });

            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapDefaultControllerRoute();

            });

        }
    }
}
