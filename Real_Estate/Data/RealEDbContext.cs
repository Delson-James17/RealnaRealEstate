using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Real_Estate.Models;
using System.Reflection.Emit;

namespace Real_Estate.Data
{
    public class RealEDbContext : IdentityDbContext<ApplicationUser>
    {
        private ILogger _logger { get; }
        private IConfiguration _appConfig { get; }

        public RealEDbContext(
            ILogger<RealEDbContext> logger,
            IConfiguration appConfig)
        {
            this._logger = logger;
            this._appConfig = appConfig;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var server = this._appConfig.GetConnectionString("Server");
            var db = this._appConfig.GetConnectionString("DB");
            var username = this._appConfig.GetConnectionString("UserName");
            var password = this._appConfig.GetConnectionString("Password");

            //string connectionString = $"Server={server};Database={db};User Id={username};Password={password};MultipleActiveResultSets=true";

            string connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=RealEDb;Integrated Security=True;MultipleActiveResultSets=true;";

            optionsBuilder
                .UseSqlServer(connectionString)
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            /*   builder.Entity<EstateProperty>()
               .HasOne(p => p.PropertyCategory)
               .WithMany(c => c.EstateProperties)
               .HasForeignKey(p => p.PropertyCategoryId);*/

            builder.Entity<ApplicationUser>()
                .HasMany(u => u.Properties)
                .WithOne(n => n.ApplicationUser)
                .HasForeignKey(n => n.ApplicationUserId)
                .HasPrincipalKey(u => u.Id);

            builder.RolesSeed();
            builder.EstatePropertyCategorySeed();
            builder.SaleOrRentSeed();
            builder.UserSeed();
            builder.EstatePropertySeed();
            builder.UserRoleSeed();
            base.OnModelCreating(builder);
        }
        public DbSet<PropertyCategory> PropertyCategories { get; set; }
        public DbSet<EstateProperty> EstateProperties { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<OwnerSchedule> OwnerSchedules { get; set; }
        public DbSet<Real_Estate.Models.SaleorRentModel>? SaleorRentModel { get; set; }
    }

}
