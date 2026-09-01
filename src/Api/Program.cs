using Microsoft.AspNetCore.Identity;
using Questline.Api.Data;
using Questline.Api.Models;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddIdentityApiEndpoints<AppUser>()
    .AddRoles<IdentityRole<Guid>>()
    .AddEntityFrameworkStores<AppDbContext>();

if (builder.Environment.IsDevelopment())
    builder.Services.AddSqlite<AppDbContext>(builder.Configuration.GetConnectionString("DefaultConnection"));

builder.Services.AddAuthorizationBuilder();

var app = builder.Build();

app.MapGroup("api/identity").MapIdentityApi<AppUser>();

app.Run();
