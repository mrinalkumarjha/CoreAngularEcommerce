# CoreAngularEcommerce
ecommerce application build on dotnet core and angular

# Extension needed to add for vs code
c sharp
c sharp extension
material icon theme
nuget-package-manager
SQLite


dotnet sln add API -- to add api project to sln

in vs code type shift + ctrl + p and type Generate asset for build and debug and press enter. 
this will create .vscode folder along with launch.json. which is useful in development.

dotnet watch run : this will watch changes and restart server.


# ADD ENTITYFRAMEWORK PACKAGE

1 add following package from nuget package
	Microsoft.EntityFrameworkCore    : varsion 3.1.1
	Microsoft.EntityFrameworkCore.SqLite    : version 3.1.1
	


# Add migration
install dotnet-ef tool for addming migration. version should match sdk version. 
use command dotnet --info to get sdk detail

for vs code
package: dotnet tool install --global dotnet-ef --version 3.1.11

Command: dotnet ef migrations add MyFirstMigration -o Data/Migrations

for vs
package:  Microsoft.EntityFrameworkCore.Tools

Command: add-migration InitialMigration

# Create Database

dotnet ef database update

# Adding classlibrary
> dotnet new classlib -o Core -- create new class lib project. core will contain domain
> dotnet new classlib -o Infrastructure -- new class lib

# PROJECT DEPENDENCIES (Architecture)

API ==> Infrastructure ==> Core

> Add infrastructure reference in api
dotnet add reference ..//infrastructure

> Add core reference in infrastructure
dotnet add reference ..//core

# Add project to version control GIT

git init  > to add project to git

comit to local repo

create public repo in github

add changes to remote git repo > git remote add origin https://github.com/mrinalkumarjha/CoreAngularEcommerce.git

push changes to git > git push -u origin master

# Repository pattern
	Decouple business code from data access
	seperation of concern
	minimise duplicate query logic
	Currently we are injecting storecontext into controller. after repository we will use repository
	suppose we have to get product details in 5 controller then we have to use same query in all 5 controller using
	storedbContext.
	also testability become easy

	Repository will sit between controller and Dbcontext
	Increased level of abstraction.
	Increased maintability, flexibility, testability.
	By this we have more classes and interfaces but less duplicate code.
	Now instead of injecting storedbcontext in controller we will inject repository.

# dropping existing db and create new migration
	remove existing databse : dotnet ef database drop -p Infrastructure -s API
	-s is for startup project
	-p is for project file where store context available.

# Remove Existing Migration
	dotnet ef Migrations remove -p Infrastructure -s API

# Add new migration
	dotnet ef Migrations Add InitialCreate  -p Infrastructure -s API -o Data/Migrations
	-o is for output directory
	
#  Migration code to run migration at runtime. this will update new migration as well as create db if not exists.

	 using(var scope = host.Services.CreateScope())
           {
               var services = scope.ServiceProvider;
               var loggerFactory = services.GetRequiredService<ILoggerFactory>();
               try
               {
                   var context = services.GetRequiredService<StoreContext>();
                   await context.Database.MigrateAsync(); // this will apply any pending migration if pending and create db if not exists.

               }
               catch(Exception ex)
               {
                   var logger = loggerFactory.CreateLogger<Program>();
                   logger.LogError(ex, "An error occured on migration");
               }
           }

# Seeding to db.
	StoreContextSeed class is used for seeding data.

# Generic repository pattern.
	IGenericRepository is repository pattern which abstract db from controller.

# Specification pattern : this pattern gives generic repositery more flexibility over querying. like querying entity with 	where clause.
	 need to study more on this
	ISpecification is specification part implementation
	SpecificationEvaluator is where we build query.

# Debug api. in vs code.
	press ctrl + shift + D to select debug window. select .net core attach from top left dropdown and click on start debugging button. then you will get a prompt to select process to debug. search project name like i have given API so search API . select the process. add debugger at endpoint and hit endpoint.

# Shaping data with DTO(data transfer object).
	dtos are only used for simple data transfer it is not any business object. we keep this inside API folder.
	We will use Automapper to map Entity to dto automatically

	Step to integrate Automapper in API project
	install Automapper.Extensions.Microsoft in api proj

	Create following mapping profile.
	public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDto>();
        }
    }

	Register Automapper in startup as service..
	services.AddAutoMapper(typeof(MappingProfiles));

	Inject it inside controller and use it like below.
	return _mapper.Map<Product, ProductToReturnDto>(product); 

# Serving ststic content in webapi.
	add following command inside stsrtup middleware function.
	app.UseStaticFiles(); 

# Some HTTP code and their meaning.
	200 range   => 	OK
	300 range   => REDIRECTION
	400 range	=> CLIENT ERROR
	500 range 	=> SERVER ERROR

# How to redirect to custom route if no endpoint found in web api request.
   use following method inside startup class middleware section.(configure method)
   app.UseStatusCodePagesWithReExecute("/errors/{0}");

# How to add swagger in api project

  we need to add 2 package for swagger
  search Swashbuckle.AspnetCore
  install Swashbuckle.AspnetCore.SwaggarGen   version 5.0
  and Swashbuckle.AspnetCore.SwaggarUI

  Click restore.

  > Register swagger as service.


# How to enable CORS.
	     // adding CORS inside service
           services.AddCors(opt => {
               opt.AddPolicy("CorsPolicy", policy => {
                   policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200");
               });
           });


		// in middleware
		app.UseCors("CorsPolicy");


		After this your each response will come with header "Access-Control-Allow-Origin" and value will be client application url.
		THis has value of url which we have allowed fron api.

# Redis for caching in api
	add StackExchange.redis in infrastructure project
	varsion 2.0.6

	DOwnload redeis from https://redis.io/download

	redis is for linux mainly so to use redis in wondow use chocklaty to install in window.
	If you dont want to install any tool in window use any free online solution.

	To use online redis server use gitlab
	endpoint: redis-17032.c251.east-us-mz.azure.cloud.redislabs.com:17032
	pass: A48HG0ea0GjfSzWGJO0uuNSrkRYnkhOu

	https://app.redislabs.com/
	testarcs1@gmail.com
	Mrinal"1234

	To view data install RedisInsight 

# Identity
	ASP.net identity is used to add users and manage authentication and authorization.
	will use UserManager and SignInManager service provided by identity.
	UserManager to query user from idsentity db
	SignInManager is used for signin.

# Setting Identity 
	Install following package insode Infrastructure proj.
	1> Microsoft.AspNetCore.Identity   Varsion:2
	2> Microsoft.AspNetCore.Identity.EntityFrameworkCore  Version 3.1.11  .. this version should same as runtime
	3> Microsoft.IdentityModel.Tokens   version 5.6.0
	4> System.IdentityModel.Tokens.Jwt   version 5.6.0

	Install following package insode API proj.
	1> Microsoft.AspNetCore.Authentication.JwtBearer    Version: 3.1.11 we install this in api proj as api will manage authenticationm.

	Install following package insode Core proj.
	1>Microsoft.Extensions.Identity.Stores      		Version : 3.1.11

# Adding migration for identity context inside seperate folder.
	dotnet ef migrations add IdentityInitial -p Infrastructure -s API -o Identity/Migrations -c AppIdentityDbContext

	-s is startup project
	-c is context name
	-o is output directory
	-p is project name in which migration need to be created.

	To remove migration
	dotnet ef migrations remove  -p Infrastructure -s API  -c AppIdentityDbContext

# Adding JWT token
	
 Added IdentityServiceExtensions


# API Validation
	We will use data annotation to validate model.
	We should not rely on client for validation. so we should apply validation on server side.
	We can add validation to database entity also but adding validation on entity will add extra responsilibity to entity class.
	So we will add validation attributes on Dto level. DTO is POCO class so its ok to add validation responsibility here.

	we will use https://regexlib.com/ for creating regular expression for validation

# Update swagger for identity
  we can now use authorize button on top right corner and provice token to authorize. swagger will use same token in rest endpoint.


# Add Order migration
	dotnet ef migrations add OrderEntityAdded -p Infrastructure -s API  -c StoreContext

	once api project start migration will apply as we added code.

# drop db
	dotnet ef  database drop -p Infrastructure -s API  -c StoreContext

# Unit of work pattern
	since with generic repository we have seperate instance of dbContext at run time for each repo.
	which could lead to partial update of entity. like while saving one entity could succeed and one
	could failed.

	also we have to inject multiple repo in controller like product repo, order repo as generic repo.

	So unit of work is designed to work into this problem.
	uow will create dbcontext instance itself, and uow will be responsible for creating instance
	of repository.
	so in case of uow each of repository wont be creating each instance of dbContext.
	uow wil maintain transaction. in case of partial update all transaction will be rolled back.

# Setting stripe payment
	got to stripe.com and signup
	id:testarcs1@gmail.com
	pass: m...1234

	publishable_key is used in client
	secret key is used in server.

	add nuget package stripe.net 34.19 to infrastructure project.

	test card:
	4242 4242 4242 4242












# ANGULAR (enable https in client project as api cors support only https).
	For this we need some signed certificate. install .cert file inside trusted root authority

	change following inside angular.json

	"serve": {
          "builder": "@angular-devkit/build-angular:dev-server",
          "options": {
            "browserTarget": "client:build",
            "sslKey": "ssl/server.key",
            "sslCert": "ssl/server.crt",
            "ssl": true
          },


# Add bootstrap in angular
	ngx-bootstrap gives functionality of using bootstrap using angular component insted of jquery.
	https://valor-software.com/ngx-bootstrap/#/ is official website of ngx bootstrsp.

	install using commad : ng add ngx-bootstrap 

# Add font-awsome in angular for creating icon
	npm install font-awesome

# Some useful angular extension.
	> angular language service  : it gives us autocomplete intelisence
	> angular snippet by john papa : for fast coding.
	> Prettier : help us to format code
	> Bracket Pair Colorizer 2 : help us to identify start end bracket as it colour it.
	> Tslint : gives linting support.


# Some useful command in angular.
ng g c nav-bar --skip-tests : create component without test file.
ng g m core  : to create module
ng g c shop --flat --skip-tests : create comp without folder without test.
ng g s shop --flat --skip-tests : create service

# Use JSON to Ts online tool for comberting json object to ts

# Structure of angular app

	App Module		Core module
					Shared module
					Feature Module

# angular service:
  as we inject httpclient into appcomponent contructure , it is not best practice to do so. so we do this via services.

  services are decorated with @injectable 

  @Injectable({
  providedIn: 'root'  // as appModule is our root module this service will be available to app module. we dont need to declare this inside provider array.
})
	# services are singalton and it will vaailable untill app is running. it wont be destroyed if we leave component.
	

# Get data from router
	// inject ActivatedRoute inside constructor
  	constructor(private shopService: ShopService, private activatedRoute: ActivatedRoute) { }
	// use following method to get id.
	this.activatedRoute.snapshot.paramMap.get('id')

# Httpinterceptor
  We use this error handling.
  create a httpinterceptor and add them inside app module provider.

    providers: [
    {provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi:true}
  ],

  # ngx-toaster : for notification in ui.
	npm install toastr

# breadcrumb 
	using xng breadcrumb package.
	npm install --save xng-breadcrumb

# Bootstratp theme change

  we use bootswatch for changing bootstrap theme. bootswatch.com
  npm install bootswatch

  to enable this add bootswatch css to angular.json . here sketchy is theme name
     "./node_modules/bootswatch/dist/sketchy/bootstrap.min.css",
	 "./node_modules/bootswatch/dist/united/bootstrap.min.css",  -- for united theme

# Ngx spinner for loader

 npm install ngx-spinner

# Behaviour subject:
	behaviour subject is one kind of observable which is used for multicasting. it is for multiple subscriber.

	async pipe used to subscribe and unsubscribe from observale in template itself rather then component.

# Package UUID 
	For generating unique identifier in angular use UUID npm package 
	npm install uuid

# Reactive form
	To use reactive form we need to import ReactiveFormsModule.

# async validation
	async validation we have used to validate if email exists in system. async validation is added in registration form.
	async validation will fire only once sync validation is passed.

# Auth Guard:
	To prevent unauthorized user in angular we will use authGuard.
	authguard is router feature which provide facility to prevent routes activation.

	syntax to create guard:

	ng g g auth --skip-tests

# angular cdk:
	we will use angular component development kit to add steper form
	ng add @angular/cdk