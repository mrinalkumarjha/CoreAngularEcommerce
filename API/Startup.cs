using API.Helpers;
using AutoMapper;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace API
{
    public class Startup
    {
        private readonly IConfiguration _config;
        public Startup(IConfiguration config)
        {
            _config = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // Di container 
        public void ConfigureServices(IServiceCollection services)
        {
            // we are registering ProductRepository as service so that we can get it inside controller.
            services.AddScoped<IProductRepository, ProductRepository>();
            // registering generic repository.
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            
            // Registering automapper as service.
            services.AddAutoMapper(typeof(MappingProfiles)); // mapping profiles is class where we have provided mappings.
            services.AddControllers();

            // add dbcontext as service
            // ordering dosent matter here. ordering matter in middleware section
            // since we are using sqllite for development purpose so we are using sqllite.
            services.AddDbContext<StoreContext>(x => 
            x.UseSqlite(_config.GetConnectionString("DefaultConnection")));


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // Places to add middleware..
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection(); // this redirect http request to https.

            app.UseRouting(); // Enable to use us routing.

            app.UseStaticFiles(); // Enabling server to serve ststic file.

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
