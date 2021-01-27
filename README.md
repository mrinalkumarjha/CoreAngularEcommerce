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
	
