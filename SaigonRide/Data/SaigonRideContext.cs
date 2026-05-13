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
                new Station { StationId = 1, Name = "Ben Thanh Central Hub", CurrentCapacity = 5, MaxCapacity = 20 },
                new Station { StationId = 2, Name = "September 23rd Park Station", CurrentCapacity = 5, MaxCapacity = 15 },
                new Station { StationId = 3, Name = "Ham Nghi Transit Center", CurrentCapacity = 4, MaxCapacity = 15 },
                new Station { StationId = 4, Name = "Saigon Riverside Station", CurrentCapacity = 3, MaxCapacity = 12 },
                new Station { StationId = 5, Name = "Notre Dame Cathedral Stop", CurrentCapacity = 3, MaxCapacity = 10 },
                new Station { StationId = 6, Name = "East Gate Terminal", CurrentCapacity = 6, MaxCapacity = 25 },
                new Station { StationId = 7, Name = "West Gate Terminal", CurrentCapacity = 6, MaxCapacity = 25 },
                new Station { StationId = 8, Name = "New East City Hub", CurrentCapacity = 4, MaxCapacity = 20 },
                new Station { StationId = 9, Name = "An Suong Gateway", CurrentCapacity = 4, MaxCapacity = 15 },
                new Station { StationId = 10, Name = "Saigon Railway Station", CurrentCapacity = 4, MaxCapacity = 15 },
                new Station { StationId = 11, Name = "University Village Hub", CurrentCapacity = 8, MaxCapacity = 30 },
                new Station { StationId = 12, Name = "HCMC Tech Campus Station", CurrentCapacity = 5, MaxCapacity = 20 },
                new Station { StationId = 13, Name = "Economy University Stop", CurrentCapacity = 3, MaxCapacity = 12 },
                new Station { StationId = 14, Name = "Agriculture Campus Station", CurrentCapacity = 4, MaxCapacity = 15 },
                new Station { StationId = 15, Name = "Thao Diep Expat Village", CurrentCapacity = 4, MaxCapacity = 15 },
                new Station { StationId = 16, Name = "Phu My Hung Center", CurrentCapacity = 5, MaxCapacity = 20 },
                new Station { StationId = 17, Name = "Tan Son Nhat Airport Station", CurrentCapacity = 5, MaxCapacity = 15 },
                new Station { StationId = 18, Name = "Chinatown Terminal", CurrentCapacity = 6, MaxCapacity = 20 },
                new Station { StationId = 19, Name = "Gia Dinh Park Station", CurrentCapacity = 4, MaxCapacity = 15 },
                new Station { StationId = 20, Name = "Dam Sen Theme Park Stop", CurrentCapacity = 4, MaxCapacity = 15 }
            );

            // Seed Vehicles
            var vehicles = new List<Vehicle>();
            string[] types = { "StandardBike", "EBike", "Scooter" };
            string[] stationNames = { 
                "Ben Thanh Central Hub", "September 23rd Park Station", "Ham Nghi Transit Center", 
                "Saigon Riverside Station", "Notre Dame Cathedral Stop", "East Gate Terminal", 
                "West Gate Terminal", "New East City Hub", "An Suong Gateway", "Saigon Railway Station",
                "University Village Hub", "HCMC Tech Campus Station", "Economy University Stop",
                "Agriculture Campus Station", "Thao Diep Expat Village", "Phu My Hung Center",
                "Tan Son Nhat Airport Station", "Chinatown Terminal", "Gia Dinh Park Station",
                "Dam Sen Theme Park Stop"
            };

            int vCount = 1;
            foreach (var station in stationNames)
            {
                // Add 1 of each type to every station
                foreach (var type in types)
                {
                    string prefix = type == "StandardBike" ? "SB" : (type == "EBike" ? "EB" : "ES");
                    vehicles.Add(new Vehicle { 
                        Code = $"{prefix}{vCount:D3}", 
                        Type = type, 
                        FarePerMin = type == "StandardBike" ? 500 : (type == "EBike" ? 1000 : 1500), 
                        State = 0, 
                        CurrentPos = station 
                    });
                    vCount++;
                }
            }

            modelBuilder.Entity<Vehicle>().HasData(vehicles);

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
