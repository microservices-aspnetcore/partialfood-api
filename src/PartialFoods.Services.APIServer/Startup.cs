using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PartialFoods.Services;

namespace PartialFoods.Services.APIServer
{
    public class Startup
    {
        private ILogger logger;

        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            Configuration = configuration;
            this.logger = logger;
        }

        public IConfiguration Configuration { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            Console.WriteLine("CONFIGURE SERVICES CALLED");
            // TODO: make these channels DNS-disco-friendly
            Channel cmdChannel = new Channel(Configuration["rpcclient:ordercommand"], ChannelCredentials.Insecure);
            var client = new OrderCommand.OrderCommandClient(cmdChannel);

            Channel invChannel = new Channel(Configuration["rpcclient:inventory"], ChannelCredentials.Insecure);
            var invClient = new InventoryManagement.InventoryManagementClient(invChannel);

            Channel orderChannel = new Channel(Configuration["rpcclient:ordermanagement"], ChannelCredentials.Insecure);
            var orderClient = new OrderManagement.OrderManagementClient(orderChannel);

            logger.LogInformation($"Order Command Client: {cmdChannel.ResolvedTarget}");
            logger.LogInformation($"Order Management Client: {orderChannel.ResolvedTarget}");
            logger.LogInformation($"Inventory Client: {invChannel.ResolvedTarget}");

            services.AddSingleton(typeof(OrderCommand.OrderCommandClient), client);
            services.AddSingleton(typeof(InventoryManagement.InventoryManagementClient), invClient);
            services.AddSingleton(typeof(OrderManagement.OrderManagementClient), orderClient);

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
