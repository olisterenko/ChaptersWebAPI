using Chapters;
using Chapters.Entities;
using Chapters.Extensions;
using Chapters.Requests;
using Chapters.Services;
using Chapters.Services.Interfaces;
using Chapters.Validators;
using FluentMigrator.Runner;
using FluentValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var services = builder.Services;

// Add services to the container.

services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwagger();

services.AddAuthentication("BasicAuthentication")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);
services.AddAuthorization();

services.AddScoped<IRepository<User>, Repository<User>>();
services.AddScoped<IUserService, UserService>();
services.AddScoped<IPasswordHasher, PasswordHasher>();
services.Configure<PasswordHasherOptions>(configuration.GetSection("PasswordHasherSettings"));

services.AddDbContextWithRepositories(configuration);

services.AddFluentMigrator(configuration);
services.AddValidators();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

using var scope = app.Services.CreateScope();
var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
runner.MigrateUp();

app.Run();
