using MediatR;
using MicroRabbit.Banging.Domain.CommandHandlers;
using MicroRabbit.Banging.Domain.Commands;
using MicroRabbit.Banging.Domain.Interfaces;
using MicroRabbit.Banking.Application.Interfaces;
using MicroRabbit.Banking.Application.Services;
using MicroRabbit.Banking.Data.Context;
using MicroRabbit.Banking.Data.Repository;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Infostructure.Bus;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroRabbit.Banking.Api
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
            services.AddDbContext<BankingDbContext>
                (options => options.UseSqlServer(Configuration.GetConnectionString("BankingDbCOnnection")));
            //RegisterServices(services);

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("BankingMicroservice", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = "Banking Microservice",
                    Version = "4",
                    Description = "Giorgi Okruadze Test Api",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    {
                        Email = "oqruadze1997@gmail.com",
                        Name = "Giorgi Okruadze",
                        Url = new Uri("https://www.facebook.com/gio.oqruadze")
                    }
                });
            });

            //services.AddMediatR(typeof(Startup));

            ////Domain
            //services.AddTransient<IRequestHandler<CreateTransferCommand, bool>, TransferCommandHandler>();

            //Domain Bus
            services.AddScoped<IEventBus, RabbitMQBus>();

            //Application Services
            services.AddScoped<IAccountService, AccountService>();

            //Data
            services.AddScoped<IAccountRepository, AccountRepository>();
            //services.AddScoped<BankingDbContext>();

            var assembly = AppDomain.CurrentDomain.Load("MicroRabbit.Banging.Domain");
            services.AddMediatR(assembly);

            services.AddControllers();
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

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/BankingMicroservice/swagger.json", "Banking Microservice");
                options.RoutePrefix = "";
            });

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
