# CoreAngularEcommerce
ecommerce application build on dotnet core and angular

# How to run sql version in local system
1: setup redis connection setting o updated one. i have used one of cloud redis provider.

	"Redis": "redis-15652.c282.east-us-mz.azure.cloud.redislabs.com:15652"

    "RedisPassword": "SuuTgkrC8ySYs2jzeNSAzI11GRJio9cZ"

2: setup sql connection for both store context and identity context in appsettings.Development.json

3: run dotnet watch run from api directory from terminal.(This will automatically create database in sql server if connection string is correct).

4: Run NPM INSTALL(if running first time) ,

		 NPM START from client directory to run angular proj.

5: login in angular app using following credential.

			id: mrinalkumarjha@ymail.com
			pass: Admin@123

6: For payment	use following test card detail. Use any future expiry and cvv.
	
	4242 4242 4242 4242

	Declined card
	4000 0000 0000 0002 

	Test card to check insufficient fund
	4000 0000 0000 9995

	High security card
	4000 0027 6000 3184	

7: If ssl error comes while doing payment:
	if ssl error comes use this link to resolve.https://stackoverflow.com/questions/59352651/angular-default-app-ng-serve-privacy-error-in-chrome-neterr-cert-authority-i

# Extension needed to add for vs code
c# by microsoft

c sharp extension  by JosKreativ

material icon theme by philip (optional)

nuget package manager by jmrog

SQLite by alexcvzz

nuget gallery  by pcislo

dotnet new sln : for adding new solution
dotnet new api -o API : for creating api proj

dotnet sln add API -- to add api project to sln

dotnet sln list -- to list project inside solution

in vs code type shift + ctrl + p and type Generate asset for build and debug and press enter. 
this will create .vscode folder along with launch.json. which is useful in development.

dotnet watch run : this will watch changes and restart server.

# Course git repo : https://github.com/TryCatchLearn/Skinet


# ADD ENTITYFRAMEWORK PACKAGE

1 add following package from nuget package
	Microsoft.EntityFrameworkCore    : varsion 3.1.1
	Microsoft.EntityFrameworkCore.SqLite    : version 3.1.1
	
	Microsoft.EntityFrameworkCore.Design : this package is required in startup project for migration


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

# one to one relationship in Core5
 by adding  public Horse Horse { get; set; } in samurai class ef automatically
 determine relationship between horse and samurai. this is one to one relationship.


public class Horse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SamuraiId { get; set; }

    }

	public class Samurai
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Quote> Quotes { get; set; } = new List<Quote>();
        public List<Battle> Battles { get; set; } = new List<Battle>();
        public Horse Horse { get; set; }
    }

# Ef core power tools visual studio extension.
	model visulization and migration ui for EF core. 
	open visual studio > extension > search ef core power tools > download it.
	Restart vs.

	now right click on project where dbcontext file available > select ef core power tools
	> Add Db context model diagram.


# many to many relationship in Core5

 if we creaate a link using list in property. ef automatically determine many to  many relation.
 see example below.

 public class Battle
    {
        public int BattleId { get; set; }
        public string Name { get; set; }
        public List<Samurai> Samurais { get; set; } = new List<Samurai>(); 
		// many to many rel . ef core create BattleSamurai table automatically as it is convention based. we have also option to override existing convention.

    }

	public class Samurai
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Quote> Quotes { get; set; } = new List<Quote>();
        public List<Battle> Battles { get; set; } = new List<Battle>();
    }

# many to many relationship with additional column in Core5
	to achieve this we create seperate class and configure it in dbcontext onModelcreating.

	  class SamuraiBattle
    {
        public int SamuraiId { get; set; }
        public int BattleId { get; set; }
        public DateTime DateJoined { get; set; }

    }

	configure it 
	protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Samurai>()
                .HasMany(s => s.Battles)
                .WithMany(b => b.Samurais)
                .UsingEntity<SamuraiBattle>
                (bs => bs.HasOne<Battle>().WithMany(),
                bs => bs.HasOne<Samurai>().WithMany())
                .Property(bs => bs.DateJoined)
                .HasDefaultValueSql("getdate()");

        }

#  tagging sql in ef
by tagging we can easily identify our sql in sql profiler.

	EFCore5.Data.AppContext _context = new EFCore5.Data.AppContext();
	_context.Samurais.Add(new Samurai { Name = "mrinal" });
	_context.Samurais.TagWith("Add Samurai method");
	_context.SaveChanges();

# use find()
	use .find(1) method of linq if willing to find by id.

# use AsNoTracking to prevent traking option of entity framework.

	use AsNoTracking to prevent traking option of entity framework. when we dont use AsNoTracking
	ef cache it within scope of dbcontext object for SaveChanges(). so if we dont want to save object after getting list we can use AsNoTracking to prevent tracking.
	
	var blogs = context.Blogs
		.AsNoTracking()
		.ToList();

# Add, Update, Attach in ef core.
	to update record in disconnected scenario use attach.

# projection in ef.
	projection is done using select method of linq. by projection we can return selected property
	of object.

	var someProp = context.samurais.select(s => new {s.id, s.name}).tolist();

	or casting list to defined type.
	var someProp = context.samurais.select(s => new IdAndName(s.id, s.name)).tolist();

# Method to load related data in ef.
	Eager loading: use include to get related object. EFcore 5 can use filtered include.

	_context.samurais.include(s=>s.quote).tolist();
	
	Ecplicit loading: this is something like when we have some data in memory and want to load related data. following are way to retrive related data for objects already in memory.

	var samurai = _context.samurai.find(1);
	_context.entry(samurais).collection(s=>s.quote).load();
	or
	_context.entry(samurais).reference(s=>s.horse).load();

	Lazy loading:  Loading data on fly

# working with sp in code first.

	1: create a sp.
	2: create empty migration.
	3: add sp inside migration UP method.
	    migrationbuilder.sql(@"Create proc .....");
	4. call it
		var samurais = -context.samurais.FromSqlRaw("EXEC dbo.spname {0}", "paramss").tolist();
	or
	var samurais = -context.samurais.FromSqlInterpolated
	($"EXEC dbo.spname {paramss}").tolist();

# Executing raw sql or sp

	-context.database.executesqlRaw("EXEC spname {0}", params);
	
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

# Serving static content in webapi.
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

	account id acct_1IMrBOJzRQfscld3

# stripe listen : use this command to listen to webhook locally.
	before this we need to install stripe cli using scoop package manager.

		install stripe cli using following command
	scoop bucket add stripe https://github.com/stripe/scoop-stripe-cli.git
    scoop install stripe

	whsecret: whsec_XupnkKtw1tUebKtLvs98hhxyw7JlS0pr

# stripe listen -f  https://localhost:5001/api/Payments/webhook --skip-verify
	after running this if stripe sends any envent our webhook controller will be invoked.

	test card:
	4242 4242 4242 4242
	Declined card
	4000 0000 0000 0002 

	Test card to check insufficient fund
	4000 0000 0000 9995

	High security card
	4000 0027 6000 3184	



# Performance of api
	We implement caching in api to enhance performance

	for caching we have created CachedAttribute (custom attribute) inside helper filder of api.


# Installing sql server  in docker or local
		> install sql provider from nuget in infra project

		Microsoft.EntityFrameworkCore.SqlServer

		update project to 5
		update dotnet tool dotnet tool update -g dotnet-ef --version 5.0.3


# migration for sql server
	delete old migration folder ; right click and delete folder.

	delete old migration folder for identity also ; right click and delete folder.

	add postgre migration
	dotnet ef migrations add  "sqlserver store initials" -p infrastructure -s api -c StoreContext -o Data/Migrations	

	add migration for identity also
	dotnet ef migrations add  "sqlserver identity initials" -p infrastructure -s api -c AppIdentityDbContext -o Identity/Migrations


# api pre deployment work

	command to publish; dotnet publish -c Release -o publish skinet.sln
	run this inside root dire of solution. this will 

	add following line inside api itemgroup so that content directory will also be included inside publish dir.
	<Content Include="Content\**" CopyToPublishDirectory="PreserveNewest" />

	add following line inside infrastructure csproj to include seed data
	 <None Include="Data\SeedData\**" CopyToOutputDirectory="PreserveNewest" />

	 now publish code
	 dotnet publish -c Release -o publish skinet.sln

	 now seed file will be available inside debug and release so that system can get data from debug folder .


# deploy application to linux server (digital ocean)
	we will use digital ocean account for deploying to linux server
	https://www.digitalocean.com/

	id; mrinalkumarjha@ymail.com
	pass; m...1234


	


	
# To not get certificate errors we will also need to do the following:

1.  Create a new directory called “ssl” in the client folder.

2.  Copy the server.crt and server.key files from the StudentAssets folder into the newly created ssl folder:




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


# Angular build configuration
	outputHashing inside angular.json will create randome filename of js file on build. this will force 
	browser to load new file if browser has cached js.

	Things to change before production

	1: copy paste all key from environment.ts to environment.prod.ts
	2: Remove delay from loading interceptor.
	3: move node modules styles reference from angular.json to styles.css so that angular build will 
		compiled proper sequence so that once css will load after in sequence.

	4: Change outputpath from angular.json as per your need. this path is where you want to put your build file.
	5: set setting inside startup.cs if angular is being served using api

# development mode build
	 ng build :  this will create build file and place inside wwwroot folder of api

	 in development mode JIT compiler is used anf file size is bigger

	now we can see angular app running at https://localhost:5001/

# production mode build
	 ng build --prod :  this will create build file and place inside wwwroot folder of api

	 in production build AOT compiler is used and file size is minimal here.

	now we can see angular app running at https://localhost:5001/

# Upgrade to angular 11

install angular globally
npm install @angular/cli -g
this will install latest angular globally

> ng update @angular/core @angular/cli
this will update local angular to 11

> ng update
>ng update @angular/cdk
>ng update rxjs

check outdated package with npm outdated and update them also to wanted version.

then use tool npx to update all library in singal command.
or use individually for ex:  ng update rxjs

npx -p npm-check-updates ncu -u


# rxjs 

# observale
	observable is blue print of stream. and we can create instance of stream using subscribe on it.
	observable is just a defination of http stream.


when we create observable we add dollar $ symbal. its not necessary but its a convention to understand.
1. const interval$ = interval(1000);
	here interval$ is observable of type number. which will emit sequenceal number to its subscriber.
	when we subscribe to interval$ observable it create stream of data and emit to its subscriber.
	in this case subscriver of interval will receive sequential number in interval of 1 sec.

	we can subscribe to interval like this
	interval$.subscribe(val => console.log('stream 1 '+ val ));  // stream one as we have subscribed.

	interval$.subscribe(val => console.log('stream 2 '+ val ));  // stream 2 as we have subscribed again.

2. const interval$ = timer(3000, 1000);

    interval$.subscribe(val => console.log('stream 1 '+ val ));

	timer is observale which also emit sequential value but it gives us an option to initial waiting.
	in this case it will wait for 3 sec initially and then will start emiting sequantial value.


3.  const click$ = fromEvent(document, 'click');

    click$.subscribe(event => console.log(event ));

	fromEvent gives us flexibility to create stream of event. as in this case subscriber will
	get click event of document object.


4. Unsubscribe from observable.

	    const timer = interval(1000);
    const sub = timer.subscribe(num => console.log(num));


  setTimeout(() => sub.unsubscribe(), 5000);

  We can unsubscribe from observable.once unsubscribed from observable obserber will stop 
  getting stream value.

  in above case we are unsubscribing from observable after 5 sec using setTimeout.


5. Creating your own observable.

	const http$ = new  Observable(
       observer => {

        fetch('/api/courses')
        .then(resp => {
          return resp.json();
        })
        .then(body => {
          observer.next(body);
          observer.complete();
        })
        .catch(err => {
          observer.next(err);
        })

       });


       http$.subscribe(
         course => {console.log(course)},
         ()=>{},
         () => console.log('completed')
         );

6. RXJS OPERATOR
An operator is a pure function which takes in observable as input and the output is also an observable.

1. Pipe : pipe is used to chain operators.

2. Of : This operator will take in the arguments passed and convert them to observable.

3. map : map is transformation operator. In the case of map operator, a project function is applied on each value on the source Observable and the same output is emitted as an Observable. You use map to transform a collection of items into a collection of different items. 

function multiplyByTwo(collection) {
    return collection.map(function (value) {
        return value * 2;
    });
}

var a = of(1, 2, 3, 4, 0, 5);
var b = multiplyByTwo(a); // a new observable [2, 4, 6, 8, 0, 10]

4. shareReplay : if we use async pipe to subscribe observale then if multiple subscription is 
happening to same observable then muitiple call to database happen.
so to share same response to multiple subscriber we will use shareReplay.

  	beginnerCourses$: Observable<Course[]>;
    advanceCourses$: Observable<Course[]>;


	const http$ = createObservable('api/courses');
        const courses$: Observable<Course[]> = http$.pipe(
            tap(() => console.log('http req executed.')),
            map(res =>  Object.values(res['payload'])),
            shareReplay()
        );

		this.beginnerCourses$ = courses$.pipe(
            map((courses: Course[]) => courses
            .filter(course => course.category == 'BEGINNER'))
        )

        this.advanceCourses$ = courses$.pipe(
            map((courses: Course[]) =>  courses
            .filter(course => course.category == 'ADVANCED'))
        )

 in above example we have created two observale  beginnerCourses$, advanceCourses$ which will
 subscribes using async pipe in component. as two times we are subscribing there will be two net
 work call. so to share same network call to multiple subscriber we will use shareReplay() here.

 
