using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using TegBotTrmd.Context;
using WebTrmdBot.AbsCommands;
using WebTrmdBot.CommandServices;
using FluentValidation.AspNetCore;
using TegBotTrmd.IRepository;
using TegBotTrmd.Repository;

namespace WebTrmdBot
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
            services.AddDbContext<ShopContext>(options =>
            {
                options
                    .UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services
                .AddScoped<ICommandService, CommandService>()
                //.AddScoped<IDbRepository, DbRepository>()
                .AddTelegramBotClient(Configuration)
                .AddTransient<IOrderRepository, OrderRepository>()
                .AddTransient<IProductRepository, ProductRepository>()
                .AddTransient<IUserRepository, UserRepository>()
                .AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                })
                .AddFluentValidation();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
