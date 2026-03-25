using Microsoft.EntityFrameworkCore;
using SeedStore.Database.Entities.Store.Catalog;
using SeedStore.Database.Entities.Store.Products;
using SeedStore.Database.Entities.Store.Account;
using SeedStore.Database.Entities.Store.Orders;
using SeedStore.Database.Entities.Store.Reviews;
using SeedStore.Database.Entities.Store.StoreInfo;
using SeedStore.Database.Entities.Admin.Account;
using SeedStore.Database.Entities.Admin.Orders;
using SeedStore.Database.Entities.Admin.Roles;
using SeedStore.Database.Entities.Support.Logging;

namespace SeedStore.Database.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Store — Catalog
        public DbSet<CategoryEntity> Categories { get; set; }

        // Store — Products
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<ProductImageEntity> ProductImages { get; set; }
        public DbSet<FeatureEntity> Features { get; set; }
        public DbSet<FeatureHeaderEntity> FeatureHeaders { get; set; }
        public DbSet<FilterValueEntity> FilterValues { get; set; }
        public DbSet<ProductFeatureEntity> ProductFeatures { get; set; }
        public DbSet<DiscountGroupEntity> DiscountGroups { get; set; }
        public DbSet<DiscountEntity> Discounts { get; set; }

        // Store — Account
        public DbSet<AccountEntity> Accounts { get; set; }
        public DbSet<PendingAccountEntity> PendingAccounts { get; set; }
        public DbSet<RefreshTokenEntity> RefreshTokens { get; set; }
        public DbSet<PasswordResetRequestEntity> PasswordResetRequests { get; set; }
        public DbSet<EmailChangeRequestEntity> EmailChangeRequests { get; set; }

        // Store — Orders
        public DbSet<OrderEntity> Orders { get; set; }
        public DbSet<OrderItemEntity> OrderItems { get; set; }
        public DbSet<OrderStatusEntity> OrderStatuses { get; set; }
        public DbSet<OrderTransactionEntity> OrderTransactions { get; set; }

        // Store — Reviews
        public DbSet<ReviewEntity> Reviews { get; set; }
        public DbSet<ReviewReplyEntity> ReviewReplies { get; set; }
        public DbSet<ReviewStatusEntity> ReviewStatuses { get; set; }

        // Store — StoreInfo
        public DbSet<DeliveryVariantEntity> DeliveryVariants { get; set; }
        public DbSet<PaymentVariantEntity> PaymentVariants { get; set; }

        // Admin — Account
        public DbSet<EmployeeEntity> Employees { get; set; }
        public DbSet<EmployeeRefreshTokenEntity> EmployeeRefreshTokens { get; set; }
        public DbSet<BlockedEmployeeEntity> BlockedEmployees { get; set; }

        // Admin — Roles
        public DbSet<RoleEntity> Roles { get; set; }
        public DbSet<PermissionEntity> Permissions { get; set; }
        public DbSet<RolePermissionEntity> RolePermissions { get; set; }

        // Admin — Orders
        public DbSet<OrderActionEntity> OrderActions { get; set; }
        public DbSet<OrderProcessingEntity> OrderProcessings { get; set; }

        // Support
        public DbSet<EmployeeActivityEntity> EmployeesActivity { get; set; }
        public DbSet<LoggingEntity> Logs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}