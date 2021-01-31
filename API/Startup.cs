using API.Extensions;
using API.Helpers;
using API.Middleware;
using AutoMapper;
using Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
         
            
            // Registering automapper as service.
            services.AddAutoMapper(typeof(MappingProfiles)); // mapping profiles is class where we have provided mappings.
            services.AddControllers();

            // add dbcontext as service
            // ordering dosent matter here. ordering matter in middleware section
            // since we are using sqllite for development purpose so we are using sqllite.
            services.AddDbContext<StoreContext>(x => 
            x.UseSqlite(_config.GetConnectionString("DefaultConnection")));

           services.AddApplicationServices(); // coming from extension method.
           services.AddSwaggerDocumentation(); // coming from swagger extension

           // adding CORS
           services.AddCors(opt => {
               opt.AddPolicy("CorsPolicy", policy => {
                   policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200");
               });
           });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // Places to add middleware..
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // if (env.IsDevelopment())
            // {
            //     app.UseDeveloperExceptionPage();
            // }
            // Using custom middleware.
            app.UseMiddleware<ExceptionMiddleware>();

            app.UseStatusCodePagesWithReExecute("/errors/{0}"); // When no endpoint match it will hit this middleware which will redirect to route /errors.

            app.UseHttpsRedirection(); // this redirect http request to https.

            app.UseRouting(); // Enable to use us routing.

            app.UseStaticFiles(); // Enabling server to serve ststic file.

            app.UseCors("CorsPolicy");

            app.UseAuthorization();

            app.UseSwaggerDocumentation(); // from extension method.


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
