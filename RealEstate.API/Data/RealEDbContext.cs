using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RealEstate.API.Models;

namespace RealEstate.API.Data
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
            base.OnModelCreating(builder);
        }
        public DbSet<EstateProperty> EstateProperties { get; set; }
        public DbSet<OwnerSchedule> OwnerSchedules { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<SaleorRentModel> SaleorRentModel { get; set; }
        public DbSet<PropertyCategory> PropertyCategories { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    }

}
