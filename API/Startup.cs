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
            services.AddControllers();

            // add dbcontext as service
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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
