using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using System.Text;
using SeedStore.Database.Context;
using SeedStore.Support.General.ExceptionHandling.MIddlewares;
using SeedStore.Support.General.Logging.Interfaces;
using SeedStore.Support.General.Logging.Middlewares;
using SeedStore.Support.General.Logging.Models;
using SeedStore.Support.General.Logging.Repositories;
using SeedStore.Support.General.Logging.Services;
using SeedStore.Support.General.PasswordHash.Interfaces;
using SeedStore.Support.General.PasswordHash.Services;
using SeedStore.Support.General.Swagger.Filters;
using SeedStore.Support.General.TokenGeneration.Interfaces;
using SeedStore.Support.General.TokenGeneration.Services;
using SeedStore.Support.Store.Cleanup.Services;
using SeedStore.Support.Store.Email.Interfaces;
using SeedStore.Support.Store.Email.Services;
using SeedStore.Support.Store.Notifications.Hubs;
using SeedStore.Support.Store.Notifications.Interfaces;
using SeedStore.Support.Store.Notifications.Services;
using SeedStore.Support.Store.Payment.Interfaces;
using SeedStore.Support.Store.Payment.Services;
using SeedStore.Support.Admin.EmployeesActivity.Interfaces;
using SeedStore.Support.Admin.EmployeesActivity.Repository;
using SeedStore.Support.Admin.EmployeesActivity.Services;
using SeedStore.Support.Admin.PermissionsHandling.Handlers;
using SeedStore.Support.Admin.PermissionsHandling.Interfaces;
using SeedStore.Support.Admin.PermissionsHandling.Repositories;
using SeedStore.Support.Admin.PermissionsHandling.Requirements;
using SeedStore.Store.Authorization.Interfaces;
using SeedStore.Store.Authorization.Repositories;
using SeedStore.Store.Authorization.Services;
using SeedStore.Store.Account.Interfaces;
using SeedStore.Store.Account.Repositories;
using SeedStore.Store.Account.Services;
using SeedStore.Store.Catalog.Interfaces;
using SeedStore.Store.Catalog.Repositories;
using SeedStore.Store.Catalog.Services;
using SeedStore.Store.Products.Interfaces;
using SeedStore.Store.Products.Repositories;
using SeedStore.Store.Products.Services;
using SeedStore.Store.Reviews.Interfaces;
using SeedStore.Store.Reviews.Repositories;
using SeedStore.Store.Reviews.Services;
using SeedStore.Store.Orders.Interfaces;
using SeedStore.Store.Orders.Repositories;
using SeedStore.Store.Orders.Services;
using SeedStore.Store.StoreInfo.Interfaces;
using SeedStore.Store.StoreInfo.Repositories;
using SeedStore.Store.StoreInfo.Services;
using SeedStore.Admin.Authorization.Interfaces;
using SeedStore.Admin.Authorization.Repositories;
using SeedStore.Admin.Authorization.Services;
using SeedStore.Admin.Account.Interfaces;
using SeedStore.Admin.Account.Repositories;
using SeedStore.Admin.Account.Services;
using SeedStore.Admin.Catalog.Interfaces;
using SeedStore.Admin.Catalog.Repositories;
using SeedStore.Admin.Catalog.Services;
using SeedStore.Admin.Products.Interfaces;
using SeedStore.Admin.Products.Repositories;
using SeedStore.Admin.Products.Services;
using SeedStore.Admin.Reviews.Interfaces;
using SeedStore.Admin.Reviews.Repositories;
using SeedStore.Admin.Reviews.Services;
using SeedStore.Admin.Orders.Interfaces;
using SeedStore.Admin.Orders.Repositories;
using SeedStore.Admin.Orders.Services;
using SeedStore.Admin.Roles.Interfaces;
using SeedStore.Admin.Roles.Repositories;
using SeedStore.Admin.Roles.Services;
using SeedStore.Admin.Stats.Interfaces;
using SeedStore.Admin.Stats.Repositories;
using SeedStore.Admin.Stats.Services;
using SeedStore.Admin.StoreInfo.Interfaces;
using SeedStore.Admin.StoreInfo.Repositories;
using SeedStore.Admin.StoreInfo.Services;

var builder = WebApplication.CreateBuilder(args);

// Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

// Cache
builder.Services.AddMemoryCache();

// General support
builder.Services.AddScoped<IPasswordHashService, PasswordHashService>();
builder.Services.AddScoped<ITokenGenerationService, TokenGenerationService>();
builder.Services.AddScoped<ILoggingRepository, LoggingRepository>();
builder.Services.AddScoped<ILoggingService, LoggingService>();

// Store support
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddHostedService<CleanupService>();

// Admin support
builder.Services.AddScoped<IEmployeesActivityRepository, EmployeesActivityRepository>();
builder.Services.AddScoped<IEmployeesActivityService, EmployeesActivityService>();
builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
builder.Services.AddScoped<IAuthorizationHandler, PermissionHandler>();

// Store modules
builder.Services.AddScoped<IAuthorizationRepository, AuthorizationRepository>();
builder.Services.AddScoped<SeedStore.Store.Authorization.Interfaces.IAuthorizationService, AuthorizationService>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ICatalogRepository, CatalogRepository>();
builder.Services.AddScoped<ICatalogService, CatalogService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IReviewsRepository, ReviewsRepository>();
builder.Services.AddScoped<IReviewsService, ReviewsService>();
builder.Services.AddScoped<IOrdersRepository, OrdersRepository>();
builder.Services.AddScoped<IOrdersService, OrdersService>();
builder.Services.AddScoped<IStoreInfoRepository, StoreInfoRepository>();
builder.Services.AddScoped<IStoreInfoService, StoreInfoService>();

// Admin modules
builder.Services.AddScoped<IAdminAuthorizationRepository, AdminAuthorizationRepository>();
builder.Services.AddScoped<IAdminAuthorizationService, AdminAuthorizationService>();
builder.Services.AddScoped<IAdminAccountRepository, AdminAccountRepository>();
builder.Services.AddScoped<IAdminAccountService, AdminAccountService>();
builder.Services.AddScoped<IAdminCatalogRepository, AdminCatalogRepository>();
builder.Services.AddScoped<IAdminCatalogService, AdminCatalogService>();
builder.Services.AddScoped<IAdminProductsRepository, AdminProductsRepository>();
builder.Services.AddScoped<IAdminProductsService, AdminProductsService>();
builder.Services.AddScoped<IAdminReviewsRepository, AdminReviewsRepository>();
builder.Services.AddScoped<IAdminReviewsService, AdminReviewsService>();
builder.Services.AddScoped<IAdminOrdersRepository, AdminOrdersRepository>();
builder.Services.AddScoped<IAdminOrdersService, AdminOrdersService>();
builder.Services.AddScoped<IRolesRepository, RolesRepository>();
builder.Services.AddScoped<IRolesService, RolesService>();
builder.Services.AddScoped<IStatsRepository, StatsRepository>();
builder.Services.AddScoped<IStatsService, StatsService>();
builder.Services.AddScoped<IAdminStoreInfoRepository, AdminStoreInfoRepository>();
builder.Services.AddScoped<IAdminStoreInfoService, AdminStoreInfoService>();

// SignalR
builder.Services.AddSignalR();

// Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "Store";
    options.DefaultChallengeScheme = "Store";
})
.AddJwtBearer("Store", options =>
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
})
.AddJwtBearer("Admin", options =>
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
            context.Token = context.Request.Cookies["admin_access_token"];
            return Task.CompletedTask;
        }
    };
});

// Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("StorePolicy", policy =>
        policy.AddAuthenticationSchemes("Store").RequireAuthenticatedUser());

    options.AddPolicy("AdminPolicy", policy =>
        policy.AddAuthenticationSchemes("Admin").RequireAuthenticatedUser());

    options.AddPolicy("Permission:orders.manage", policy =>
        policy.AddAuthenticationSchemes("Admin")
              .RequireAuthenticatedUser()
              .AddRequirements(new PermissionRequirement("orders.manage")));

    options.AddPolicy("Permission:orders.tracking", policy =>
        policy.AddAuthenticationSchemes("Admin")
              .RequireAuthenticatedUser()
              .AddRequirements(new PermissionRequirement("orders.tracking")));

    options.AddPolicy("Permission:reviews.moderate", policy =>
        policy.AddAuthenticationSchemes("Admin")
              .RequireAuthenticatedUser()
              .AddRequirements(new PermissionRequirement("reviews.moderate")));

    options.AddPolicy("Permission:reviews.reply", policy =>
        policy.AddAuthenticationSchemes("Admin")
              .RequireAuthenticatedUser()
              .AddRequirements(new PermissionRequirement("reviews.reply")));

    options.AddPolicy("Permission:catalog.manage", policy =>
        policy.AddAuthenticationSchemes("Admin")
              .RequireAuthenticatedUser()
              .AddRequirements(new PermissionRequirement("catalog.manage")));

    options.AddPolicy("Permission:products.manage", policy =>
        policy.AddAuthenticationSchemes("Admin")
              .RequireAuthenticatedUser()
              .AddRequirements(new PermissionRequirement("products.manage")));

    options.AddPolicy("Permission:accounts.manage", policy =>
        policy.AddAuthenticationSchemes("Admin")
              .RequireAuthenticatedUser()
              .AddRequirements(new PermissionRequirement("accounts.manage")));

    options.AddPolicy("Permission:roles.manage", policy =>
        policy.AddAuthenticationSchemes("Admin")
              .RequireAuthenticatedUser()
              .AddRequirements(new PermissionRequirement("roles.manage")));

    options.AddPolicy("Permission:stats.view", policy =>
        policy.AddAuthenticationSchemes("Admin")
              .RequireAuthenticatedUser()
              .AddRequirements(new PermissionRequirement("stats.view")));

    options.AddPolicy("Permission:store.manage", policy =>
        policy.AddAuthenticationSchemes("Admin")
              .RequireAuthenticatedUser()
              .AddRequirements(new PermissionRequirement("store.manage")));
});

// Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("store", new OpenApiInfo { Title = "Store API", Version = "v1" });
    c.SwaggerDoc("admin", new OpenApiInfo { Title = "Admin API", Version = "v1" });
    c.AddSecurityDefinition("cookieAuth", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Cookie,
        Name = "access_token"
    });
    c.OperationFilter<AuthorizeOperationFilter>();
});

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Startup logging
var logger = app.Services.GetRequiredService<ILogger<Program>>();
Console.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}] APPLICATION STARTED");
using (var scope = app.Services.CreateScope())
{
    var loggingService = scope.ServiceProvider.GetRequiredService<ILoggingService>();
    await loggingService.LogAsync(new LoggingModel
    {
        Status = "ok",
        Address = "system:start",
        ExecutionTime = 0
    });
}

// Middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseCors("AllowAll");

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

app.MapHub<NotificationHub>("/hubs/notifications");
app.MapControllers();

// Shutdown logging
var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
lifetime.ApplicationStopping.Register(() =>
{
    using var scope = app.Services.CreateScope();
    var loggingService = scope.ServiceProvider.GetRequiredService<ILoggingService>();
    loggingService.LogAsync(new LoggingModel
    {
        Status = "ok",
        Address = "system:stop",
        ExecutionTime = 0
    }).GetAwaiter().GetResult();
    Console.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}] APPLICATION STOPPED");
});

app.Run();