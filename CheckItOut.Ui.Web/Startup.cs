using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheckItOut.Payments.Application.CommandHandlers;
using CheckItOut.Payments.Application.QueryHandlers;
using CheckItOut.Payments.Domain.BankSim;
using CheckItOut.Payments.Domain.Interfaces;
using CheckItOut.Payments.Domain.Interfaces.Repository;
using CheckItOut.Payments.Domain.MerchantContracts;
using CheckItOut.Payments.Domain.Queries;
using CheckItOut.Payments.Infrastructure.BankSim;
using CheckItOut.Payments.Infrastructure.Merchant;
using CheckItOut.Payments.Infrastructure.Persistence.InMemory;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CheckItOut.Ui.Web
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
            services.AddControllersWithViews();

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
            services.AddTransient<INotifyMerchantPaymentSucceeded, NotifyMerchantPaymentSucceeded>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
