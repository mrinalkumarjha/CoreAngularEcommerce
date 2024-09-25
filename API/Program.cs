using StackExchange.Redis;
using Core.Entities.Identity;



var builder = WebApplication.CreateBuilder(args);


// add services to container

     // Registering automapper as service.
        builder.Services.AddAutoMapper(typeof(MappingProfiles)); // mapping profiles is class where we have provided mappings.
        builder.Services.AddControllers();

        // add dbcontext as service
        // ordering dosent matter here. ordering matter in middleware section

        builder.Services.AddDbContext<StoreContext>(x => 
        x.UseSqlServer(builder.Configuration.GetConnectionString("StoreSqlServerConnection")));

        // add identiy db context as service
        builder.Services.AddDbContext<AppIdentityDbContext>(x => 
        x.UseSqlServer(builder.Configuration.GetConnectionString("IdentitySqlServerConnection")));

        // Adding redis connnection settiong

        builder.Services.AddSingleton<IConnectionMultiplexer>(c =>
        {
        var configuration = ConfigurationOptions.Parse(
        builder.Configuration.GetConnectionString("Redis"), true);
        configuration.Password =  builder.Configuration.GetConnectionString("RedisPassword");
        return ConnectionMultiplexer.Connect(configuration);
        });

        builder.Services.AddApplicationServices(); // coming from extension method.
        builder.Services.AddIdentityServices(builder.Configuration); // coming from extension
        builder.Services.AddSwaggerDocumentation(); // coming from swagger extension


        // adding CORS
        //    services.AddCors(opt => {
        //        opt.AddPolicy("CorsPolicy", policy => {
        //            policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200");
        //        });
        //    });

        builder.Services.AddCors(options =>
        {
        options.AddDefaultPolicy(
        builder =>
        {

        //you can configure your custom policy
        builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
        });
        });



        // Configure http request pipeline..
         // Using custom middleware.

            var app = builder.Build();
            app.UseMiddleware<ExceptionMiddleware>();

            app.UseStatusCodePagesWithReExecute("/errors/{0}"); // When no endpoint match it will hit this middleware which will redirect to route /errors.

           // app.UseHttpsRedirection(); // this redirect http request to https.

            app.UseRouting(); // Enable to use us routing.

            app.UseStaticFiles(); // Enabling server to serve ststic file. it serve file from wwwroot folder

            // following is to serve image from content folder
            app.UseStaticFiles(new StaticFileOptions {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "Content")
                ),
                RequestPath = "/content"
            });

            //app.UseCors("CorsPolicy");
             app.UseCors();

            app.UseAuthentication(); // must before UseAuthorization

            app.UseAuthorization();

            app.UseSwaggerDocumentation(); // from extension method.



            app.MapControllers();
            // following setting for angular as we are serving angular page from api
            app.MapFallbackToController("Index", "Fallback");
        


           // following code is for creating migration on startup and logging.
           using(var scope = app.Services.CreateScope())
           {
               var services = scope.ServiceProvider;
               var loggerFactory = services.GetRequiredService<ILoggerFactory>();
               try
               {
                   var context = services.GetRequiredService<StoreContext>();
                   await context.Database.MigrateAsync(); // this will apply any pending migration if pending and create db if not exists.

                   // seeding data
                   await StoreContextSeed.SeedAsync(context, loggerFactory);


                   // identity sees code
                   var userManager = services.GetRequiredService<UserManager<AppUser>>();
                   var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
                   var identityContext = services.GetRequiredService<AppIdentityDbContext>();
                   await identityContext.Database.MigrateAsync(); // here identity db will be created
                   await AppIdentityDbContextSeed.SeedUsersAsync(userManager, roleManager); // addind seed for user
               }
               catch(Exception ex)
               {
                   var logger = loggerFactory.CreateLogger<Program>();
                   logger.LogError(ex, "An error occured on migration");
               }
           }

           await app.RunAsync();



