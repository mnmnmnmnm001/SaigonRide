using Microsoft.EntityFrameworkCore;
using SaigonRide.Models;

namespace SaigonRide.Data
{
    public class SaigonRideContext : DbContext
    {
        public SaigonRideContext(DbContextOptions<SaigonRideContext> options) : base(options) {}

        public DbSet<User> Users { get; set; }
        public DbSet<LocalCommuter> LocalCommuters { get; set; }
        public DbSet<ForeignTourist> ForeignTourists { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Station> Stations { get; set; }
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<RentalTransaction> RentalTransactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure User inheritance (TPH - Table Per Hierarchy)
            modelBuilder.Entity<User>()
                .HasKey(u => u.BankNum);

            modelBuilder.Entity<User>()
                .HasDiscriminator<string>("UserType")
                .HasValue<LocalCommuter>("LocalCommuter")
                .HasValue<ForeignTourist>("ForeignTourist");

            // Configure Vehicle with Code as primary key
            modelBuilder.Entity<Vehicle>()
                .HasKey(v => v.Code);

            modelBuilder.Entity<Vehicle>()
                .HasMany(v => v.Rentals)
                .WithOne(r => r.Vehicle)
                .HasForeignKey(r => r.VehicleId);

            // Configure Station relationships
            modelBuilder.Entity<Station>()
                .HasMany(s => s.StartStations)
                .WithOne(r => r.StartStation)
                .HasForeignKey(r => r.StartStationId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Station>()
                .HasMany(s => s.EndStations)
                .WithOne(r => r.EndStation)
                .HasForeignKey(r => r.EndStationId)
                .OnDelete(DeleteBehavior.NoAction);

            // Configure Rental relationships
            modelBuilder.Entity<Rental>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Note: Seed data is managed via migrations (20260507_InitialCreate and 20260507_AddVehiclesToMatchCapacity)
        }
    }
}
