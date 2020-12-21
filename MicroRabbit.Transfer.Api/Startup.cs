using MediatR;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Infostructure.Bus;
using MicroRabbit.Transfer.Application.Services;
using MicroRabbit.Transfer.Application.Services.Abstractions;
using MicroRabbit.Transfer.Data.Context;
using MicroRabbit.Transfer.Data.Repository;
using MicroRabbit.Transfer.Data.Repository.Abstraction;
using MicroRabbit.Transfer.Domain.EventHandlers;
using MicroRabbit.Transfer.Domain.Events;
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

namespace MicroRabbit.Transfer.Api
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

            services.AddDbContext<TransferDbContext>
                (options => options.UseSqlServer(Configuration.GetConnectionString("TransferDbConnection")));

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("TransferMicroservice", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = "Transfer Microservice",
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

            services.AddTransient<IEventBus, RabbitMQBus>();
            services.AddScoped<ITransferService, TransferService>();
            services.AddScoped<ITransferRepository, TransferRepository>();
            services.AddTransient<IEventHandler<TransferCreatedEvent>, TranferEventHandler>();

            var assembly = AppDomain.CurrentDomain.Load("MicroRabbit.Transfer.Domain");
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
                options.SwaggerEndpoint("/swagger/TransferMicroservice/swagger.json", "Transfer Microservice");
                options.RoutePrefix = "";
            });

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            ConfigureEventBus(app);
        }

        private void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.Subscribe<TransferCreatedEvent, TranferEventHandler>();
        }
    }
}
