using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using seed_store_api.Database.Context;
using seed_store_api.Store.Modules.Authorization.Interfaces;
using seed_store_api.Store.Modules.Authorization.Repositories;
using seed_store_api.Store.Modules.Authorization.Services;
using seed_store_api.Store.Modules.Catalog.Interfaces;
using seed_store_api.Store.Modules.Catalog.Repositories;
using seed_store_api.Store.Modules.Catalog.Services;
using seed_store_api.Store.Modules.Products.Interfaces;
using seed_store_api.Store.Modules.Products.Repositories;
using seed_store_api.Store.Modules.Products.Services;
using seed_store_api.Store.Modules.StoreInfo.Interfaces;
using seed_store_api.Store.Modules.StoreInfo.Repositories;
using seed_store_api.Store.Modules.StoreInfo.Services;
using seed_store_api.Store.Support.Email.Interfaces;
using seed_store_api.Store.Support.Email.Services;
using seed_store_api.Store.Support.TokenGeneration.Interfaces;
using seed_store_api.Store.Support.TokenGeneration.Services;
using seed_store_api.Store.Support.PasswordHash.Interfaces;
using seed_store_api.Store.Support.PasswordHash.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using seed_store_api.Store.Support.Cleanup.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

builder.Services.AddScoped<IStoreInfoRepository, StoreInfoRepository>();
builder.Services.AddScoped<IStoreInfoService, StoreInfoService>();

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddScoped<IAuthorizationRepository, AuthorizationRepository>();
builder.Services.AddScoped<IAuthorizationService, AuthorizationService>();

builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IPasswordHashService, PasswordHashService>();
builder.Services.AddScoped<ITokenGenerationService, TokenGenerationService>();

builder.Services.AddHostedService<CleanupService>();

builder.Services.AddMemoryCache();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("store", new OpenApiInfo { Title = "Store API", Version = "v1" });
    c.SwaggerDoc("admin", new OpenApiInfo { Title = "Admin API", Version = "v1" });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!))
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                context.Token = context.Request.Cookies["access_token"];
                return Task.CompletedTask;
            }
        };
    });

var app = builder.Build();

app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/store/swagger.json", "Store API");
        c.SwaggerEndpoint("/swagger/admin/swagger.json", "Admin API");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
