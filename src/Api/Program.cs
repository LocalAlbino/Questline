using Questline.Api.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication()
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["Jwt:Authority"];
        options.Audience = builder.Configuration["Jwt:Audience"];
    });

if (builder.Environment.IsDevelopment())
    builder.Services.AddSqlite<AppDbContext>(builder.Configuration.GetConnectionString("DefaultConnection"));

builder.Services.AddAuthorizationBuilder();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
