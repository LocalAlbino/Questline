using Microsoft.AspNetCore.Identity;
using Questline.Api.Data;
using Questline.Api.Endpoints;
using Questline.Api.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddIdentityApiEndpoints<AppUser>()
    .AddRoles<IdentityRole<Guid>>()
    .AddEntityFrameworkStores<AppDbContext>();

if (builder.Environment.IsDevelopment())
    builder.Services.AddSqlite<AppDbContext>(builder.Configuration.GetConnectionString("DefaultConnection"));

builder.Services.AddAuthorizationBuilder();

builder.Services.AddValidation();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapGroup("api/identity").MapIdentityApi<AppUser>();

app.MapBoardsEndpoints();
app.MapBoardsGroupsEndpoints();
app.MapGroupsEndpoints();

app.Run();