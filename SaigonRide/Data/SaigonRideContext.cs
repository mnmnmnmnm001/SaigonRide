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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure User inheritance (TPH - Table Per Hierarchy)
            modelBuilder.Entity<User>()
                .HasDiscriminator<string>("UserType")
                .HasValue<LocalCommuter>("LocalCommuter")
                .HasValue<ForeignTourist>("ForeignTourist");

            // Configure Vehicle relationships
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

            // Seed initial data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Stations
            modelBuilder.Entity<Station>().HasData(
                new Station { StationId = 1, Name = "Ben Thanh Market", CurrentCapacity = 15, MaxCapacity = 20, Latitude = 10.7725, Longitude = 106.6992, Ratio = 0.75 },
                new Station { StationId = 2, Name = "District 1 Hub", CurrentCapacity = 8, MaxCapacity = 30, Latitude = 10.7758, Longitude = 106.7008, Ratio = 0.27 },
                new Station { StationId = 3, Name = "Saigon Center", CurrentCapacity = 28, MaxCapacity = 35, Latitude = 10.7769, Longitude = 106.7009, Ratio = 0.80 },
                new Station { StationId = 4, Name = "Tan Binh Station", CurrentCapacity = 5, MaxCapacity = 25, Latitude = 10.8074, Longitude = 106.6756, Ratio = 0.20 }
            );

            // Seed Vehicles
            modelBuilder.Entity<Vehicle>().HasData(
                new Vehicle { VehicleId = 1, Type = "StandardBike", FarePerMin = 500, State = 0, Code = "SB001", CurrentPos = "Station 1" },
                new Vehicle { VehicleId = 2, Type = "StandardBike", FarePerMin = 500, State = 0, Code = "SB002", CurrentPos = "Station 1" },
                new Vehicle { VehicleId = 3, Type = "EBike", FarePerMin = 1000, State = 0, Code = "EB001", CurrentPos = "Station 2" },
                new Vehicle { VehicleId = 4, Type = "EBike", FarePerMin = 1000, State = 0, Code = "EB002", CurrentPos = "Station 2" },
                new Vehicle { VehicleId = 5, Type = "Scooter", FarePerMin = 1500, State = 0, Code = "ES001", CurrentPos = "Station 3" },
                new Vehicle { VehicleId = 6, Type = "Scooter", FarePerMin = 1500, State = 0, Code = "ES002", CurrentPos = "Station 3" }
            );

            // Seed Users
            modelBuilder.Entity<LocalCommuter>().HasData(
                new LocalCommuter { UserId = 1, BankNum = "0123456789", ChosenPaymentCode = "momo", Payed = 50000, P_MoMo = true, P_VNPay = false },
                new LocalCommuter { UserId = 2, BankNum = "0987654321", ChosenPaymentCode = "vnpay", Payed = 100000, P_MoMo = true, P_VNPay = true }
            );

            modelBuilder.Entity<ForeignTourist>().HasData(
                new ForeignTourist { UserId = 3, BankNum = "INTL001", ChosenPaymentCode = "applepay", Payed = 75000, Passport = "A12345678", P_ApplePay = true, P_PayPal = false },
                new ForeignTourist { UserId = 4, BankNum = "INTL002", ChosenPaymentCode = "paypal", Payed = 150000, Passport = "B98765432", P_ApplePay = false, P_PayPal = true }
            );
        }
    }
}
