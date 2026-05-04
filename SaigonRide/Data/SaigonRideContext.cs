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

            // Seed initial data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Stations
            modelBuilder.Entity<Station>().HasData(
                new Station { StationId = 1, Name = "Ben Thanh Market", CurrentCapacity = 3, MaxCapacity = 12 },
                new Station { StationId = 2, Name = "District 1 Hub", CurrentCapacity = 3, MaxCapacity = 10 },
                new Station { StationId = 3, Name = "Saigon Center", CurrentCapacity = 2, MaxCapacity = 5 }
            );

            // Seed Vehicles
            modelBuilder.Entity<Vehicle>().HasData(
                new Vehicle { Code = "SB001", Type = "StandardBike", FarePerMin = 500, State = 0, CurrentPos = "Ben Thanh Market" },
                new Vehicle { Code = "SB002", Type = "StandardBike", FarePerMin = 500, State = 0, CurrentPos = "Ben Thanh Market" },
                new Vehicle { Code = "EB001", Type = "EBike", FarePerMin = 1000, State = 0, CurrentPos = "Ben Thanh Market" },
                new Vehicle { Code = "EB002", Type = "EBike", FarePerMin = 1000, State = 0, CurrentPos = "District 1 Hub" },
                new Vehicle { Code = "ES001", Type = "Scooter", FarePerMin = 1500, State = 0, CurrentPos = "Saigon Center" },
                new Vehicle { Code = "ES002", Type = "Scooter", FarePerMin = 1500, State = 0, CurrentPos = "District 1 Hub"},
                new Vehicle { Code = "ES003", Type = "Scooter", FarePerMin = 1500, State = 0, CurrentPos = "Saigon Center" },
                new Vehicle { Code = "ES004", Type = "Scooter", FarePerMin = 1500, State = 0, CurrentPos = "District 1 Hub" }
            );

            // Seed Users
            modelBuilder.Entity<LocalCommuter>().HasData(
                new LocalCommuter { BankNum = "0123456789", ChosenPaymentCode = "momo", Payed = 50000, P_MoMo = true, P_VNPay = false },
                new LocalCommuter { BankNum = "0987654321", ChosenPaymentCode = "vnpay", Payed = 100000, P_MoMo = true, P_VNPay = true }
            );

            modelBuilder.Entity<ForeignTourist>().HasData(
                new ForeignTourist { BankNum = "INTL001", ChosenPaymentCode = "applepay", Payed = 75000, Passport = "A12345678", P_ApplePay = true, P_PayPal = false },
                new ForeignTourist { BankNum = "INTL002", ChosenPaymentCode = "paypal", Payed = 150000, Passport = "B98765432", P_ApplePay = false, P_PayPal = true }
            );
        }
    }
}
